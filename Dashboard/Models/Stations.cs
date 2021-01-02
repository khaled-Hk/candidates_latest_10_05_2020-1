using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Stations
    {
        public long StationId { get; set; }
        public string ArabicName { get; set; }
        public string EnglishName { get; set; }
        public string Description { get; set; }
        public long? CenterId { get; set; }
        public long? ProfileId { get; set; }
        public short? Status { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public virtual Centers Center { get; set; }
    }
}
