using CompanyName.ProjectName.Infrastructure.Services;
using NUnit.Framework;

namespace CompanyName.ProjectName.UnitTests
{
    [TestFixture]
    public class ServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var testService = new TestService();
            var result = testService.GetTestModel();

            Assert.IsTrue(string.Equals(result.Name, "Hello, World!"));
        }
    }
}