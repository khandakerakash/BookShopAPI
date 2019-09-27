using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class AddAuthorRequestModel : LoginUserInformation
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AddAuthorRequestModelValidator : AbstractValidator<AddAuthorRequestModel>
    {
        private readonly IAuthorRepository _authorRepository;

        public AddAuthorRequestModelValidator(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            RuleFor(x => x.FirstName).NotNull().MaximumLength(20);
            RuleFor(x => x.LastName).NotNull().MaximumLength(20);
        }
    }
}
