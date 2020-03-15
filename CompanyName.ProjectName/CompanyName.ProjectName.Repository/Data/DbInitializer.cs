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

            if (!context.Users.Any())
            {
                // load test data into arrays rather than List<T> collections to optimize performance.
                var users = new User[]
                {
                    new User
                    {
                        Email = "bill.smith@test.com",
                        FirstName = "Bill",
                        LastName = "Smith",
                        IsActive = true,
                        Guid = Guid.NewGuid(),
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now
                    },
                    new User
                    {
                        Email = "bob.jones@test.com",
                        FirstName = "Bob",
                        LastName = "Jones",
                        IsActive = true,
                        Guid = Guid.NewGuid(),
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now
                    },
                };

                foreach (User u in users)
                {
                    context.Users.Add(u);
                }

                context.SaveChanges();
            }

            if (!context.Messages.Any())
            {
                var messages = new Message[]
                {
                    new Message
                    {
                        Text = "Hello, Bill!",
                        Category = "special",
                        IsActive = true,
                        Guid = Guid.NewGuid(),
                        UserId = 1,
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now
                    },
                    new Message
                    {
                        Text = "Hi, Bob!",
                        Category = "generic",
                        IsActive = true,
                        Guid = Guid.NewGuid(),
                        UserId = 2,
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = 1,
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
}
