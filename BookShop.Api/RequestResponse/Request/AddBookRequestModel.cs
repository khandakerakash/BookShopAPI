using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShop.Api.Services;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace BookShop.Api.RequestResponse.Request
{
    public class AddBookRequestModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public long AuthorId { get; set; }
        //public long CategoryId { get; set; }
    }

    public class AddBookRequestModelValidator : AbstractValidator<AddBookRequestModel>
    {
        private readonly IBookRepository _bookRepository;

        public AddBookRequestModelValidator(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
            RuleFor(x => x.Title).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.Description).NotNull().MinimumLength(50).MaximumLength(500);
            RuleFor(x => x.Image).NotNull();
            RuleFor(x => x.Price).NotNull().GreaterThan(0);
            RuleFor(x => x.Quantity).NotNull().GreaterThan(0);
            RuleFor(x => x.AuthorId).NotNull().GreaterThan(0)
                .MustAsync(_bookRepository.AuthorExists).WithMessage("This author Id is not our system.");
            //RuleFor(x => x.CategoryId).NotNull().GreaterThan(0).MustAsync(_bookRepository.CategoryExists).WithMessage("This category Id is not our system.");
        }
    }
}
