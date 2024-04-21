using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Название роли")]
        public string Name { get; set; }

        [Display(Name = "Назначена")]
        public bool IsChecked { get; set; }
    }
}
