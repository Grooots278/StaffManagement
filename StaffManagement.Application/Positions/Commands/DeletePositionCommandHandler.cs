using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Exceptions;
using StaffManagement.Application.Common.Interfaces;

namespace StaffManagement.Application.Positions.Commands
{

    public record DeletePositionCommand(Guid Id) : IRequest<Unit>;

    public class DeletePositionCommandHandler : IRequestHandler<DeletePositionCommand, Unit>
    {

        private readonly IApplicationDbContext _context;

        public DeletePositionCommandHandler(IApplicationDbContext context) => _context = context;

        public async Task<Unit> Handle(DeletePositionCommand command, CancellationToken cancellationToken)
        {
            var position = await _context.Positions.FirstOrDefaultAsync(i => i.Id == command.Id);

            if (position == null)
                throw new NotFoundException($"Position with id {command.Id} not found.");

            _context.Positions.Remove(position);
            await _context.SaveChangeAsync(cancellationToken);
            return Unit.Value;
        }
    }
}
