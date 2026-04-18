using FluentValidation;

namespace StaffManagement.Application.Positions.Commands
{
    public class CreatePositionCommandValidator : AbstractValidator<CreatePositionCommand>
    {

        public CreatePositionCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Position title is required.")
                .MaximumLength(100);

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department is required.");

            RuleFor(x => x.SalaryMin)
                .GreaterThanOrEqualTo(0).When(x => x.SalaryMin.HasValue)
                .WithMessage("SalaryMin must be non-negative.");

            RuleFor(x => x.SalaryMax)
                .GreaterThanOrEqualTo(0).When(x => x.SalaryMax.HasValue)
                .WithMessage("SalaryMin must be non-negative.");

            RuleFor(x => x)
                .Must(x => !x.SalaryMax.HasValue || !x.SalaryMin.HasValue || x.SalaryMax >= x.SalaryMin)
                .WithMessage("SalaryMax must be greater than or equal to SalaryMin.");
        }
    }
}
