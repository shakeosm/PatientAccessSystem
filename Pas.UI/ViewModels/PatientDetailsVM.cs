using Pas.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pas.UI.ViewModels
{
    public class PatientDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ShortId { get; set; }
    }
}
