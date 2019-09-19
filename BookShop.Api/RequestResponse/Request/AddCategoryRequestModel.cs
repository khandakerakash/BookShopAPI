using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class AddCategoryRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AddCategoryRequestModelValidation : AbstractValidator<AddCategoryRequestModel>
    {
        private readonly ICategoryRepository _categoryRepository;

        public AddCategoryRequestModelValidation(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            RuleFor(x => x.Name).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.Description).NotNull().MinimumLength(20).MaximumLength(500);
        }
    }
}
