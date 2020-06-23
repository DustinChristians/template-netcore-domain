using System;
using System.Linq;
using System.Threading;
using CompanyName.ProjectName.Repository.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CompanyName.ProjectName.Repository.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CompanyNameProjectNameContext context)
        {
            Semaphore semaphoreObject = new Semaphore(initialCount: 1, maximumCount: 1, name: "Database Initialization");

            // Only allow one startup project to create and seed the database if it doesn't exist.
            // All other projects will wait here until the first startup project is finished so they
            // don't move forward and try to access the database prematurely.
            semaphoreObject.WaitOne();
            InitializeDatabase(context);
            semaphoreObject.Release();
        }

        private static void InitializeDatabase(CompanyNameProjectNameContext context)
        {
            try
            {
                context.Database.Migrate();
            }
            catch (SqlException exception) when (exception.Number == 1801)
            {
                // exception.Number 1801 = The database already exists

                // It is preffered to avoid an excpetion but unfortunately Entity
                // Framework doesn't offer a method to do that. CanConnect() and
                // (context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists()
                // both return false even if the database already exists right after it is created.
                return;
            }

            System.Diagnostics.Debug.WriteLine("No Migrate Exception");

            if (context?.Users != null && !context.Users.Any())
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

            if (context?.Messages != null && !context.Messages.Any())
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

            System.Diagnostics.Debug.WriteLine("InitializeDatabase Complete");
        }
    }
}
