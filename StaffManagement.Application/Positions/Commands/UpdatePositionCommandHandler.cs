using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;

namespace StaffManagement.Application.Positions.Commands
{

    public record UpdatePositionCommand(Guid Id, string Title, decimal? SalaryMin, decimal? SalaryMax) : IRequest<Unit>;


    public class UpdatePositionCommandHandler
    {
        private readonly IApplicationDbContext _context;

        public UpdatePositionCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(UpdatePositionCommand command, CancellationToken cancellationToken)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(i => i.Id == command.Id);

            if (position == null)
                throw new Exception($"Position with id {command.Id} not found.");

            position.Update(command.Title, command.SalaryMin, command.SalaryMax);
            await _context.SaveChangeAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
