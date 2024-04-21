using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class UserViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Имя пользователя")]
        public string? Username { get; set; }

        [Display(Name = "E-mail")]
        public string? Email { get; set; }

        [Display(Name = "ФИО")]
        public string? FullName { get; set; }

        [Display(Name = "Последнее посещение")]
        public DateTime LastVisit { get; set; }

        [Display(Name = "Подтвержден")]
        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Активный")]
        public bool IsActiveAccount { get; set; }

        [Display(Name = "Роли")]
        public string? Roles { get; set; }

    }
}
