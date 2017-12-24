using Login.Data.Entities;
using Login.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MyCodeCamp.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Login.Web.Controllers
{
    public class AuthController : Controller
    {
        private SignInManager<ApplicationUser> _signInMgr;

        private UserManager<ApplicationUser> _userMgr;

        private IPasswordHasher<ApplicationUser> _hasher;

        public AuthController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> hasher)
        {
            this._signInMgr = signInManager;
            this._userMgr = userManager;
            this._hasher = hasher;
        }

        [HttpPost("api/auth/login")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] User model)
        {
            try
            {
                var result = await _signInMgr.PasswordSignInAsync(model.Email, model.Password, false, false);
                if (result.Succeeded)
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception thrown while logging in: {ex}");
            }

            return BadRequest("Failed to login");
        }


        [ValidateModel]
        [HttpPost("api/auth/token")]
        public async Task<IActionResult> CreateToken([FromBody] User model)
        {
            try
            {
                var user = await _userMgr.FindByNameAsync(model.Email);
                if (user != null)
                {
                    if (_hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {
                        var userClaims = await _userMgr.GetClaimsAsync(user);

                        DateTime exp = new DateTime(2017, 12, 30);

                        var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                new Claim(JwtRegisteredClaimNames.Email, user.Email)
                            }.Union(userClaims);

                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("VERYLONGKEYVALUETHATISSECURE"));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                          issuer: "http://localhost:3485/",
                          audience: "http://localhost:3485/",
                          claims: claims,
                          expires: DateTime.UtcNow.AddSeconds(30),
                          signingCredentials: creds
                          );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });
                    }
                }

            }
            catch (Exception ex)
            {
                //_logger.LogError($"Exception thrown while creating JWT: {ex}");
            }

            return BadRequest("Failed to generate jwt token");
        }


    }
}
