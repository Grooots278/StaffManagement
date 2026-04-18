using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Departments.DTOs;

namespace StaffManagement.Application.Departments.Queries
{
    public record GetDepartmentListQuery(string? SearchTerm, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<DepartmentDto>>;
    public class GetDepartmentListQueryHandler : IRequestHandler<GetDepartmentListQuery, PaginatedList<DepartmentDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetDepartmentListQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<PaginatedList<DepartmentDto>> Handle(GetDepartmentListQuery query, CancellationToken cancellationToken)
        {
            var departmentsQuery = _context.Departments.Include(d => d.Positions).AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.SearchTerm))
            {
                departmentsQuery = departmentsQuery.Where(d => d.Name.Contains(query.SearchTerm) ||
                    (d.Description != null && d.Description.Contains(query.SearchTerm)));
            }

            var totalCount = await departmentsQuery.CountAsync(cancellationToken);

            var items = await departmentsQuery
                .OrderBy(d => d.Name)
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(d => new DepartmentDto
                (
                    d.Id,
                    d.Name,
                    d.Description,
                    d.CreatedAt,
                    d.UpdateAt,
                    d.Positions.Count
                    )).ToListAsync(cancellationToken);

            return new PaginatedList<DepartmentDto>(items, totalCount, query.PageNumber, query.PageSize);
        }
    }
}
