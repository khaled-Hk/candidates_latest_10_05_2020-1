using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class ConstituencyDetailChairs
    {
        public long ConstituencyDetailChairId { get; set; }
        public long? ConstituencyDetailId { get; set; }
        public long? ChairDetailId { get; set; }

        public virtual ChairDetails ChairDetail { get; set; }
        public virtual Constituencies ConstituencyDetail { get; set; }
    }
}
