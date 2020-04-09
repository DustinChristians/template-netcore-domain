using System.ComponentModel.DataAnnotations;

namespace CompanyName.ProjectName.WebApi.Models
{
    public class UserForCreation
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
