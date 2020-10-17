using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Profile
    {
        public Profile()
        {
            Branches = new HashSet<Branches>();
            Centers = new HashSet<Centers>();
            Constituencies = new HashSet<Constituencies>();
            ConstituencyDetails = new HashSet<ConstituencyDetails>();
            Offices = new HashSet<Offices>();
        }

        public long ProfileId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public short? ProfileType { get; set; }
        public short? IsActivate { get; set; }
        public short? Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Branches> Branches { get; set; }
        public virtual ICollection<Centers> Centers { get; set; }
        public virtual ICollection<Constituencies> Constituencies { get; set; }
        public virtual ICollection<ConstituencyDetails> ConstituencyDetails { get; set; }
        public virtual ICollection<Offices> Offices { get; set; }
    }
}
