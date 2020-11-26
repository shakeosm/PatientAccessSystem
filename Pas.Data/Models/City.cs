using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pas.Data.Models
{
    public partial class City : BaseEntityModel
    {
        public City()
        {
            AddressBooks = new HashSet<AddressBook>();
        }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string BanglaName { get; set; }

        public virtual IEnumerable<AddressBook> AddressBooks { get; set; }
    }
}
