using Microsoft.EntityFrameworkCore;
using StaffManagement.Domain.Entities;

namespace StaffManagement.Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Department> Departments { get; }
        DbSet<Position> Positions { get; }
        DbSet<Employee> Employees { get; }

        Task<int> SaveChangeAsync(CancellationToken cancellationToken);
    }
}
