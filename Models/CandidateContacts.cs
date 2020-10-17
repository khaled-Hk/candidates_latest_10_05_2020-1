using System;
using System.Collections.Generic;

namespace Models
{
    public partial class CandidateContacts
    {
        public long CandidateContactId { get; set; }
        public string Object { get; set; }
        public short? ObjectType { get; set; }
        public long? CandidateId { get; set; }
        public short? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
    }
}
