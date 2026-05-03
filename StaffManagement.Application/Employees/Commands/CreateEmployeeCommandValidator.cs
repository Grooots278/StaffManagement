using FluentValidation;

namespace StaffManagement.Application.Employees.Commands
{
    public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
    {
        public CreateEmployeeCommandValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.MiddleName).MaximumLength(50);
            RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(100);
            RuleFor(x => x.Phone).MaximumLength(20).Matches(@"^[\d\s\+\-\(\)]+$").When(x => !string.IsNullOrEmpty(x.Phone));
            RuleFor(x => x.HireDate).LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Hire date cannot be in the future.");
            RuleFor(x => x.Salary).GreaterThanOrEqualTo(0);
            RuleFor(x => x.DepartmentId).NotEmpty();
            RuleFor(x => x.PositionId).NotEmpty();
        }
    }
}
