using System;
using System.Linq;
using CompanyName.ProjectName.Core.Models.Repositories;

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
                new Message
                {
                    Text = "Hello, World!",
                    Category = "special",
                    Guid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
                new Message
                {
                    Text = "This is a sample application.",
                    Category = "generic",
                    Guid = Guid.NewGuid(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now
                },
            };

            foreach (Message s in messages)
            {
                context.Messages.Add(s);
            }

            context.SaveChanges();
        }
    }
}
