using CompanyName.ProjectName.Core.Models;

namespace CompanyName.ProjectName.Infrastructure.Services
{
    public class TestService
    {
        public TestModel GetTestModel()
        {
            return new TestModel
            {
                Name = "Test"
            };
        }
    }
}
