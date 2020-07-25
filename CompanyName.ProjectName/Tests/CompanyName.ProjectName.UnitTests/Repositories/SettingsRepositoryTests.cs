using System.Threading.Tasks;
using CompanyName.ProjectName.Repository.Data;
using CompanyName.ProjectName.Repository.Repositories.Settings;
using CompanyName.ProjectName.TestUtilities;
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
        public async Task GetSettingValue_KeyNotExists_DefaultValue()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var defaultValue = "default value";

            using (var context = new CompanyNameProjectNameContext(options))
            {
                var settingsRepository = new SettingsRepository(context, MapperUtilities.GetTestMapper());

                // Act
                var value = await settingsRepository.GetSettingValue("TestKey", defaultValue);

                // Assert
                Assert.AreEqual(value, defaultValue);
            }
        }

        [Test]
        public async Task GetSettingValue_TestKeyString_AreEqual()
        {
            // Arrange
            var options = DatabaseUtilities.GetTestDbConextOptions<CompanyNameProjectNameContext>();

            var testSetting = new Repository.Entities.SettingEntity()
            {
                Key = "TestKey",
                Value = "TestValue",
                Type = typeof(string).ToString(),
                DisplayName = "Test Key",
                Description = "For Testing GetSettingValue"
            };

            using (var context = new CompanyNameProjectNameContext(options))
            {
                context.Settings.Add(testSetting);

                context.SaveChanges();
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
