using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Exceptions;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Domain.Entities;

namespace StaffManagement.Application.Employees.Commands
{
    public record CreateEmployeeCommand(
        string FirstName,
        string LastName,
        string Email,
        DateTime HireDate,
        decimal Salary,
        Guid DepartmentId,
        Guid PositionId,
        string? MiddleName,
        string? Phone,
        bool IsActive
        ) : IRequest<Guid>;

    public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Guid>
    {

        private readonly IApplicationDbContext _context;

        public CreateEmployeeCommandHandler( IApplicationDbContext context ) => _context = context;

        public async Task<Guid> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var departmentExists = await _context.Departments.AnyAsync(d => d.Id == command.DepartmentId, cancellationToken);

            if (!departmentExists)
                throw new NotFoundException($"Department with Id {command.DepartmentId} not found.");

            var positionExists = await _context.Positions.AnyAsync(p => p.Id == command.PositionId, cancellationToken);

            if (!positionExists)
                throw new NotFoundException($"Position with Id {command.PositionId} not found.");

            var emailExists = await _context.Employees.AnyAsync(e => e.Email == command.Email, cancellationToken);

            if (emailExists)
                throw new ValidationException(
                    new List<FluentValidation.Results.ValidationFailure>
                    {
                        new ("Email", "Employee with this email already exists.")
                    });

            var employee = new Employee(
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

            _context.Employees.Add(employee);
            await _context.SaveChangeAsync(cancellationToken);

            return employee.Id;
        }
    }
}
