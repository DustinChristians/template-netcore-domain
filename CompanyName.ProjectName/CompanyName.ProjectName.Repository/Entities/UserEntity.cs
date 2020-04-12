using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CompanyName.ProjectName.Repository.Entities
{
    public class UserEntity : BaseEntity
    {
        [Required]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public ICollection<MessageEntity> Messages { get; set; }
            = new List<MessageEntity>();
    }
}
