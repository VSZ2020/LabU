using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class AddTeacherViewModel: AddUserViewModel
    {
        [Display(Name = "Должность")]
        public string? Function { get; set; }

        [Display(Name = "Рабочий адрес")]
        public string? Address { get; set; }
    }
}
