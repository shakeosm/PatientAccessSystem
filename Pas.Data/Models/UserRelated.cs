using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class UserRelated
    {
        public int Id { get; set; }
        public int RelatedUserId { get; set; }
        public int RelatedTypeId { get; set; }

        public virtual User RelatedUser { get; set; }
    }
}
