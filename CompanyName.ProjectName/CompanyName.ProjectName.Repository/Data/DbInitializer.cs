using System.Linq;
using CompanyName.ProjectName.Repository.Models;

namespace CompanyName.ProjectName.Repository.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CompanyNameProjectNameContext context)
        {
            context.Database.EnsureCreated();

            // Look for any messages.
            if (context.Messages.Any())
            {
                return; // DB has been seeded
            }

            // load test data into arrays rather than List<T> collections to optimize performance.
            var messages = new Message[]
            {
                new Message { Text = "Hello, World!" },
            };

            foreach (Message s in messages)
            {
                context.Messages.Add(s);
            }

            context.SaveChanges();
        }
    }
}
