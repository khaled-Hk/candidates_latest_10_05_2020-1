using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
    public partial class Candidates2Context : DbContext
    {
        public Candidates2Context()
        {
        }

        public Candidates2Context(DbContextOptions<Candidates2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<CandidateAttachments> CandidateAttachments { get; set; }
        public virtual DbSet<CandidateContacts> CandidateContacts { get; set; }
        public virtual DbSet<CandidateRepresentatives> CandidateRepresentatives { get; set; }
        public virtual DbSet<CandidateUsers> CandidateUsers { get; set; }
        public virtual DbSet<Candidates> Candidates { get; set; }
        public virtual DbSet<Centers> Centers { get; set; }
        public virtual DbSet<ChairDetails> ChairDetails { get; set; }
        public virtual DbSet<Chairs> Chairs { get; set; }
        public virtual DbSet<Constituencies> Constituencies { get; set; }
        public virtual DbSet<ConstituencyDetailChairs> ConstituencyDetailChairs { get; set; }
        public virtual DbSet<ConstituencyDetails> ConstituencyDetails { get; set; }
        public virtual DbSet<Endorsements> Endorsements { get; set; }
        public virtual DbSet<Entities> Entities { get; set; }
        public virtual DbSet<EntityAttachments> EntityAttachments { get; set; }
        public virtual DbSet<EntityRepresentatives> EntityRepresentatives { get; set; }
        public virtual DbSet<EntityUsers> EntityUsers { get; set; }
        public virtual DbSet<Offices> Offices { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<Stations> Stations { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-4AI87L8\\SQLEXPRESS;Database=Candidates2;Trusted_Connection=True;;MultipleActiveResultSets=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branches>(entity =>
            {
                entity.HasKey(e => e.BrancheId);

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Branches)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Branches_Profile");
            });

            modelBuilder.Entity<CandidateAttachments>(entity =>
            {
                entity.HasKey(e => e.CandidateAttachmentId)
                    .HasName("PK_CandidateAttachments_1");

                entity.Property(e => e.CandidateAttachmentId).ValueGeneratedNever();

                entity.Property(e => e.AbsenceOfPrecedents).HasMaxLength(300);

                entity.Property(e => e.BirthDateCertificate).HasMaxLength(300);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.FamilyPaper).HasMaxLength(300);

                entity.Property(e => e.Nidcertificate)
                    .HasColumnName("NIDCertificate")
                    .HasMaxLength(300);

                entity.Property(e => e.PaymentReceipt).HasMaxLength(300);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.CandidateAttachments)
                    .HasForeignKey(d => d.CandidateId)
                    .HasConstraintName("FK_CandidateAttachments_Candidates");
            });

            modelBuilder.Entity<CandidateContacts>(entity =>
            {
                entity.HasKey(e => e.CandidateContactId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Object)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ObjectType).HasComment(@"1- نقال
2- تلفون ارضي
3- ايميل");
            });

            modelBuilder.Entity<CandidateRepresentatives>(entity =>
            {
                entity.HasKey(e => e.CandidateRepresentativeId);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FatherName).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.GrandFatherName).HasMaxLength(100);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MotherName).HasMaxLength(100);

                entity.Property(e => e.Nid)
                    .HasColumnName("NID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SurName).HasMaxLength(100);

                entity.HasOne(d => d.Candidate)
                    .WithMany(p => p.CandidateRepresentatives)
                    .HasForeignKey(d => d.CandidateId)
                    .HasConstraintName("FK_CandidateRepresentatives_Candidates");
            });

            modelBuilder.Entity<CandidateUsers>(entity =>
            {
                entity.HasKey(e => e.CandidateUserId);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LoginTryAttemptDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.Property(e => e.UserType)
                    .HasDefaultValueSql("((2))")
                    .HasComment(@"1- مندوب 
2- ممثل");
            });

            modelBuilder.Entity<Candidates>(entity =>
            {
                entity.HasKey(e => e.CandidateId);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CompetitionType).HasComment(@"1- عام 
2- خاص
");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EntityId).HasComment(@"1- if null is candidate
2- if not null is entity");

                entity.Property(e => e.FatherName).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.GrandFatherName).HasMaxLength(100);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MotherName).HasMaxLength(100);

                entity.Property(e => e.Nid)
                    .HasColumnName("NID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Qualification).HasMaxLength(200);

                entity.Property(e => e.SurName).HasMaxLength(100);
            });

            modelBuilder.Entity<Centers>(entity =>
            {
                entity.HasKey(e => e.CenterId);

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.Latitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Longitude)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.ConstituencDetail)
                    .WithMany(p => p.Centers)
                    .HasForeignKey(d => d.ConstituencDetailId)
                    .HasConstraintName("FK_Centers_ConstituencyDetails");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Centers)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_Centers_Offices");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Centers)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Centers_Profile");
            });

            modelBuilder.Entity<ChairDetails>(entity =>
            {
                entity.HasKey(e => e.ChairDetailId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                //entity.HasOne(d => d.Chair)
                //    .WithMany(p => p.ChairDetails)
                //    .HasForeignKey(d => d.ChairId)
                //    .HasConstraintName("FK_ChairDetails_Chairs");
            });

            modelBuilder.Entity<Chairs>(entity =>
            {
                entity.HasKey(e => e.ChairId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Constituency)
                    .WithMany(p => p.Chairs)
                    .HasForeignKey(d => d.ConstituencyId)
                    .HasConstraintName("FK_Chairs_Constituencies");
            });

            modelBuilder.Entity<Constituencies>(entity =>
            {
                entity.HasKey(e => e.ConstituencyId);

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Office)
                    .WithMany(p => p.Constituencies)
                    .HasForeignKey(d => d.OfficeId)
                    .HasConstraintName("FK_Constituencies_Offices");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Constituencies)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Constituencies_Profile");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Constituencies)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_Constituencies_Regions");
            });

            modelBuilder.Entity<ConstituencyDetailChairs>(entity =>
            {
                entity.HasKey(e => e.ConstituencyDetailChairId);

                entity.HasOne(d => d.ChairDetail)
                    .WithMany(p => p.ConstituencyDetailChairs)
                    .HasForeignKey(d => d.ChairDetailId)
                    .HasConstraintName("FK_ConstituencyDetailChairs_ChairDetails");

                entity.HasOne(d => d.ConstituencyDetail)
                    .WithMany(p => p.ConstituencyDetailChairs)
                    .HasForeignKey(d => d.ConstituencyDetailId)
                    .HasConstraintName("FK_ConstituencyDetailChairs_Constituencies");
            });

            modelBuilder.Entity<ConstituencyDetails>(entity =>
            {
                entity.HasKey(e => e.ConstituencyDetailId)
                    .HasName("PK_SubConstituencyId");

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Constituency)
                    .WithMany(p => p.ConstituencyDetails)
                    .HasForeignKey(d => d.ConstituencyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ConstituencyDetails_Constituencies");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.ConstituencyDetails)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_ConstituencyDetails_Profile");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.ConstituencyDetails)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_ConstituencyDetails_Regions");
            });

            modelBuilder.Entity<Endorsements>(entity =>
            {
                entity.HasKey(e => e.EndorsementId);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Nid)
                    .HasColumnName("NID")
                    .HasMaxLength(13)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Entities>(entity =>
            {
                entity.HasKey(e => e.EntityId);

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Descriptions).HasMaxLength(500);

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.Logo).HasColumnName("logo");

                entity.Property(e => e.Name).HasMaxLength(300);

                entity.Property(e => e.Owner).HasMaxLength(250);

                entity.Property(e => e.Phone)
                    .HasMaxLength(25)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<EntityAttachments>(entity =>
            {
                entity.HasKey(e => e.EntityAttachmentId);

                entity.Property(e => e.CampaignAccountNumber).HasMaxLength(300);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.LegalAgreementPoliticalEntity).HasMaxLength(300);

                entity.Property(e => e.NameHeadEntity).HasMaxLength(300);

                entity.Property(e => e.PoliticalEntitySymbol).HasMaxLength(300);
            });

            modelBuilder.Entity<EntityRepresentatives>(entity =>
            {
                entity.HasKey(e => e.EntityRepresentativeId);

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FatherName).HasMaxLength(100);

                entity.Property(e => e.FirstName).HasMaxLength(100);

                entity.Property(e => e.GrandFatherName).HasMaxLength(100);

                entity.Property(e => e.HomePhone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MotherName).HasMaxLength(100);

                entity.Property(e => e.Nid)
                    .HasColumnName("NID")
                    .HasMaxLength(12)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.SurName).HasMaxLength(100);

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EntityRepresentatives)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_EntityRepresentatives_Entities");
            });

            modelBuilder.Entity<EntityUsers>(entity =>
            {
                entity.HasKey(e => e.EntityUserId);

                entity.Property(e => e.EntityUserId).ValueGeneratedNever();

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LoginTryAttemptDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.Property(e => e.UserType)
                    .HasDefaultValueSql("((2))")
                    .HasComment(@"1- مندوب 
2- ممثل");

                entity.HasOne(d => d.Entity)
                    .WithMany(p => p.EntityUsers)
                    .HasForeignKey(d => d.EntityId)
                    .HasConstraintName("FK_EntityUsers_Entities");
            });

            modelBuilder.Entity<Offices>(entity =>
            {
                entity.HasKey(e => e.OfficeId);

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Branch)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.BranchId)
                    .HasConstraintName("FK_Offices_Offices");

                entity.HasOne(d => d.Profile)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.ProfileId)
                    .HasConstraintName("FK_Offices_Profile");
            });

            modelBuilder.Entity<Profile>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(200);

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Regions>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");
            });

            modelBuilder.Entity<Stations>(entity =>
            {
                entity.HasKey(e => e.StationId);

                entity.Property(e => e.ArabicName).HasMaxLength(200);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(600);

                entity.Property(e => e.EnglishName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.Stations)
                    .HasForeignKey(d => d.CenterId)
                    .HasConstraintName("FK_Stations_Centers");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.LastLoginOn).HasColumnType("datetime");

                entity.Property(e => e.LoginName).HasMaxLength(50);

                entity.Property(e => e.LoginTryAttemptDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasMaxLength(25);

                entity.Property(e => e.Status)
                    .HasDefaultValueSql("((0))")
                    .HasComment(@"1-active
2-not active
3-stopped
4-admin
9-delete
");

                entity.Property(e => e.UserType)
                    .HasDefaultValueSql("((2))")
                    .HasComment(@"1-admin
2-user
3-doctor A
4-doctor B
5-doctor C
");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
