using System;
using System.Collections.Generic;

namespace Models.Models
{
    public partial class Branches
    {
        public Branches()
        {
            Offices = new HashSet<Offices>();
        }

        public long BrancheId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? Status { get; set; }
        public long? ProfileId { get; set; }

        public virtual Profile Profile { get; set; }
        public virtual ICollection<Offices> Offices { get; set; }
    }
}
