using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Exceptions;
using StaffManagement.Application.Common.Interfaces;

namespace StaffManagement.Application.Employees.Commands
{
    public record UpdateEmployeeCommand(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime HireDate,
        decimal Salary,
        Guid DepartmentId,
        Guid PositionId,
        string? MiddleName,
        string? Phone,
        bool? IsActive
        ) : IRequest<Unit>;
    public class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateEmployeeCommandHandler( IApplicationDbContext context ) => _context = context;

        public async Task<Unit> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

            if (employee == null)
                throw new NotFoundException($"Employee with Id {command.Id} not found.");

            var departmentExists = await _context.Departments.AnyAsync(i => i.Id == command.DepartmentId, cancellationToken);

            if (!departmentExists)
                throw new NotFoundException($"Department with Id {command.DepartmentId} not found.");

            var positionExists = await _context.Positions.AnyAsync(i => i.Id == command.PositionId, cancellationToken);

            if (!positionExists)
                throw new NotFoundException($"Position with Id {command.PositionId} not found.");

            var emailExist = await _context.Employees.AnyAsync(i => i.Email == command.Email && i.Id != command.Id, cancellationToken);
            if (emailExist)
                throw new ValidationException(
                    new List<FluentValidation.Results.ValidationFailure>
                    {
                        new("Email", "Another employee with this email already exists.")
                    });

            employee.Update(
                command.FirstName,
                command.LastName,
                command.Email,
                command.HireDate,
                command.Salary,
                command.DepartmentId,
                command.PositionId,
                command.MiddleName,
                command.Phone,
                command.IsActive
                );

            await _context.SaveChangeAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
