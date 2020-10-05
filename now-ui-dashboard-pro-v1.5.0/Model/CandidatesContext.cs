using System;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Models
{
    public partial class CandidatesContext : DbContext
    {
        public CandidatesContext()
        {
        }

        public CandidatesContext(DbContextOptions<CandidatesContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Branches> Branches { get; set; }
        public virtual DbSet<Centers> Centers { get; set; }
        public virtual DbSet<ChairDetails> ChairDetails { get; set; }
        public virtual DbSet<Chairs> Chairs { get; set; }
        public virtual DbSet<Constituencies> Constituencies { get; set; }
        public virtual DbSet<ConstituencyDetailChairs> ConstituencyDetailChairs { get; set; }
        public virtual DbSet<ConstituencyDetails> ConstituencyDetails { get; set; }
        public virtual DbSet<Offices> Offices { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Regions> Regions { get; set; }
        public virtual DbSet<Stations> Stations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-4AI87L8\\SQLEXPRESS;Database=Candidates;Trusted_Connection=True;;MultipleActiveResultSets=true;");
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

                entity.HasOne(d => d.Chair)
                    .WithMany(p => p.ChairDetails)
                    .HasForeignKey(d => d.ChairId)
                    .HasConstraintName("FK_ChairDetails_Chairs");
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

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Offices)
                    .HasForeignKey(d => d.RegionId)
                    .HasConstraintName("FK_Offices_Regions");
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
