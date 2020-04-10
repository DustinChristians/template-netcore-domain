using System.ComponentModel.DataAnnotations;
using CompanyName.ProjectName.WebApi.Attributes.Validation;

namespace CompanyName.ProjectName.WebApi.Models
{
    public class MessageForCreation
    {
        [Required, Id]
        public int UserId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required, Id]
        public int ChannelId { get; set; }
    }
}
