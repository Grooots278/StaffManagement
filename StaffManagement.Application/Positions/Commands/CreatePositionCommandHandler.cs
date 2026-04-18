using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Domain.Entities;
using MediatR;
namespace StaffManagement.Application.Positions.Commands
{
    public record CreatePositionCommand(string Title, decimal? SalaryMin, decimal? SalaryMax, Guid DepartmentId) : IRequest<Guid>;

    public class CreatePositionCommandHandler : IRequestHandler<CreatePositionCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreatePositionCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Guid> Handle(CreatePositionCommand command, CancellationToken cancellationToken)
        {
            var department = await _context.Departments
                .AnyAsync(d => d.Id == command.DepartmentId, cancellationToken);

            if (!department)
                throw new Exception($"Department with Id {command.DepartmentId} not found");

            var position = new Position(
                command.Title,
                command.DepartmentId,
                command.SalaryMin,
                command.SalaryMax);

            _context.Positions.Add(position);
            await _context.SaveChangeAsync(cancellationToken);

            return position.Id;
        }
    }
}
