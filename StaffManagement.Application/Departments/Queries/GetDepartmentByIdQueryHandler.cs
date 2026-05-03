using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Departments.DTOs;

namespace StaffManagement.Application.Departments.Queries
{
    public record GetDepartmentByIdQuery(Guid Id) : IRequest<DepartmentDto?>;

    public class GetDepartmentByIdQueryHandler : IRequestHandler<GetDepartmentByIdQuery, DepartmentDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetDepartmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DepartmentDto?> Handle(GetDepartmentByIdQuery query, CancellationToken cancellationToken)
        {
            var department = await _context.Departments
                .Include(d => d.Positions)
                .FirstOrDefaultAsync(i => i.Id == query.Id);

            return department == null ? null : _mapper.Map<DepartmentDto>(department);
        }
    }
}
