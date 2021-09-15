using FluentValidation;

namespace Hb.Application.Commands.CategoryCreate
{
    public class CategoryCreateValidator : AbstractValidator<CategoryCreateCommand>
    {
        public CategoryCreateValidator()
        {
            RuleFor(v => v.Name)
                .NotEmpty();
        }
    }
}
