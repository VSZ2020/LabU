using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities.Identity;

public class UserRoleTable
{
    public int RoleId { get; set; }

    public RoleEntity? Role { get; set; }
    
    public int UserId { get; set; }

    public UserEntity? User { get; set; }
}