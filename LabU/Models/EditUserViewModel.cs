using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите логин")]
        [Display(Name = "Никнейм/Логин")]
        [StringLength(100, ErrorMessage = "Длина не должна превышать 100 символов")]
        public string? Username { get; set; }

        [Display(Name = "Старый пароль")]
        [DataType(DataType.Password)]
        public string? OldPassword { get; set; }

        [Display(Name = "Новый пароль")]
        [DataType(DataType.Password)]
        [StringLength(18, ErrorMessage = "Пароль должен содержать от {2} до {1} символов", MinimumLength = 6)]
        public string? Password { get; set; }

        [Display(Name = "Подтверждение пароля")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

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
    }
}
