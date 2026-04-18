using MediatR;
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

        private readonly IMediator _mediator;

        public PositionController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreatePositionCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdatePositionDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdatePositionCommand(id, dto.Title, dto.SalaryMin, dto.SalaryMax);
            await _mediator.Send(command, cancellationToken);
            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeletePositionCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PositionDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var query = new GetPositionByIdQuery(id);
            var result = await _mediator.Send(query, cancellationToken);
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
            var query = new GetPositionsListQuery(departmentId, searchTerm, pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
