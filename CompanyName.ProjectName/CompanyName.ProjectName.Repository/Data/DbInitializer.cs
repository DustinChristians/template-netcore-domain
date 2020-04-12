using System;
using System.Linq;
using CompanyName.ProjectName.Repository.Entities;

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
                var users = new UserEntity[]
                {
                    new UserEntity
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
                    new UserEntity
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

                foreach (var user in users)
                {
                    context.Users.Add(user);
                }

                context.SaveChanges();
            }

            if (!context.Messages.Any())
            {
                var messages = new MessageEntity[]
                {
                    new MessageEntity
                    {
                        Text = "Hello, Bill!",
                        ChannelId = 1,
                        IsActive = true,
                        Guid = Guid.NewGuid(),
                        UserId = 1,
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now
                    },
                    new MessageEntity
                    {
                        Text = "Hi, Bob!",
                        ChannelId = 1,
                        IsActive = true,
                        Guid = Guid.NewGuid(),
                        UserId = 2,
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now,
                        ModifiedBy = 1,
                        ModifiedOn = DateTime.Now
                    },
                };

                foreach (var message in messages)
                {
                    context.Messages.Add(message);
                }

                context.SaveChanges();
            }
        }
    }
}
