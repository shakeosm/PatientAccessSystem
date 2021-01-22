using System.ComponentModel.DataAnnotations;

namespace Pas.Web.ViewModels
{
    public class PrescriptionExaminationItemVM
    {
        public int Id { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Value for {0} must be between {1} and {2}.")]
        public int PrescriptionId { get; set; }

        [Required]
        public int ExaminationId { get; set; }

        [Required]
        public int ExaminationItemOptionId { get; set; }

        [Required]
        public string Findings { get; set; }
    }
}
