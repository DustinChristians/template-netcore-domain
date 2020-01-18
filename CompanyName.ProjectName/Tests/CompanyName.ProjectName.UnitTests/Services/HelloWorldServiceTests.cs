using CompanyName.ProjectName.Infrastructure.Services;
using NUnit.Framework;

namespace CompanyName.ProjectName.UnitTests
{
    [TestFixture]
    public class HelloWorldServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var helloWorldService = new HelloWorldService();
            var result = helloWorldService.GetHelloWorldModel();

            Assert.IsTrue(string.Equals(result.Name, "Hello, World!"));
        }
    }
}