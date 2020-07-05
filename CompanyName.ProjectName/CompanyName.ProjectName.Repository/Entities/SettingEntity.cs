namespace CompanyName.ProjectName.Repository.Entities
{
    public class SettingEntity : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
