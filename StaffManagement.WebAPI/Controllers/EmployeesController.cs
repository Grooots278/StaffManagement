using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;
using StaffManagement.Application.Common;
using StaffManagement.Application.Employees.Commands;
using StaffManagement.Application.Employees.DTOs;
using StaffManagement.Application.Employees.Queries;

namespace StaffManagement.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmployeesController(IMediator mediator) => _mediator = mediator;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateEmployeeCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid Id, UpdateEmployeeDto dto, CancellationToken cancellationToken)
        {
            var command = new UpdateEmployeeCommand(
                Id,
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.HireDate,
                dto.Salary,
                dto.DepartmentId,
                dto.PositionId,
                dto.MiddleName,
                dto.Phone,
                dto.IsActive
                );

            await _mediator.Send(command,cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid Id, CancellationToken cancellationToken)
        {
            await _mediator.Send(new DeleteEmployeeCommand(Id), cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEmployeeByIdQuery(id), cancellationToken);
            return result == null ? NoContent() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<EmployeeDto>>> GetList(
            [FromQuery] Guid? departmentId,
            [FromQuery] Guid? positionId,
            [FromQuery] string? searchTerm,
            [FromQuery] bool? isActive,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken cancellationToken = default
            )
        {
            var query = new GetEmployeeListQuery(departmentId, positionId, searchTerm, isActive, pageNumber, pageSize);
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
