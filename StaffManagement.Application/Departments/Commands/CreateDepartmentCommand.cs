using MediatR;

namespace StaffManagement.Application.Departments.Commands
{
    public record CreateDepartmentCommand(string Name, string? Description) : IRequest<Guid>;
}
