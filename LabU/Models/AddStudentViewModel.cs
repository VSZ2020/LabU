using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class AddStudentViewModel: AddUserViewModel
    {
        [Display(Name = "Курс обучения")]
        [Range(1, 10, ErrorMessage = "Значение номера курса должно быть в диапазоне от 1 до 10")]
        public int Course { get; set; } = 1;

        [Display(Name = "Академическая группа")]
        public string? AcademicGroupName { get; set; }

        public int? AcademicGroupId { get; set; }

        [Display(Name = "Номер команды")]
        [Range(1, 20, ErrorMessage = "Номер команды должен быть в диапазоне от 1 до 20")]
        public int CommandId { get; set; }
    }
}
