using CompanyName.ProjectName.Core.Abstractions;
using CompanyName.ProjectName.Core.Models;

namespace CompanyName.ProjectName.Infrastructure.Services
{
    public class HelloWorldService : IHelloWorldService
    {
        public HelloWorldModel GetHelloWorldModel()
        {
            return new HelloWorldModel
            {
                Name = "Hello, World!"
            };
        }
    }
}
