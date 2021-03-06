﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pas.Data.Models
{
    public partial class User : BaseEntityModel
    {
        public User()
        {
            Organisations = new HashSet<Organisation>();
            UserOrganisationRoles = new HashSet<UserOrganisationRole>();
            UserRelated = new HashSet<UserRelated>();
            PrescriptionChiefComplaints = new HashSet<PrescriptionChiefComplaints>();
            AddressBooks = new HashSet<AddressBook>();
            ClinicalHistories = new HashSet<ClinicalHistory>();
            PatientIndications = new HashSet<PatientIndications>();
        }
        
        public int Title { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string LastName { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string BanglaName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        
        [Required]
        public int Age { get; set; }

        [Required]
        public int Gender { get; set; }

        [Required]
        [Column(TypeName = "varchar(15)")]
        public string Mobile { get; set; }


        [Column(TypeName = "varchar(10)")]
        public string ShortId { get; set; }

        [Required]
        [Column(TypeName = "varchar(100)")]
        public string Email { get; set; }
        
        public bool EmailConfirmed { get; set; }
        public bool? IsDeceased { get; set; }

        [Required]
        [Column(TypeName = "datetime2(3)")]
        public DateTime CreatedOn { get; set; }        

        public virtual ICollection<Organisation> Organisations { get; set; }
        public virtual ICollection<UserOrganisationRole> UserOrganisationRoles { get; set; }
        public virtual ICollection<UserRelated> UserRelated { get; set; }
        public virtual ICollection<PrescriptionChiefComplaints> PrescriptionChiefComplaints { get; set; }
        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }

        public virtual ICollection<AddressBook> AddressBooks { get; set; }

        public virtual ICollection<ClinicalHistory> ClinicalHistories { get; set; }
        public virtual ICollection<PatientIndications> PatientIndications { get; private set; }
        public virtual ICollection<VitalsHistory> VitalsHistories { get; set; }
        
    }
}
