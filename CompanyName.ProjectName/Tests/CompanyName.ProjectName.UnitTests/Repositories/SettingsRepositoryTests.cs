using System.Threading.Tasks;
using CompanyName.ProjectName.Core.Models.Domain;
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

        [TestCase("")]
        [TestCase("KeyThatDoesNotExist")]
        [TestCase(null)]
        public async Task GetSettingValue_ReturnsDefaultValue(string key)
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var defaultValue = "default value";

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var value = await settingsRepository.GetSettingValue(key, defaultValue);

                // Assert
                Assert.AreEqual(value, defaultValue);
            }
        }

        [Test]
        public async Task GetSettingValue_TestKeyString_ReturnsKeyValue()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var testSetting = new Setting()
            {
                Key = "TestKey",
                Value = "TestValue",
                Type = typeof(string).ToString(),
                DisplayName = "Test Key",
                Description = "For Testing GetSettingValue"
            };

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());
                await settingsRepository.CreateAsync(testSetting);
            }

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var value = await settingsRepository.GetSettingValue("TestKey", string.Empty);

                // Assert
                Assert.AreEqual(value, testSetting.Value);
            }
        }

        [TestCase("")]
        [TestCase("KeyThatDoesNotExist")]
        [TestCase(null)]
        public async Task TryGetSettingValue_ReturnsNullSuccessfulFalse(string key)
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var result = await settingsRepository.TryGetSettingValue<string>(key);

                // Assert
                Assert.AreEqual(result.Value, null);
                Assert.AreEqual(result.Successful, false);
            }
        }

        [Test]
        public async Task TryGetSettingValue_TestKeyString_ReturnsKeyValue()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var testSetting = new Setting()
            {
                Key = "TestKey",
                Value = "TestValue",
                Type = typeof(string).ToString(),
                DisplayName = "Test Key",
                Description = "For Testing GetSettingValue"
            };

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Database.OpenConnection();
                context.Database.EnsureCreated();

                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());
                await settingsRepository.CreateAsync(testSetting);
            }

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var result = await settingsRepository.TryGetSettingValue<string>("TestKey");

                // Assert
                Assert.AreEqual(result.Value, testSetting.Value);
                Assert.AreEqual(result.Successful, true);
            }
        }

        [Test]
        public void TryUpdateSettingValue()
        {
            Assert.Pass();
        }
    }
}
