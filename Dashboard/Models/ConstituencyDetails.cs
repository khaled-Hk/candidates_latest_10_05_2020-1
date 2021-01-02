using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class ConstituencyDetails
    {
        public ConstituencyDetails()
        {
            Centers = new HashSet<Centers>();
        }

        public long ConstituencyDetailId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? Status { get; set; }
        public long ConstituencyId { get; set; }
        public long? RegionId { get; set; }
        public long? ProfileId { get; set; }

        public virtual Constituencies Constituency { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Regions Region { get; set; }
        public virtual ICollection<Centers> Centers { get; set; }
    }
}
