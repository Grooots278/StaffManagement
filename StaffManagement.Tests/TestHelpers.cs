using System.Reflection;

namespace StaffManagement.Tests
{
    public static class TestHelpers
    {
        public static void SetId<T>(T entity, Guid id)
        {
            var property = typeof(T).GetProperty("Id",
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (property == null)
                throw new InvalidOperationException($"Property 'Id' not found on {typeof(T).Name}");

            property.SetValue(entity, id);
        }
    }
}
