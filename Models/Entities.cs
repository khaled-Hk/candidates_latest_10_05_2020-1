using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Entities
    {
        public long EntityId { get; set; }
        public string Name { get; set; }
        public long? Number { get; set; }
        public string Descriptions { get; set; }
        public string Phone { get; set; }
        public byte[] Logo { get; set; }
        public string Owner { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public long? ProfileId { get; set; }
        public short? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
    }
}
