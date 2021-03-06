﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Pas.Data.Models;

namespace Pas.Data
{
    public partial class PasContext : DbContext
    {
        //public IConfiguration _configuration;

        public PasContext()
        {
            //_configuration = Configuration;
        }

        public PasContext(DbContextOptions<PasContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AddressBook> AddressBooks { get; set; }
        public virtual DbSet<AdviseInstructions> AdviseInstructions { get; set; }        
        public virtual DbSet<AllergyTypes> AllergyTypes { get; set; }
        
        public virtual DbSet<CategoryTypes> CategoryTypes { get; set; }
        public virtual DbSet<City> Cities { get; set; }
        public virtual DbSet<ClinicalHistory> ClinicalHistory { get; set; }
        public virtual DbSet<DiagnosticTest> DiagnosticTest { get; set; }
        public virtual DbSet<DiagnosticTestHistory> DiagnosticTestHistory { get; set; }
        public virtual DbSet<DoctorSpeciality> DoctorSpeciality { get; set; }
        public virtual DbSet<DoctorMedicalDegrees> DoctorMedicalDegrees { get; set; }
        public virtual DbSet<Speciality> Speciality { get; set; }
        public virtual DbSet<MedicalDegree> MedicalDegrees { get; set; }
        public virtual DbSet<DoctorProfile> DoctorProfile { get; set; }
        public virtual DbSet<StrengthType> DosageTypes { get; set; }
        public virtual DbSet<DrugStrengthType> DrugDosageType { get; set; }
        public virtual DbSet<DrugModeOfDelivery> DrugModeOfDelivery { get; set; }
        public virtual DbSet<Drugs> Drugs { get; set; }
        public virtual DbSet<DrugBrands> DrugBrands { get; set; }
        public virtual DbSet<BrandDoseTemplate> BrandDoseTemplates { get; set; }
        public virtual DbSet<BrandForIndications> BrandForIndications { get; set; }
        public virtual DbSet<IndicationTypes> IndicationTypes { get; set; }
        public virtual DbSet<DrugIndicationTypes> DrugIndicationTypes { get; set; }
        public virtual DbSet<IntakePattern> IntakePatterns { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<ModeOfDelivery> ModeOfDelivery { get; set; }
        public virtual DbSet<Organisation> Organisation { get; set; }        
        public virtual DbSet<PatientAllergy> PatientAllergies { get; set; }
        public virtual DbSet<PatientIndications> PatientIndications { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<PrescriptionChiefComplaints> PrescriptionChiefComplaints { get; set; }
        public virtual DbSet<PrescriptionDiagnosticTest> PrescriptionDiagnosticTest { get; set; }
        public virtual DbSet<PrescriptionDrugs> PrescriptionDrugs { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Symptoms> Symptoms { get; set; }
        public virtual DbSet<StrengthType> StrengthTypes { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserOrganisationRole> UserOrganisationRole { get; set; }
        public virtual DbSet<UserRelated> UserRelated { get; set; }
        public virtual DbSet<ActiveRole> ActiveRoles { get; set; }
        public virtual DbSet<VitalsHistory>  VitalsHistories{ get; set; }        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new NotImplementedException("override void OnConfiguring-> optionsBuilder.IsConfigured = false");
                //optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Initial Catalog=roogi-test;Trusted_Connection=True;");
                //optionsBuilder.UseSqlServer(_configuration.GetConnectionString("PasData"));
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

            modelBuilder.Entity<AdviseInstructions>(entity =>
            {
                entity.ToTable("AdviseInstructions", "dbo");                
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

            modelBuilder.Entity<Speciality>(entity =>
            {
                entity.ToTable("Speciality", "dbo");
            });

            modelBuilder.Entity<MedicalDegree>(entity =>
            {
                entity.ToTable("MedicalDegrees", "dbo");
            });

            modelBuilder.Entity<DoctorSpeciality>(entity =>
            {
                entity.Property(e => e.BanglaName).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DoctorMedicalDegrees>(entity =>
            {
                entity.ToTable("DoctorMedicalDegrees", "dbo");
            });

            modelBuilder.Entity<DoctorProfile>(entity =>
            {
                entity.ToTable("DoctorProfile", "dbo");

                entity.Property(e => e.Acheivements)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime2(3)");                
                
            });

            modelBuilder.Entity<StrengthType>(entity =>
            {
                entity.ToTable("DosageTypes", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DrugStrengthType>(entity =>
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

            modelBuilder.Entity<DrugBrands>(entity =>
            {
                entity.ToTable("DrugBrands", "Drug");
            
            });

            

            modelBuilder.Entity<IndicationTypes>(entity =>
            {
                entity.ToTable("IndicationTypes", "dbo");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DrugIndicationTypes>(entity =>
            {
                entity.ToTable("DrugIndicationTypes", "drug");
            });

            modelBuilder.Entity<IntakePattern>(entity =>
            {
                entity.ToTable("IntakePattern", "drug");
            });


            modelBuilder.Entity<BrandForIndications>(entity =>
            {
                entity.ToTable("BrandForIndications", "drug");

                entity.HasOne(d => d.DrugBrands)
                    .WithMany(p => p.BrandForIndications)
                    .HasForeignKey(d => d.DrugBrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.IndicationType)
                    .WithMany(p => p.BrandForIndications)
                    .HasForeignKey(d => d.IndicationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

            });
            modelBuilder.Entity<BrandDoseTemplate>(entity =>
            {
                entity.ToTable("BrandDoseTemplate", "drug"); 
            });

            modelBuilder.Entity<ModeOfDelivery>(entity =>
            {
                entity.ToTable("ModeOfDelivery", "Drug");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });


            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("Manufacturer", "dbo");
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

            modelBuilder.Entity<PrescriptionChiefComplaints>(entity =>
            {
                entity.ToTable("PrescriptionChiefComplaints", "Patient");
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
                       
            modelBuilder.Entity<PatientAllergy>(entity =>
            {
                entity.ToTable("PatientAllergy", "Patient");
            });

            modelBuilder.Entity<PatientIndications>(entity =>
            {
                entity.ToTable("PatientIndications", "Patient");
            });

            modelBuilder.Entity<PrescriptionDrugs>(entity =>
            {
                entity.ToTable("PrescriptionDrugs", "Patient");

                entity.HasOne(d => d.Drug)
                    .WithMany(p => p.PrescriptionDrugs)
                    .HasForeignKey(d => d.DrugId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDrugs_Drugs");

                entity.HasOne(d => d.DrugBrands)
                    .WithMany(p => p.PrescriptionDrugs)
                    .HasForeignKey(d => d.DrugBrandId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PrescriptionDrugs_DrugBrands");

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

            modelBuilder.Entity<Symptoms>(entity =>
            {
                entity.ToTable("Symptoms", "dbo");
            });

            modelBuilder.Entity<StrengthType>(entity =>
            {
                entity.ToTable("StrengthType", "drug");
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


            modelBuilder.Entity<ActiveRole>(entity =>
            {
                entity.ToTable("ActiveRole", "User");
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
            
            modelBuilder.Entity<VitalsHistory>(entity =>
            {
                entity.ToTable("VitalsHistory", "Patient");                
            });
               
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
