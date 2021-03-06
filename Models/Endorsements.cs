using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Endorsements
    {
        public long EndorsementId { get; set; }
        public long? CandidateId { get; set; }
        public string Nid { get; set; }
        public long? ProfileId { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual Candidates Candidate { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
