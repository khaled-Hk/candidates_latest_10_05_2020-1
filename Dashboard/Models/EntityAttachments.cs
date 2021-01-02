using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class EntityAttachments
    {
        public long EntityAttachmentId { get; set; }
        public string NameHeadEntity { get; set; }
        public string LegalAgreementPoliticalEntity { get; set; }
        public string PoliticalEntitySymbol { get; set; }
        public string CampaignAccountNumber { get; set; }
        public short? Status { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
    }
}
