using System.ComponentModel.DataAnnotations;

namespace CompanyName.ProjectName.WebApi.Models
{
    public class MessageForCreation
    {
        [Required]
        public int userId { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public int ChannelId { get; set; }
    }
}
