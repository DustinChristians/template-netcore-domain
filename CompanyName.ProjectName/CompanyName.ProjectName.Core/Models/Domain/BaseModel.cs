using System;

namespace CompanyName.ProjectName.Core.Models.Domain
{
    public class BaseModel
    {
        public int Id { get; set; }
        public Guid Guid { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public int ModifiedBy { get; set; }
    }
}
