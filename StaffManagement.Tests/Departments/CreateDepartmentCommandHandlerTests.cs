using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Departments.Commands;
using StaffManagement.Infrastructure.Data;
using Xunit;

namespace StaffManagement.Tests.Departments
{
    public class CreateDepartmentCommandHandlerTests
    {
        [Fact]
        public async Task Handle_ValidCommand_CreateDepartmentAndReturnsId()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var context = new AppDbContext(options);
            IApplicationDbContext dbContext = context;

            var handler = new CreateDepartmentCommandHandler(dbContext);
            var command = new CreateDepartmentCommand("IT", "Information technology");

            //Act
            var result = await handler.Handle(command, CancellationToken.None);

            //Assert
            result.Should().NotBeEmpty();
            var department = await context.Departments.FindAsync(result);
            department.Should().NotBeNull();
            department!.Name.Should().Be("IT");
        }
    }
}
