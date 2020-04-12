using System;

namespace CompanyName.ProjectName.WebApi.Models.User
{
    public class ReadUser
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
