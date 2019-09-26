using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class AddCategoryRequestModel : LoginUserInformation
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public long ApplicationUserId { get; set; }
    }

    public class AddCategoryRequestModelValidation : AbstractValidator<AddCategoryRequestModel>
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryRequestModelValidation(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            When(d => !String.IsNullOrWhiteSpace(d.Name), () =>
            {
                RuleFor(x => x.Name).NotNull().MinimumLength(2).MaximumLength(100).MustAsync(_categoryRepository.CheckNameAlreadyExits);

            });

            When(d => !String.IsNullOrWhiteSpace(d.Description), () =>
            {
                RuleFor(x => x.Description).NotNull().MinimumLength(2).MaximumLength(100);

            });
        }
    }
}
