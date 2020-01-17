using CompanyName.ProjectName.Core.Abstractions;
using CompanyName.ProjectName.Core.Models;

namespace CompanyName.ProjectName.Infrastructure.Services
{
    public class TestService : ITestService
    {
        public TestModel GetTestModel()
        {
            return new TestModel
            {
                Name = "Hello, World!"
            };
        }
    }
}
