namespace StaffManagement.Domain.Entities
{
    public class Employee
    {
        public Guid Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string? MiddleName { get; private set; }
        public string Email { get; private set; }
        public string? Phone { get; private set; }
        public DateTime? HireDate { get; private set; }
        public decimal Salary { get; private set; }
        public bool IsActive { get; private set; }

        //Внешние ключи
        public Guid DepartmentId { get; private set; }
        public Guid PositionId { get; private set; }


        //Навигационные свойства
        public Department Department { get; private set; } = null!;
        public Position Position { get; private set; } = null!;

        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        private Employee() { }

        public Employee(
            string firstName,
            string lastName,
            string email,
            DateTime hireDate,
            decimal salary,
            Guid departmentId,
            Guid positionId,
            string? middleName = null,
            string? phone = null,
            bool isActive = true
            )
        {
            Id = Guid.NewGuid();
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            HireDate = hireDate;
            Salary = salary;
            DepartmentId = departmentId;
            PositionId = positionId;
            MiddleName = middleName;
            Phone = phone;
            IsActive = isActive;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(
            string firstName,
            string lastName,
            string email,
            DateTime hireDate,
            decimal salary,
            Guid departmentId,
            Guid positionId,
            string? middleName = null,
            string? phone = null,
            bool? isActive = null
            )
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Email = email;
            Phone = phone;
            HireDate = hireDate;
            Salary = salary;
            DepartmentId = departmentId;
            PositionId = positionId;
            if (isActive.HasValue)
                IsActive = isActive.Value;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
