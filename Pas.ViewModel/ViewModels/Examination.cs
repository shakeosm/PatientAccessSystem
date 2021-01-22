using System;
using System.Collections.Generic;
using System.Text;

namespace Pas.Web.ViewModels
{
    public class ExaminationItemVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }

    }
    public class ExaminationSubItemOptionsVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ParentId { get; set; }

    }
}
