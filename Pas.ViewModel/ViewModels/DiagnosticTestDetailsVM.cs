﻿namespace Pas.Web.ViewModels
{
    public class DiagnosticTestDetailsVM
    {
        /* X-Ray, Blood Test, Urine Test */
        public int Id { get; set; }
        public string Name { get; set; }
        public string Date { get; set; }
        public string Details { get; set; }
    }
}
