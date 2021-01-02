using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Constituencies
    {
        public Constituencies()
        {
            Chairs = new HashSet<Chairs>();
            ConstituencyDetailChairs = new HashSet<ConstituencyDetailChairs>();
            ConstituencyDetails = new HashSet<ConstituencyDetails>();
        }

        public long ConstituencyId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? Status { get; set; }
        public long? OfficeId { get; set; }
        public long? ProfileId { get; set; }
        public long? RegionId { get; set; }

        public virtual Offices Office { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual Regions Region { get; set; }
        public virtual ICollection<Chairs> Chairs { get; set; }
        public virtual ICollection<ConstituencyDetailChairs> ConstituencyDetailChairs { get; set; }
        public virtual ICollection<ConstituencyDetails> ConstituencyDetails { get; set; }
    }
}
