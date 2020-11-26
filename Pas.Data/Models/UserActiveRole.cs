using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{
    public partial class ActiveRole : BaseEntityModel
    {        
        [Required]
        public int UserId { get; set; }
        
        [Required]
        public int OrganisationId { get; set; }

        [Required]
        public int RoleId { get; set; }

        public virtual User User { get; set; }
        public virtual Organisation Organisation { get; set; }
    }
}
