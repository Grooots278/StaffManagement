using AutoMapper;
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
        private readonly IMapper _mapper;

        public GetPositionByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PositionDto?> Handle(GetPositionByIdQuery query, CancellationToken cancellationToken)
        {
            var position = await _context.Positions
                .Include(d => d.Department)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == query.Id, cancellationToken);

            return position == null ? null : _mapper.Map<PositionDto>( position );  
        }
    }
}
