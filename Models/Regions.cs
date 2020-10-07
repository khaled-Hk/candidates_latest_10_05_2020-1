using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Regions
    {
        public Regions()
        {
            Constituencies = new HashSet<Constituencies>();
            ConstituencyDetails = new HashSet<ConstituencyDetails>();
        }

        public long RegionId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? Status { get; set; }

        public virtual ICollection<Constituencies> Constituencies { get; set; }
        public virtual ICollection<ConstituencyDetails> ConstituencyDetails { get; set; }
    }
}