using LabU.Core.Entities.Identity;
using LabU.Models;

namespace LabU.Mappers
{
    public class RolesMapper
    {
        public static UserRoleViewModel Map(RoleEntity e, bool isChecked = false)
        {
            return new UserRoleViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                IsChecked = isChecked
            };
        }
    }
}
