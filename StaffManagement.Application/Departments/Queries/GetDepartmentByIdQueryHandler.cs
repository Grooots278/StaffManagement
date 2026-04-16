using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Departments.DTOs;
using StaffManagement.Domain.Entities;

namespace StaffManagement.Application.Departments.Queries
{
    public record GetDepartmentByIdQuery(Guid Id) : IRequest<DepartmentDto?>;

    public class GetDepartmentByIdQueryHandler
    {
        private readonly IApplicationDbContext _context;

        public GetDepartmentByIdQueryHandler(IApplicationDbContext context) => _context = context;

        public async Task<DepartmentDto?> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
        {
            var department = await _context.Departments
                .Include(d => d.Positions)
                .FirstOrDefaultAsync(i => i.Id == query.Id);

            if (department == null)
                return null;

            return new DepartmentDto(
                department.Id,
                department.Name,
                department.Description,
                department.CreatedAt,
                department.UpdateAt,
                department.Positions.Count);
        }
    }
}
