using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Models;
using BookShop.Api.RequestResponse.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookShop.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegistrationController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException();
        }

        // POST: api/manager-register
        [Authorize(AuthenticationSchemes = "Bearer", Roles = "owner")]
        [HttpPost("manager-register")]
        public async Task<IActionResult> ManagerRegistration(RegistrationRequestModel request)
        {
            return await RegisterUser(request, "manager");
        }

        // POST: api/customer-register
        [HttpPost("customer-register")]
        public async Task<IActionResult> CustomerRegister(RegistrationRequestModel request)
        {
            return await RegisterUser(request, "customer");

        }

        private async Task<IActionResult> RegisterUser(RegistrationRequestModel request, string roleName)
        {
            var user = await _userManager.CreateAsync(new ApplicationUser()
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.Email
            }, request.Password);

            if (user.Succeeded)
            {
                var nowInsertedUser = await _userManager.FindByEmailAsync(request.Email);
                var roleInsert = await _userManager.AddToRoleAsync(nowInsertedUser, roleName);
                return Ok(new
                {
                    Success = true,
                    Message = "User created successfully."
                });
            }

            return BadRequest(user.Errors);
        }
    }
}