using System.Collections.Generic;
using System.Linq;

namespace Pas.Web.ViewModels
{
    public class DoctorDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BanglaName { get; set; }
        public string Acheivements { get; set; }
        public string AcheivementsBangla { get; set; }
        public string Speciality { get; set; }
        public string SpecialityBangla { get; set; }
        public string RegistrationNumber { get; set; }
        public string RegistrationNumberBangla { get; set; }

        public string HeaderEnglish { get; set; }

        public string HeaderBangla { get; set; }

        public IEnumerable<SpecialityVM> SpecialityList { get; set; }
        public string Specialities()
        {
            if (SpecialityList.Count() > 0)
            {
                return string.Join(", ", SpecialityList.Select(s => s.Name));
            }
            return "";
        }

        public IEnumerable<DoctorDegreesVM> DoctorDegreeList { get; set; }
        public string DoctorDegrees()
        {
            //var dList = new List<string>();
            //var englishDegreeList = DoctorDegreeList.Select(s => s.Name );
            if (DoctorDegreeList.Count() > 0)
            {
                return string.Join(", ", DoctorDegreeList.Select(s => s.Name));
            }
            return "";
        }
    } /*End of DoctorDetailsVM*/

    public class DoctorDetailsUpdateVM
    {
        public int Id { get; set; }
        public string Acheivements { get; set; }
        public string AcheivementsBangla { get; set; }
        public string Speciality { get; set; }
        public string SpecialityBangla { get; set; }
        public string RegistrationNumber { get; set; }

        public string HeaderEnglish { get; set; }

        public string HeaderBangla { get; set; }

    } /*End of DoctorDetailsVM*/


    public class SpecialityVM
    {
        public string Name { get; set; }
        public string BanglaName { get; set; }

    }
    public class DoctorDegreesVM
    {
        public string Name { get; set; }
        public string BanglaName { get; set; }

    }

}
