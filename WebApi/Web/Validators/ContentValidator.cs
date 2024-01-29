using FluentValidation;

namespace WebApi.Web.Validators
{
    public class ContentValidator : AbstractValidator<object>
    {
        public ContentValidator()
        {
            RuleFor(x => x.ToString()).NotNull().NotEmpty();
        }
    }

}
