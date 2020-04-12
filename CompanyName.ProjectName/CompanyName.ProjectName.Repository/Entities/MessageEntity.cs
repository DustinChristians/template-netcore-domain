namespace CompanyName.ProjectName.Repository.Entities
{
    public class MessageEntity : BaseEntity
    {
        public string Text { get; set; }
        public int ChannelId { get; set; }
        public int UserId { get; set; }
        public UserEntity User { get; set; }
    }
}
