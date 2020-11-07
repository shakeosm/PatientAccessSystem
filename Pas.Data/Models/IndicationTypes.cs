using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class IndicationTypes
    {
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public int CreatedById { get; set; }
        public string Name { get; set; }
    }
}
