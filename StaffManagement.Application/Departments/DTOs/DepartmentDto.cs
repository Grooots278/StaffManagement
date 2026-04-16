using System;
using System.Collections.Generic;
using System.Text;

namespace StaffManagement.Application.Departments.DTOs
{
    public record DepartmentDto(
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        DateTime? UpdatedAt,
        int PositionsCount
        );

    public record CreateDepartmentDto(
        string Name,
        string? Description
        );

    public record UpdateDepartmentDto(
        string Name,
        string? Description
        );
}
