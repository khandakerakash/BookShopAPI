using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class UpdateCategoryRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class UpdateCategoryRequestModelValidation : AbstractValidator<UpdateCategoryRequestModel>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryRequestModelValidation(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
            RuleFor(x => x.Name).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.Description).NotNull().MinimumLength(50).MaximumLength(500);
        }
    }
}
