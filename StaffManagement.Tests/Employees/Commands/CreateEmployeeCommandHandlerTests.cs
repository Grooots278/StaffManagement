using FluentAssertions;
using Moq;
using StaffManagement.Application.Common.Interfaces;
using StaffManagement.Application.Employees.Commands;
using StaffManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq; 
using StaffManagement.Application.Common.Exceptions;
using Xunit;

namespace StaffManagement.Tests.Employees.Commands
{
    public class CreateEmployeeCommandHandlerTests
    {
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly CreateEmployeeCommandHandler _handler;

        public CreateEmployeeCommandHandlerTests(Mock<IApplicationDbContext> dbContextMock, CreateEmployeeCommandHandler handler)
        {
            _dbContextMock = dbContextMock;
            _handler = handler;
        }

        [Fact]
        public async Task Handle_ValidCommand_CreateEmployeeAndReturnsId()
        {
            var departmentId = Guid.NewGuid();
            var positionsId = Guid.NewGuid();

            var department = new Department("IT");
            TestHelpers.SetId(department, departmentId);

            var position = new Position("Developer", departmentId);
            TestHelpers.SetId(position, positionsId);

            var command = new CreateEmployeeCommand(
            "John", "Doe", "john@example.com", DateTime.UtcNow.AddDays(-10),
            50000m, departmentId, positionsId, null, null, true);

            var departments = new List<Department> { department }.BuildMockDbSet<Department>();
            var positions = new List<Position> { position }.BuildMockDbSet<Position>();
            var employees = new List<Employee>().BuildMockDbSet<Employee>();

            _dbContextMock.Setup(c => c.Departments).Returns(departments.Object);
            _dbContextMock.Setup(c => c.Positions).Returns(positions.Object);   
            _dbContextMock.Setup(c => c.Employees).Returns(employees.Object);
            _dbContextMock.Setup(c => c.SaveChangeAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeEmpty();
            employees.Object.Should().ContainSingle(e => e.Email == command.Email);
            _dbContextMock.Verify(c => c.SaveChangeAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DepartmentNotFound_ThrowsNotFoundException()
        {
            var command = new CreateEmployeeCommand(
                "John", "Doe", "john@example.com", DateTime.UtcNow, 50000m,
                Guid.NewGuid(), Guid.NewGuid(), null, null, true
                );

            var departments = new List<Department>().BuildMockDbSet<Department>();
            var positions = new List<Position>().BuildMockDbSet<Position>();
            var employees = new List<Employee>().BuildMockDbSet<Employee>();

            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage($"Department with id {command.DepartmentId} not found.");
        }
    }
}
