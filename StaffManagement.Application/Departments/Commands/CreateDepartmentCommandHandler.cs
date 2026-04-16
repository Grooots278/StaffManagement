using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Domain.Entities;

namespace StaffManagement.Application.Departments.Commands
{
    public class CreateDepartmentCommandHandler
    {
        private readonly IApplicationDbContext _context;

        public CreateDepartmentCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Guid> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var department = new Department(command.Name, command.Description);

            _context.Departments.Add(department);
            await _context.SaveChangeAsync(cancellationToken);

            return department.Id;
        }
    }
}
