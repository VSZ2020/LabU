using LabU.Core.Entities.Identity;

namespace LabU.Core.Interfaces;

public interface IRoleService
{
    Task<bool> CreateAsync(RoleEntity role);
    Task<bool> UpdateAsync(RoleEntity role);
    Task<bool> RemoveAsync(RoleEntity role);

    /// <summary>
    /// Возвращает список доступных ролей в системе
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<RoleEntity>> GetRoles();

    /// <summary>
    /// Проверяет существование роли
    /// </summary>
    /// <param name="role"></param>
    /// <returns></returns>
    bool HasRole(RoleEntity role);
    
    bool HasRole(string roleName);

    Task<RoleEntity?> FindByIdAsync(int id);
    Task<RoleEntity?> FindByNameAsync(string name);
}