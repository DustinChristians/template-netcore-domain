using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Domain;
using CompanyName.ProjectName.Core.Models.ResourceParameters;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Repositories;
using CompanyName.ProjectName.TestUtilities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CompanyName.ProjectName.UnitTests.Repositories
{
    [TestFixture]
    public class UserRepositoryTests
    {
        public static IEnumerable<TestCaseData> NormalSearchTestCases
        {
            get
            {
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = "test1@test.com",
                    FirstName = null,
                    LastName = null,
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = "test1",
                    LastName = null,
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = "user1",
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = null,
                    SearchQuery = "test1"
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = "Test1@test.com",
                    FirstName = null,
                    LastName = null,
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = "Test1",
                    LastName = null,
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = "User1",
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = null,
                    SearchQuery = "Test1"
                });
            }
        }

        public static IEnumerable<TestCaseData> NotFoundSearchTestCases
        {
            get
            {
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = "NotFound",
                    FirstName = null,
                    LastName = null,
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = "NotFound",
                    LastName = null,
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = "NotFound",
                    SearchQuery = null
                });
                yield return new TestCaseData(new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = null,
                    SearchQuery = "NotFound"
                });
            }
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetUsersAsync_NullParameters_ReturnsArgumentNullException()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());

                // Assert
                Assert.ThrowsAsync<ArgumentNullException>(async () => await usersRepository.GetUsersAsync(null));
            }
        }

        [Test]
        public async Task GetUsersAsync_AllPropertiesNull_ReturnsAllUsers()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var user1 = new User()
            {
                Email = "test1@test.com",
                FirstName = "Test1",
                LastName = "User1"
            };

            var user2 = new User()
            {
                Email = "test2@test.com",
                FirstName = "Test2",
                LastName = "User2"
            };

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the Users
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user1);
                await usersRepository.CreateAsync(user2);
            }

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var parameters = new UsersResourceParameters
                {
                    Email = null,
                    FirstName = null,
                    LastName = null,
                    SearchQuery = null
                };

                // Get Users with null properties for parameters
                var results = await usersRepository.GetUsersAsync(parameters);

                // Assert
                Assert.AreEqual(results.Count(), 2);
                Assert.IsTrue(results.FirstOrDefault(x => x.Email == user1.Email) != null);
                Assert.IsTrue(results.FirstOrDefault(x => x.Email == user2.Email) != null);
            }
        }

        [TestCaseSource(nameof(NormalSearchTestCases))]
        public async Task GetUsersAsync_Search_ReturnsUser(UsersResourceParameters usersResourceParameters)
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var user1 = new User()
            {
                Email = "test1@test.com",
                FirstName = "Test1",
                LastName = "User1"
            };

            var user2 = new User()
            {
                Email = "test2@test.com",
                FirstName = "Test2",
                LastName = "User2"
            };

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the Users
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user1);
                await usersRepository.CreateAsync(user2);
            }

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var results = await usersRepository.GetUsersAsync(usersResourceParameters);

                // Assert
                Assert.AreEqual(results.Count(), 1);
                Assert.IsTrue(results.FirstOrDefault(x => x.Email == user1.Email) != null);
            }
        }

        [TestCaseSource(nameof(NotFoundSearchTestCases))]
        public async Task GetUsersAsync_SearchQueryDoesntExist_ReturnsNull(UsersResourceParameters usersResourceParameters)
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var user1 = new User()
            {
                Email = "test1@test.com",
                FirstName = "Test1",
                LastName = "User1"
            };

            var user2 = new User()
            {
                Email = "test2@test.com",
                FirstName = "Test2",
                LastName = "User2"
            };

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                // Add a user because we need a UserId foreign key for the Users
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());
                await usersRepository.CreateAsync(user1);
                await usersRepository.CreateAsync(user2);
            }

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var usersRepository = new UsersRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var results = await usersRepository.GetUsersAsync(usersResourceParameters);

                // Assert
                Assert.AreEqual(results.Count(), 0);
            }
        }
    }
}