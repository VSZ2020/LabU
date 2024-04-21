using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Models;
using LabU.Utils.Extensions;

namespace LabU.Mappers
{
    public class UsersMapper
    {
        public static UserViewModel Map(UserEntity user, BasePersonEntity? relatedPerson = null)
        {
            var roles = user.Roles != null ? string.Join(", ", user.Roles.Select(r => r.Name).ToArray()) : "-";
            var fullName = relatedPerson != null ? relatedPerson.ToFullName() : "-";

            return new UserViewModel()
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                IsActiveAccount = user.IsActiveAccount,
                IsEmailConfirmed = user.IsEmailConfirmed,
                LastVisit = user.LastVisit,
                FullName = fullName,
                Roles = roles,
            };
        }
    }
}
