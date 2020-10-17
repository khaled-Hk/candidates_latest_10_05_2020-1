using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Centers
    {
        public Centers()
        {
            Stations = new HashSet<Stations>();
        }

        public long CenterId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public long? OfficeId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public long? ProfileId { get; set; }
        public long? ConstituencDetailId { get; set; }
        public short? Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual ConstituencyDetails ConstituencDetail { get; set; }
        public virtual Offices Office { get; set; }
        public virtual Profile Profile { get; set; }
        public virtual ICollection<Stations> Stations { get; set; }
    }
}
