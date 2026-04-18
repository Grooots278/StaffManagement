using FluentValidation;
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

        private readonly IServiceProvider _serviceProvider;

        public DepartmentController(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;

        [HttpPost]
        public async Task<ActionResult<Guid>> Create(CreateDepartmentCommand command, CancellationToken cancellationToken)
        {
            await ValidateAsync(command, cancellationToken);
            var handler = _serviceProvider.GetRequiredService<CreateDepartmentCommandHandler>();
            var id = await handler.Handle(command, cancellationToken);
            return Ok(id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateDepartmentDto dto, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<UpdateDepartmentCommandHandler>();
            var command = new UpdateDepartmentCommand(id, dto.Name, dto.Description);
            await handler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<DeleteDepartmentCommandHandler>();
            var command = new DeleteDepartmentCommand(id);
            await handler.Handle(command, cancellationToken);
            return NoContent();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentDto>> GetById(Guid id, CancellationToken cancellationToken)
        {
            var handler = _serviceProvider.GetRequiredService<GetDepartmentByIdQueryHandler>();
            var command = new GetDepartmentByIdQuery(id);
            var result = await handler.Handle(command, cancellationToken);
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
            var handler = _serviceProvider.GetRequiredService<GetDepartmentListQueryHandler>();
            var command = new GetDepartmentListQuery(searchTerm, pageNumber, pageSize);
            var result = await handler.Handle(command, cancellationToken);
            return Ok(result);
        }

        // Не очень чисто, но до подключения MediatR добавлена для тестов
        private async Task ValidateAsync<T>(T command, CancellationToken cancellationToken)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();
            if (validator != null)
            {
                var result = await validator.ValidateAsync(command, cancellationToken);
                if (!result.IsValid)
                    throw new ValidationException(result.Errors);
            }
        }
    }
}
