using MediatR;
using Microsoft.AspNetCore.Mvc;
using StaffManagement.Application.Common;
using StaffManagement.Application.Departments.Commands;
using StaffManagement.Application.Departments.DTOs;
using StaffManagement.Application.Departments.Queries;

namespace StaffManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {

        private readonly IMediator _mediator;

        public DepartmentController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDepartmentDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateDepartmentCommand(id, dto.Name, dto.Description);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var command = new DeleteDepartmentCommand(id);
            await _mediator.Send(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var command = new GetDepartmentByIdQuery(id);
            var result = await _mediator.Send(command, cancellationToken);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<DepartmentDto>>> GetList(
            [FromQuery] string? searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default
            )
        {
            var command = new GetDepartmentListQuery(searchTerm, pageNumber, pageSize);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
