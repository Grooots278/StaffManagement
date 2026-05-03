using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Employees.DTOs;

namespace StaffManagement.Application.Employees.Queries
{
    public record GetEmployeeListQuery(
        Guid? DepartmentId,
        Guid? PositionId,
        string? SearchTerm,
        bool? IsActive,
        int PageNumber = 1,
        int PageSize = 10) : IRequest<PaginatedList<EmployeeDto>>;

    public class GetEmployeeListQueryHandler : IRequestHandler<GetEmployeeListQuery, PaginatedList<EmployeeDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginatedList<EmployeeDto>> Handle(GetEmployeeListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .AsNoTracking();

            if (request.DepartmentId.HasValue)
                query = query.Where(e => e.DepartmentId == request.DepartmentId.Value);
            if (request.PositionId.HasValue)
                query = query.Where(e => e.PositionId == request.PositionId.Value);
            if (request.IsActive.HasValue)
                query = query.Where(e => e.IsActive == request.IsActive.Value);

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.ToLower();
                query.Where(e => 
                e.FirstName.ToLower().Contains(term) ||
                e.LastName.ToLower().Contains(term) || 
                (e.MiddleName != null && e.MiddleName.ToLower().Contains(term)) || 
                e.Email.ToLower().Contains(term));
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderBy(e => e.LastName)
                .ThenBy(e => e.FirstName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<EmployeeDto>>(items);
            return new PaginatedList<EmployeeDto>(dtos, totalCount, request.PageNumber, request.PageSize);
        }
    }
}
