using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Domain.Entities;

namespace StaffManagement.Infrastructure.Data
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Department> Departments => Set<Department>();
        public DbSet<Position> Positions => Set<Position>();

        public Task<int> SaveChangeAsync(CancellationToken cancellationToken)
        {
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>(entity =>
            {
                entity.HasKey(i => i.Id);

                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.HasIndex(e => e.Name).IsUnique();
            });

            modelBuilder.Entity<Position>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Department)
                .WithMany(d => d.Positions)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(p => new { p.Title, p.DepartmentId }).IsUnique();
            });
        }

    }
}
