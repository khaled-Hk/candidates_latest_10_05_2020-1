using System;
using System.Collections.Generic;

namespace Models
{
    public partial class CandidateUsers
    {
        public long CandidateUserId { get; set; }
        public string Name { get; set; }
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte[] Image { get; set; }
        public string Phone { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? LoginTryAttemptDate { get; set; }
        public short? LoginTryAttempts { get; set; }
        public DateTime? LastLoginOn { get; set; }
        public short? Gender { get; set; }
        public short? UserType { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public short? Status { get; set; }
        public long? CandidateId { get; set; }
        public long? EntityId { get; set; }
    }
}
