using FluentValidation;

namespace Core.Features.ProductManagement.Commands.Create
{
    public class ProductValidator : AbstractValidator<CreateProductCommand>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MaximumLength(100)
                .WithMessage("Product name must not exceed 100 characters.");
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .WithMessage("Product price must be greater than zero.");
            RuleFor(x => x.StockCount)
                .NotEmpty()
                .WithMessage("Stock count is required.")
                .GreaterThan(0)
                .WithMessage("Stock count must be greater than zero.");
            RuleFor(x => x.CategoryId).NotEmpty()
                .WithMessage("Category ID is required.")
                .NotEqual(Guid.Empty)
                .WithMessage("Category ID must not be empty.");
            RuleFor(x => x.ManagerId).NotEmpty()
             .WithMessage("Manager ID is required.");


        }
    }

}
