using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Models;
using BookShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace BookShop.Api.RequestResponse.Request
{
    public class RegistrationRequestModel
    {
        public string Email { get; set; }   
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class RegistrationRequestModelValidator : AbstractValidator<RegistrationRequestModel>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoginService _loginService;

        public RegistrationRequestModelValidator(UserManager<ApplicationUser> _userManager, ILoginService loginService)
        {
            this._userManager = _userManager;
            _loginService = loginService;

            RuleFor(x => x.Email).NotNull().EmailAddress().MustAsync(_loginService.EmailExists)
                .WithMessage("Email already exists in our system.");
            RuleFor(x => x.PhoneNumber).NotNull().MinimumLength(11).MustAsync(_loginService.PhoneNumberExists)
                .WithMessage("Phone number already exists in our system");
            RuleFor(x => x.Password).NotNull().MinimumLength(6);
        }
    }
}
