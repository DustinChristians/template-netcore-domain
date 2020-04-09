using System;

namespace CompanyName.ProjectName.WebApi.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public int ChannelId { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
