using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Employees.DTOs;

namespace StaffManagement.Application.Employees.Queries
{
    public record GetEmployeeByIdQuery(Guid Id) : IRequest<EmployeeDto?>;
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, EmployeeDto?>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetEmployeeByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeDto?> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken)
        {
            var employee = await _context.Employees
                .Include(e => e.Department)
                .Include(e => e.Position)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken);

            return employee == null ? null : _mapper.Map<EmployeeDto>(employee);
        }

    }
}
