using System.ComponentModel.DataAnnotations;
using CompanyName.ProjectName.WebApi.Attributes.Validation;

namespace CompanyName.ProjectName.WebApi.Models.Message
{
    public class CreateMessage
    {
        [Required]
        [IdValidation]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        [IdValidation]
        public int ChannelId { get; set; }
    }
}
