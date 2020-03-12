using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyName.ProjectName.Core.Models.Repositories
{
    public class User : BaseModel
    {
        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<Message> Messages { get; set; }
            = new List<Message>();
    }
}
