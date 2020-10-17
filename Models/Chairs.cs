using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Chairs
    {
        public Chairs()
        {
            ChairDetails = new HashSet<ChairDetails>();
        }

        public long ChairId { get; set; }
        public int? GeneralChairs { get; set; }
        public int? PrivateChairs { get; set; }
        public long? RelativeChairs { get; set; }
        public int? GeneralChairRemaining { get; set; }
        public int? PrivateChairRemaining { get; set; }
        public long? ConstituencyId { get; set; }
        public short? Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Constituencies Constituency { get; set; }
        public virtual ICollection<ChairDetails> ChairDetails { get; set; }
    }
}
