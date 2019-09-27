using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class UpdateAuthorRequestModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UpdateAuthorRequestModelValidator : AbstractValidator<UpdateAuthorRequestModel>
    {
        private readonly IAuthorRepository _authorRepository;

        public UpdateAuthorRequestModelValidator(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;

            When(d => !String.IsNullOrWhiteSpace(d.FirstName), () =>
            {
                RuleFor(x => x.FirstName).NotNull().MaximumLength(20);
            });

            When(d => !String.IsNullOrWhiteSpace(d.LastName), () =>
            {
                RuleFor(x => x.LastName).NotNull().MaximumLength(20);
            });   
        }
    }
}
