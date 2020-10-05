using System;
using System.Collections.Generic;

namespace Models
{
    public partial class ChairDetails
    {
        public ChairDetails()
        {
            ConstituencyDetailChairs = new HashSet<ConstituencyDetailChairs>();
        }

        public long ChairDetailId { get; set; }
        public int? GeneralChairs { get; set; }
        public int? PrivateChairs { get; set; }
        public long? ChairId { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public short? Status { get; set; }

        public virtual Chairs Chair { get; set; }
        public virtual ICollection<ConstituencyDetailChairs> ConstituencyDetailChairs { get; set; }
    }
}
