using LabU.Core;
using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class AddSubjectViewModel
    {
        [Required(ErrorMessage = "Название дисциплины не может быть пустым")]
        [Display(Name = "Название")]
        [StringLength(500, ErrorMessage = "Название дисицплины не должно превышать {0} символов")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Учебный год не указан")]
        [Display(Name = "Учебный год")]
        [RegularExpression(@"\d\d\d\d-\d\d\d\d", ErrorMessage = "Название учебного года должно быть в формате 'yyyy-yyyy'")]
        public string? AcademicYear { get; set; }

        [Required(ErrorMessage = "Не указан семестр обучения")]
        [Display(Name = "Семестр")]
        public string? AcademicTerm { get; set; }
    }
}
