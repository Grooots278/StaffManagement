using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Positions.DTOs;

namespace StaffManagement.Application.Positions.Queries
{
    public record GetPositionsListQuery(Guid? DepartmentId,
        string? searchTerm, int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<PositionDto>>;

    public class GetPositionsListQueryHandler : IRequestHandler<GetPositionsListQuery, PaginatedList<PositionDto>>
    {

        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetPositionsListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

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
                .ToListAsync(cancellationToken);

            var dtos = _mapper.Map<List<PositionDto>>(item);

            return new PaginatedList<PositionDto>(dtos, totalCount, query.PageNumber, query.PageSize);
        }
    }
}
