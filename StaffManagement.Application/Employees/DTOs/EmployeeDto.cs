namespace StaffManagement.Application.Employees.DTOs
{
    public record EmployeeDto(
        Guid Id,
        string FirstName,
        string LastName,
        string? MiddleName,
        string Email,
        string? Phone,
        DateTime HireDate,
        decimal Salary,
        bool IsActive,
        Guid DepartmentId,
        string DepartmentName,
        Guid PositionId,
        string PositionTitle,
        DateTime CreatedAt,
        DateTime? UpdatedAt
        );

    public record CreateEmployeeDto(
        string FirstName,
        string LastName,
        string Email,
        DateTime HireDate,
        decimal Salary,
        Guid DepartmentId,
        Guid PositionId,
        string? MiddleName = null,
        string? Phone = null,
        bool IsActive = true
        );

    public record UpdateEmployeeDto(
        string FirstName,
        string LastName,
        string Email,
        DateTime HireDate,
        decimal Salary,
        Guid DepartmentId,
        Guid PositionId,
        string? MiddleName = null,
        string? Phone = null,
        bool? IsActive = null
        );
}
