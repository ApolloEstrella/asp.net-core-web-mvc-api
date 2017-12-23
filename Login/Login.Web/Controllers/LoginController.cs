using Login.Data;
using Login.Data.Entities;
using Login.Web.Helpers;
using Login.Web.Models;
using Microsoft.AspNetCore.Authorization;
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

        public LoginController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager, ILoginRepository repository)
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

            var userExists =  await  _userManager.FindByEmailAsync(userParam.Email);

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
            if (result != IdentityResult.Success)
            {

                //await return new ValidationResult("ProductId is invalid");
                return BadRequest();
            }
            return Ok();
        }

        [Route("api/[controller]")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] User userParam)
        {

            var signIn = await _signInManager.PasswordSignInAsync(userParam.Email, userParam.Password, false, false);

            if (!signIn.Succeeded)
                ModelState.AddModelError(nameof(userParam), "Email or Password is incorrect !");
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

    }

     
}
