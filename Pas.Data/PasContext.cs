using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Pas.Data.Models;

namespace Pas.Data
{
    public partial class PasContext : DbContext
    {
        public PasContext()
        {
        }

        public PasContext(DbContextOptions<PasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressBook> AddressBooks { get; set; }
        public virtual DbSet<AilmentTypes> AilmentTypes { get; set; }
        public virtual DbSet<AllergyTypes> AllergyTypes { get; set; }
        
        public virtual DbSet<CategoryTypes> CategoryTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ClinicalHistory> ClinicalHistory { get; set; }
        public virtual DbSet<DiagnosticTest> DiagnosticTest { get; set; }
        public virtual DbSet<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
        public virtual DbSet<DoctorSpeciality> DoctorSpeciality { get; set; }
        public virtual DbSet<Doctors> Doctors { get; set; }
        public virtual DbSet<DosageTypes> DosageTypes { get; set; }
        public virtual DbSet<DrugDosageType> DrugDosageType { get; set; }
        public virtual DbSet<DrugModeOfDelivery> DrugModeOfDelivery { get; set; }
        public virtual DbSet<Drugs> Drugs { get; set; }
        public virtual DbSet<IndicationTypes> IndicationTypes { get; set; }
        public virtual DbSet<ModeOfDelivery> ModeOfDelivery { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }
        public virtual DbSet<PatientAilment> PatientAilmentTypes { get; set; }
        public virtual DbSet<PatientAllergy> PatientAllergies { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
        public virtual DbSet<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserOrganisationRole> UserOrganisationRole { get; set; }
        public virtual DbSet<UserRelated> UserRelated { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Initial Catalog=roogi-test;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<AspNetRoleClaims>(entity =>
            //{
            //    entity.HasIndex(e => e.RoleId);

            //    entity.Property(e => e.RoleId).IsRequired();

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetRoleClaims)
            //        .HasForeignKey(d => d.RoleId);
            //});

            //modelBuilder.Entity<AspNetRoles>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedName)
            //        .HasName("RoleNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedName] IS NOT NULL)");

            //    entity.Property(e => e.Name).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedName).HasMaxLength(256);
            //});

            //modelBuilder.Entity<AspNetUserClaims>(entity =>
            //{
            //    entity.HasIndex(e => e.UserId);

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserClaims)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserLogins>(entity =>
            //{
            //    entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            //    entity.HasIndex(e => e.UserId);

            //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

            //    entity.Property(e => e.ProviderKey).HasMaxLength(128);

            //    entity.Property(e => e.UserId).IsRequired();

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserLogins)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserRoles>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.RoleId });

            //    entity.HasIndex(e => e.RoleId);

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.RoleId);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserRoles)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUserTokens>(entity =>
            //{
            //    entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            //    entity.Property(e => e.LoginProvider).HasMaxLength(128);

            //    entity.Property(e => e.Name).HasMaxLength(128);

            //    entity.HasOne(d => d.User)
            //        .WithMany(p => p.AspNetUserTokens)
            //        .HasForeignKey(d => d.UserId);
            //});

            //modelBuilder.Entity<AspNetUsers>(entity =>
            //{
            //    entity.HasIndex(e => e.NormalizedEmail)
            //        .HasName("EmailIndex");

            //    entity.HasIndex(e => e.NormalizedUserName)
            //        .HasName("UserNameIndex")
            //        .IsUnique()
            //        .HasFilter("([NormalizedUserName] IS NOT NULL)");

            //    entity.Property(e => e.Email).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

            //    entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

            //    entity.Property(e => e.UserName).HasMaxLength(256);
            //});

            
                 modelBuilder.Entity<AddressBook>(entity =>
                 {
                     entity.ToTable("AddressBook", "dbo");

                     entity.HasOne(d => d.User)
                     .WithMany(p => p.AddressBooks)
                     .HasForeignKey(d => d.UserId)
                     .OnDelete(DeleteBehavior.Cascade);
                 });          


            modelBuilder.Entity<CategoryTypes>(entity =>
            {
                entity.ToTable("CategoryTypes", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ClinicalHistory>(entity =>
            {
                entity.ToTable("ClinicalHistory", "Patient");              
            });


            modelBuilder.Entity<DiagnosticTest>(entity =>
            {
                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime2(3)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Details).HasMaxLength(500);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DiagnosticTestHistory>(entity =>
            {
                entity.ToTable("DiagnosticTestHistory", "Patient");

                entity.Property(e => e.Result)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.TestDate).HasColumnType("date");

                entity.Property(e => e.TestReportImageUrl)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.DiagnosticHospital)
                    .WithMany(p => p.DiagnosticTestHistoryDiagnosticHospital)
                    .HasForeignKey(d => d.DiagnosticHospitalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiagnosticTestHistory_Organisation1");

                entity.HasOne(d => d.DiagnosticTest)
                    .WithMany(p => p.DiagnosticTestHistory)
                    .HasForeignKey(d => d.DiagnosticTestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiagnosticTestHistory_DiagnosticTest");

                entity.HasOne(d => d.DoctorHospital)
                    .WithMany(p => p.DiagnosticTestHistoryDoctorHospital)
                    .HasForeignKey(d => d.DoctorHospitalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiagnosticTestHistory_Organisation");

                entity.HasOne(d => d.Doctor)
                    .WithMany(p => p.DiagnosticTestHistory)
                    .HasForeignKey(d => d.DoctorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DiagnosticTestHistory_Doctors");
            });

            modelBuilder.Entity<DoctorSpeciality>(entity =>
            {
                entity.Property(e => e.BanglaName).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Doctors>(entity =>
            {
                entity.Property(e => e.Acheivements)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.BanglaName).HasMaxLength(100);

                entity.Property(e => e.DateCreated).HasColumnType("datetime2(3)");

                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Speciality)
                    .WithMany(p => p.Doctors)
                    .HasForeignKey(d => d.SpecialityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Doctors_DoctorSpeciality");
            });

            modelBuilder.Entity<DosageTypes>(entity =>
            {
                entity.ToTable("DosageTypes", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DrugDosageType>(entity =>
            {
                entity.ToTable("DrugDosageType", "Drug");

                entity.HasOne(d => d.DosageType)
                    .WithMany(p => p.DrugDosageType)
                    .HasForeignKey(d => d.DosageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugDosageType_DosageTypes");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.DrugDosageType)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugDosageType_Drugs");
            });

            modelBuilder.Entity<DrugModeOfDelivery>(entity =>
            {
                entity.ToTable("DrugModeOfDelivery", "Drug");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.DrugModeOfDelivery)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugModeOfDelivery_Drugs");

                entity.HasOne(d => d.ModeOfDelivery)
                    .WithMany(p => p.DrugModeOfDelivery)
                    .HasForeignKey(d => d.ModeOfDeliveryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DrugModeOfDelivery_ModeOfDelivery");
            });

            modelBuilder.Entity<Drugs>(entity =>
            {
                entity.ToTable("Drugs", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.DrugCategoryType)
                    .WithMany(p => p.Drugs)
                    .HasForeignKey(d => d.DrugCategoryTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Drugs_DrugCategoryTypes_DrugCategoryTypeId");
            });

            modelBuilder.Entity<IndicationTypes>(entity =>
            {
                entity.ToTable("IndicationTypes", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ModeOfDelivery>(entity =>
            {
                entity.ToTable("ModeOfDelivery", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Organisation>(entity =>
            {
                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.HeaderBangla)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.HeaderEnglish)
                    .HasMaxLength(1000)
                    .IsUnicode(false);

                entity.Property(e => e.LogoImageFile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.HasOne(d => d.ContactPerson)
                    .WithMany(p => p.Organisations)
                    .HasForeignKey(d => d.ContactPersonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Organisation_Organisation");
            });

            modelBuilder.Entity<Prescription>(entity =>
            {
                entity.ToTable("Prescription", "Patient");

                entity.Property(e => e.DateCreated).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Notes).HasMaxLength(1000);
            });

            modelBuilder.Entity<PrescriptionDiagnosticTest>(entity =>
            {
                entity.ToTable("PrescriptionDiagnosticTest", "Patient");

                entity.Property(e => e.Instructions).HasMaxLength(500);

                entity.HasOne(d => d.DiagnosticTest)
                    .WithMany(p => p.PrescriptionDiagnosticTest)
                    .HasForeignKey(d => d.DiagnosticTestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDiagnosticTest_DiagnosticTest");

                entity.HasOne(d => d.Prescription)
                    .WithMany(p => p.PrescriptionDiagnosticTest)
                    .HasForeignKey(d => d.PrescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDiagnosticTest_Prescription");
            });

            modelBuilder.Entity<PatientAilment>(entity =>
            {
                entity.ToTable("PatientAilment", "Patient");
            });


            modelBuilder.Entity<PatientAllergy>(entity =>
            {
                entity.ToTable("PatientAllergy", "Patient");
            });

            modelBuilder.Entity<PrescriptionDrugs>(entity =>
            {
                entity.ToTable("PrescriptionDrugs", "Patient");

                entity.HasOne(d => d.Dosage)
                    .WithMany(p => p.PrescriptionDrugs)
                    .HasForeignKey(d => d.DosageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDrugs_DosageTypes");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.PrescriptionDrugs)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDrugs_Drugs");

                entity.HasOne(d => d.ModeOfDelivery)
                    .WithMany(p => p.PrescriptionDrugs)
                    .HasForeignKey(d => d.ModeOfDeliveryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDrugs_ModeOfDelivery");

                entity.HasOne(d => d.Prescription)
                    .WithMany(p => p.PrescriptionDrugs)
                    .HasForeignKey(d => d.PrescriptionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDrugs_Prescription");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "User");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "User");

                entity.Property(e => e.BanglaName)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DateOfBirth).HasColumnType("date");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.ShortId)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserOrganisationRole>(entity =>
            {
                entity.ToTable("UserOrganisationRole", "User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Organisation)
                    .WithMany(p => p.UserOrganisationRole)
                    .HasForeignKey(d => d.OrganisationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserOrganisationRole_Organisation");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserOrganisationRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserOrganisationRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserOrganisationRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserOrganisationRole_User");
            });

            modelBuilder.Entity<UserRelated>(entity =>
            {
                entity.ToTable("UserRelated", "User");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.RelatedUser)
                    .WithMany(p => p.UserRelated)
                    .HasForeignKey(d => d.RelatedUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRelated_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
