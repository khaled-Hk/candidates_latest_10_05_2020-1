using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Endorsements
    {
        public long EndorsementId { get; set; }
        public long? CandidateId { get; set; }
        public string Nid { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public virtual Candidates Candidate { get; set; }
    }
}
