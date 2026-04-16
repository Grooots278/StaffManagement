using System;
using System.Collections.Generic;
using System.Text;

namespace StaffManagement.Application.Positions.DTOs
{
    public record PositionDto(
        Guid Id,
        string Title,
        decimal? SalaryMin,
        decimal? Salarymax,
        Guid DepartmentId,
        string DepartmentName
        );

    public record CreatePositionDto(
        string Title,
        decimal? SalaryMin,
        decimal? SalaryMax,
        Guid DepartmentId
        );

    public record UpdatePositionDto(
        string Title,
        decimal? SalaryMin,
        decimal? SalaryMax
        );
}
