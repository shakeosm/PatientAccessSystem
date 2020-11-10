using System;
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
            PatientAilments = new HashSet<PatientAilment>();
        }

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
        public virtual ICollection<PatientAilment> PatientAilments { get; set; }
        public virtual ICollection<PatientAllergy> PatientAllergies { get; set; }
        
    }
}
