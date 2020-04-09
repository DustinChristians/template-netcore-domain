namespace CompanyName.ProjectName.Core.Models.Repositories
{
    public class Message : BaseModel
    {
        public string Text { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
    }
}
