using FluentValidation;

namespace Hb.Application.Commands.ProductCreate
{
    public class ProductCreateValidator : AbstractValidator<ProductCreateCommand>
    {
        public ProductCreateValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty();

            RuleFor(v => v.Price)
                .NotEmpty();

            RuleFor(v => v.Currency)
               .NotEmpty();
        }
    }
}
