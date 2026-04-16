namespace StaffManagement.Domain.Entities
{
    public class Department
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdateAt { get; private set; }

        //Navigation properties for Positions
        private readonly List<Position> _positions = new();
        public IReadOnlyCollection<Position> Positions => _positions.AsReadOnly();

        private Department() { }

        public Department(string name, string? description = null)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            CreatedAt = DateTime.UtcNow;
        }

        public void Update(string name, string? description = null)
        {
            Name = name;
            Description = description;
            UpdateAt = DateTime.UtcNow;
        }
    }
}
