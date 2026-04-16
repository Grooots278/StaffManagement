namespace StaffManagement.Domain.Entities
{
    public class Position
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public decimal? MinSalary { get; private set; }
        public decimal? MaxSalary { get; private set; }

        public Guid DepartmentId { get; private set; }
        public Department Department { get; private set; } = null!;

        private Position() { }

        public Position(string title, Guid departmentId, decimal? salaryMin = null, decimal? maxSalary = null)
        {
            Id = Guid.NewGuid();
            Title = title;
            DepartmentId = departmentId;
            MinSalary = salaryMin;
            MaxSalary = maxSalary;
        }

        public void Update(string title, decimal? salaryMin = null, decimal? salaryMax = null)
        {
            Title = title;
            MinSalary = salaryMin;
            MaxSalary= salaryMax;
        }
    }
}
