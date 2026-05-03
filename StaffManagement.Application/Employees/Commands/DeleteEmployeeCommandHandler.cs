using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Exceptions;
using StaffManagement.Application.Common.Interfaces;

namespace StaffManagement.Application.Employees.Commands
{
    public record DeleteEmployeeCommand(Guid Id) : IRequest<Unit>;
    public class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEmployeeCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees.FirstOrDefaultAsync(i => i.Id == command.Id, cancellationToken);

            if (employee == null)
                throw new NotFoundException($"Employee with Id {command.Id} not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangeAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
