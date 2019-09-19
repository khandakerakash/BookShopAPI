using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class AuthorRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class AuthorRequestModelValidator : AbstractValidator<AuthorRequestModel>
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorRequestModelValidator(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
            RuleFor(x => x.FirstName).NotNull().MaximumLength(20);
            RuleFor(x => x.LastName).NotNull().MaximumLength(20);
        }
    }
}
