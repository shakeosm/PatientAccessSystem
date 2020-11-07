using Pas.Common.Enums;
using System;

namespace Pas.Web.ViewModels
{
    public class PatientDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string ShortId { get; set; }
    }
}
