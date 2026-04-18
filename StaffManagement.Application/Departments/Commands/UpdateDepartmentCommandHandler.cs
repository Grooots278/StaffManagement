using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Exceptions;
using StaffManagement.Application.Common.Interfaces;

namespace StaffManagement.Application.Departments.Commands
{

    public record UpdateDepartmentCommand(Guid Id, string Name, string? Description) : IRequest<Unit>;

    public class UpdateDepartmentCommandHandler : IRequestHandler<UpdateDepartmentCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public UpdateDepartmentCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(UpdateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == command.Id, cancellationToken);

            if (department == null)
                throw new NotFoundException($"Department with Id {command.Id} not found.");

            department.Update(command.Name, command.Description);
            await _context.SaveChangeAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
