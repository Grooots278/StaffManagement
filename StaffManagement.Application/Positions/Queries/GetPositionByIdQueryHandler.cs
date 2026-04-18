using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Positions.DTOs;

namespace StaffManagement.Application.Positions.Queries
{

    public record GetPositionByIdQuery(Guid Id) : IRequest<PositionDto?>;

    public class GetPositionByIdQueryHandler : IRequestHandler<GetPositionByIdQuery, PositionDto?>
    {

        private readonly IApplicationDbContext _context;

        public GetPositionByIdQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<PositionDto?> Handle(GetPositionByIdQuery query, CancellationToken cancellationToken)
        {
            var position = await _context.Positions
                .Include(d => d.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == query.Id);

            if (position == null) return null;

            return new PositionDto
            (
                position.Id, position.Title, position.MinSalary, position.MaxSalary, position.DepartmentId, position.Department.Name
            );
        }
    }
}
