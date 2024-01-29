using FluentValidation;

namespace WebApi.Web.Validators
{
    public class JsonFileIdValidator : AbstractValidator<string>
    {
        public JsonFileIdValidator()
        {
            RuleFor(id => id)
                .NotEmpty().WithMessage("JSON File ID is required.");
        }
    }

}
