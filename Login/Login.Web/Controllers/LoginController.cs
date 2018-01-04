using Login.Data;
using Login.Data.Entities;
using Login.Web.Helpers;
using Login.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Login.Web.Controllers
{

    public class LoginController : Controller
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        private readonly ILoginRepository _repository;

        public LoginController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILoginRepository repository)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._repository = repository;
        }

        [Authorize]
        [Route("api/[controller]/[action]")]
        [HttpGet]
        public IEnumerable<ApplicationUser> Users()
        {
            return _repository.Getusers();
        }


        [Route("api/[controller]/[action]")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] User userParam)
        {
            try
            {
                var userExists = await _userManager.FindByEmailAsync(userParam.Email);

                if (userExists != null)
                {
                    ModelState.AddModelError(nameof(userParam), "User already exists.");
                }

                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }

                var user = new ApplicationUser
                {
                    UserName = userParam.Email,
                    Email = userParam.Email

                };

                var result = await _userManager.CreateAsync(user, userParam.Password);
                if (result == IdentityResult.Success)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmUrl = string.Concat("http://localhost:4200/login?", "userid=" + user.Id + "&token=" + token);
                    SendConfirmationEmail(confirmUrl);
                    //await return new ValidationResult("ProductId is invalid");               
                }
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {

            }
            return Json("For email verification, check your email for new mail and click the link to log-in.");
        }

        [Route("api/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User userParam, string userid, string token)
        {
            var signIn = await _signInManager.PasswordSignInAsync(userParam.Email, userParam.Password, false, false);

            if (!signIn.Succeeded)
                ModelState.AddModelError(nameof(userParam), "Email or Password is incorrect !");

            // Require the user to have a confirmed email before they can log on.
            var user = await this._userManager.FindByEmailAsync(userParam.Email);
            if (user != null)
            {
                if (!await this._userManager.IsEmailConfirmedAsync(user))
                {
                    if (!string.IsNullOrWhiteSpace(userid))
                        await ConfirmEmail(userid, token);
                    else
                        ModelState.AddModelError(nameof(userParam),
                                  "You must have a confirmed email to log in.");
                }
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            return Ok();
        }

        [Route("api/[controller]/[action]")]
        [HttpDelete]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var isDeleted = await _userManager.DeleteAsync(user as ApplicationUser);
                if (!isDeleted.Succeeded)
                    ModelState.AddModelError(nameof(id), "Delete not successful!");
                if (!ModelState.IsValid)
                {
                    return new UnprocessableEntityObjectResult(ModelState);
                }
            }
            return Ok();
        }

        //[Route("api/[controller]/[action]")]
        //[HttpPost]
        private void SendConfirmationEmail(string confirmUrl)
        {
            EmailService.Send("apollo.estrella@gmail.com", "Email Confirmation", confirmUrl);
        }

        private async Task<ActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                var identityResult = await _userManager.ConfirmEmailAsync(user, token);
                if (identityResult.Succeeded)
                    return Ok();
                else
                    return BadRequest();
            }
            catch (Exception ex)
            {

            }
            return Ok();
        }

        [Route("api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> ForgotPassword([FromBody] User userModel)
        {
            var user = await _userManager.FindByNameAsync(userModel.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var confirmUrl = string.Concat("http://localhost:4200/login/reset-password?", "userid=" + user.Id + "&token=" + token);
                SendConfirmationEmail(confirmUrl);
            }
            return Ok();
        }

        [Route("api/[controller]/[action]")]
        [HttpPost]
        public async Task<ActionResult> ResetPassword([FromBody] User userModel)
        {
            var user = await _userManager.FindByIdAsync(userModel.Id);
            if (user != null)
            {
                var identityResult = await _userManager.ResetPasswordAsync(user, userModel.Token, userModel.Password);
                if (!identityResult.Succeeded)
                    return BadRequest();                
            }
            return Ok();
        }
    }


}
