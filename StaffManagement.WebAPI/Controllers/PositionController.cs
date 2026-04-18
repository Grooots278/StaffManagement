using Microsoft.AspNetCore.Mvc;
using StaffManagement.Application.Common;
using StaffManagement.Application.Positions.Commands;
using StaffManagement.Application.Positions.DTOs;
using StaffManagement.Application.Positions.Queries;

namespace StaffManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PositionController : ControllerBase
    {

        private readonly IServiceProvider _serviceProvider;

        public PositionController(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreatePositionCommand command, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<CreatePositionCommandHandler>();
            var id = await handler.Handle(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePositionDto dto, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<UpdatePositionCommandHandler>();
            var command = new UpdatePositionCommand(id, dto.Title, dto.SalaryMin, dto.SalaryMax);
            await handler.Handle(command, cancellationToken);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<DeletePositionCommandHandler>();
            var command = new DeletePositionCommand(id);
            await handler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PositionDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<GetPositionByIdQueryHandler>();
            var query = new GetPositionByIdQuery(id);
            var result = await handler.Handle(query, cancellationToken);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<PositionDto>>> GetList(
            [FromQuery] Guid? departmentId,
            [FromQuery] string? searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default
            )
        {
            var handler = _serviceProvider.GetRequiredService<GetPositionsListQueryHandler>();
            var query = new GetPositionsListQuery(departmentId, searchTerm, pageNumber, pageSize);
            var result = await handler.Handle(query, cancellationToken);
            return Ok(result);
        }
    }
}
