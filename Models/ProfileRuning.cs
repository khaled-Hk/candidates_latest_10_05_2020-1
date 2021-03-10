using System;
using System.Collections.Generic;

namespace Models
{
    public partial class ProfileRuning
    {
        public long ProfileRuningId { get; set; }
        public long? UserId { get; set; }
        public long? ProfileId { get; set; }
    }
}
