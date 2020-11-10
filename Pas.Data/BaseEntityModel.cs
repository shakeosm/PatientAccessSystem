using System.ComponentModel.DataAnnotations;

namespace Pas.Data
{
    public class BaseEntityModel
    {
        [Key]
        public int Id { get; set; }
    }
}
