using System.Threading.Tasks;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Repositories.Settings;
using CompanyName.ProjectName.TestUtilities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace CompanyName.ProjectName.UnitTests.Repositories
{
    [TestFixture]
    public class SettingsRepositoryTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetSettingValue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<CompanyNameProjectNameContext>()
                .UseInMemoryDatabase("CompanyNameProjectNameDatabaseForTesting")
                .Options;

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var testSetting = new Repository.Entities.SettingEntity()
                {
                    Key = "TestKey",
                    Value = "TestValue",
                    Type = typeof(string).ToString(),
                    DisplayName = "Test Key",
                    Description = "For Testing GetSettingValue"
                };

                context.Settings.Add(testSetting);

                context.SaveChanges();

                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var value = await settingsRepository.GetSettingValue("TestKey", string.Empty);

                // Assert
                Assert.AreEqual(value, testSetting.Value);
            }
        }

        [Test]
        public void TryGetSettingValue()
        {
            Assert.Pass();
        }

        [Test]
        public void TryUpdateSettingValue()
        {
            Assert.Pass();
        }
    }
}
