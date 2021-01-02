using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class Candidates
    {
        public Candidates()
        {
            CandidateAttachments = new HashSet<CandidateAttachments>();
            CandidateRepresentatives = new HashSet<CandidateRepresentatives>();
            Endorsements = new HashSet<Endorsements>();
        }

        public long CandidateId { get; set; }
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandFatherName { get; set; }
        public string SurName { get; set; }
        public string Nid { get; set; }
        public string MotherName { get; set; }
        public short? Gender { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; }
        public string HomePhone { get; set; }
        public string Email { get; set; }
        public string Qualification { get; set; }
        public long? ProfileId { get; set; }
        public long? OfficeId { get; set; }
        public long? ConstituencyId { get; set; }
        public long? SubConstituencyId { get; set; }
        public short? Levels { get; set; }
        public short? CompetitionType { get; set; }
        public long? EntityId { get; set; }
        public short? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }

        public virtual ICollection<CandidateAttachments> CandidateAttachments { get; set; }
        public virtual ICollection<CandidateRepresentatives> CandidateRepresentatives { get; set; }
        public virtual ICollection<Endorsements> Endorsements { get; set; }
    }
}
