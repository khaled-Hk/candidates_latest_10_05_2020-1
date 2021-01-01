using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Offices
    {
        public Offices()
        {
            Centers = new HashSet<Centers>();
            Constituencies = new HashSet<Constituencies>();
        }

        public long OfficeId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public long? BranchId { get; set; }
        public long? ProfileId { get; set; }
        public short? Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Branches Branch { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Centers> Centers { get; set; }
        public virtual ICollection<Constituencies> Constituencies { get; set; }
    }
}
