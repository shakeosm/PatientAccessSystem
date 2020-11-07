using System;
using System.Collections.Generic;

namespace Pas.Data.Models
{
    public partial class DoctorSpeciality
    {
        public DoctorSpeciality()
        {
            Doctors = new HashSet<Doctors>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }

        public virtual ICollection<Doctors> Doctors { get; set; }
    }
}
