
using eCommerceAPI.Application.ViewModels.Products;
using FluentValidation;

namespace eCommerceAPI.Application.Validators.Products
{
    public class CreateProductValidator: AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                    .WithMessage("Product name cannot be empty.")
                .NotNull()
                    .WithMessage("Product name cannot be empty.")
                .MaximumLength(150)
                .MinimumLength(5)
                    .WithMessage("Product name must be between 5 and 150 charachter");
            RuleFor(p => p.Stock)
                .NotEmpty()
                    .WithMessage("Product name cannot be empty.")
                .NotNull()
                    .WithMessage("Product name cannot be empty.")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Stock must be greater than or equal to zero.");
            RuleFor(p => p.Price)
                .NotEmpty()
                    .WithMessage("Product name cannot be empty.")
                .NotNull()
                    .WithMessage("Product name cannot be empty.")
                .GreaterThan(0)
                    .WithMessage("Price must be greater than zero.");
        }
    }
}

