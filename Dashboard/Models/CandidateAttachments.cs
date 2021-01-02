using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class CandidateAttachments
    {
        public long CandidateAttachmentId { get; set; }
        public long? CandidateId { get; set; }
        public string BirthDateCertificate { get; set; }
        public string Nidcertificate { get; set; }
        public string FamilyPaper { get; set; }
        public string AbsenceOfPrecedents { get; set; }
        public string PaymentReceipt { get; set; }
        public short? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }

        public virtual Candidates Candidate { get; set; }
    }
}
