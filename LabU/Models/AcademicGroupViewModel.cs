using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class AcademicGroupViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Название группы не может быть пустым")]
        [StringLength(100, ErrorMessage = "Длина названия группы не может превышать 100 знаков")]
        [Display(Name = "Название группы")]
        public string Name { get; set; }
    }
}
