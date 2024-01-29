using FluentValidation;

namespace WebApi.Web.Validators
{

    public class FileValidator : AbstractValidator<IFormFile>
    {
        public FileValidator()
        {
            RuleFor(file => file)
                .NotNull().WithMessage("File is required.");

            RuleFor(file => file.Length)
                .GreaterThan(0).WithMessage("File cannot be empty.");
        }
    }

}
