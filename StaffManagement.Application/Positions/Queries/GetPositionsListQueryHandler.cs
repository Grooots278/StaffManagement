using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Positions.DTOs;

namespace StaffManagement.Application.Positions.Queries
{
    public record GetPositionsListQuery(Guid? DepartmentId,
        string? searchTerm, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<PositionDto>>;

    public class GetPositionsListQueryHandler
    {

        private readonly IApplicationDbContext _context;

        public GetPositionsListQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<PaginatedList<PositionDto>> Handle(GetPositionsListQuery query, CancellationToken cancellationToken)
        {
            var positionsQuery = _context.Positions
                .Include(d => d.Department)
                .AsNoTracking();

            if (query.DepartmentId.HasValue)
                positionsQuery = positionsQuery.Where(p => p.DepartmentId == query.DepartmentId);

            if (!string.IsNullOrEmpty(query.searchTerm))
                positionsQuery = positionsQuery.Where(p => p.Title.Contains(query.searchTerm));

            var totalCount = await positionsQuery.CountAsync(cancellationToken);

            var item = await positionsQuery
                .OrderBy(p => p.Title)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(x => new PositionDto(
                    x.Id, x.Title, x.MinSalary, x.MaxSalary, x.DepartmentId, x.Department.Name
                    )).ToListAsync();

            return new PaginatedList<PositionDto>(item, totalCount, query.PageNumber, query.PageSize);
        }
    }
}
