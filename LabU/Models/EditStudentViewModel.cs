using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class EditStudentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Никнейм/Логин")]
        [StringLength(100, ErrorMessage = "Длина не должна превышать 100 символов")]
        public string? Username { get; set; }

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Поле фамилии не может быть пустым")]
        [Display(Name = "Фамилия")]
        [DataType(DataType.Text)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Поле имени не может быть пустым")]
        [Display(Name = "Имя")]
        [DataType(DataType.Text)]
        public string? FirstName { get; set; }

        [Display(Name = "Отчество (при наличии)")]
        [DataType(DataType.Text)]
        public string? MiddleName { get; set; }

        [Display(Name = "Активировать аккаунт")]
        public bool IsActiveAccount { get; set; }

        [Display(Name = "Подтверждение e-mail")]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Академическая группа")]
        public string? AcademicGroup { get; set; }
        public int? AcademicGroupId { get; set; }

        [Required(ErrorMessage = "Укажите курс обучения")]
        [Display(Name = "Курс обучения")]
        [Range(1, 10, ErrorMessage = "Значение номера курса должно быть в диапазоне от 1 до 10")]
        public int Course { get; set; }

        [Display(Name = "Номер команды")]
        [Range(1, 20, ErrorMessage = "Номер команды должен быть в диапазоне от 1 до 20")]
        public int CommandId { get; set; }
    }
}
