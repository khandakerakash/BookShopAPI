using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;

namespace BookShop.Api.RequestResponse.Request
{
    public class UpdateCategoryRequestModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public long ApplicationUserId { get; set; }
    }

    public class UpdateCategoryRequestModelValidation : AbstractValidator<UpdateCategoryRequestModel>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryRequestModelValidation(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;

            When(d =>! String.IsNullOrWhiteSpace(d.Name), () =>
            {
                RuleFor(x => x.Name).NotNull().MinimumLength(2).MaximumLength(100).MustAsync(_categoryRepository.CheckNameAlreadyExits);

            });

            When(d => !String.IsNullOrWhiteSpace(d.Description), () =>
            {
                RuleFor(x => x.Description).NotNull().MinimumLength(2).MaximumLength(100);

            });
            //            _categoryRepository = categoryRepository;
            //            RuleFor(x => x.Name).NotNull()..MinimumLength(2).MaximumLength(100);
            //          //  RuleFor(x => x.Description).NotNull().MinimumLength(50).MaximumLength(500);
            //            //RuleFor(x => x.ApplicationUserId).NotNull().GreaterThan(0);
        }
    }
}
