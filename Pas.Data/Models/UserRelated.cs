using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pas.Data.Models
{
    public partial class UserRelated : BaseEntityModel
    {
        [Required]

        public int RelatedUserId { get; set; }
        [Required]
        public int RelatedTypeId { get; set; }

        public virtual User RelatedUser { get; set; }
    }
}
