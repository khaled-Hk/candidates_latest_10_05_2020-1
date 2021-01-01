using System;
using System.Collections.Generic;

namespace Vue.Models
{
    public partial class EntityRepresentatives
    {
        public long EntityRepresentativeId { get; set; }
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
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public long? EntityId { get; set; }

        public virtual Entities Entity { get; set; }
    }
}
