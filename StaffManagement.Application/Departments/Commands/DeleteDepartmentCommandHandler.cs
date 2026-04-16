using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;

namespace StaffManagement.Application.Departments.Commands
{

    public record DeleteDepartmentCommand(Guid Id) : IRequest<Unit>;

    public class DeleteDepartmentCommandHandler
    {
        private readonly IApplicationDbContext _context;

        public DeleteDepartmentCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(DeleteDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == command.Id, cancellationToken);

            if (department == null)
                throw new Exception($"Department with Id {command.Id} not found.");

            _context.Departments.Remove(department);
            await _context.SaveChangeAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
