using System.ComponentModel.DataAnnotations;

namespace LabU.Core.Entities.Identity
{
    public sealed class RoleEntity: BaseEntity
    {
        /// <summary>
        /// Название роли
        /// </summary>
        [Required]
        public string? Name { get; set; }
        public string? NormalizedName { get; set; }
        
        public List<UserEntity>? Users { get; set; }
        
        public List<UserRoleTable>? UserRoleTable { get; set; }
    }
}
