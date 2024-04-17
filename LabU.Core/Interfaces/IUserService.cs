using LabU.Core.Entities;
using LabU.Core.Entities.Identity;

namespace LabU.Core.Interfaces;

public interface IUserService
{
    Task<bool> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword);
    Task<bool> ChangePasswordAsync(UserEntity user, string newPassword);

    Task<UserEntity?> FindByIdAsync(int id);
    Task<UserEntity?> FindByUsernameAsync(string username);
    Task<UserEntity?> FindByEmailAsync(string email);

    /// <summary>
    /// Возвращает список ролей пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<IEnumerable<RoleEntity>> GetUserRolesAsync(UserEntity user);
    
    /// <summary>
    /// Добавляет роль пользователю
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<bool> AddToRoleAsync(UserEntity user, RoleEntity role);
    
    /// <summary>
    /// Удаляет пользователя из роли
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="role">Роль</param>
    /// <returns></returns>
    Task<bool> RemoveFromRoleAsync(UserEntity user, RoleEntity role);

    void ResetLoginAttemptsCount(UserEntity user);
    
    Task<bool> CreateUserAsync(UserEntity user);
    Task<bool> UpdateUserAsync(UserEntity user);
    Task<bool> RemoveUserAsync(UserEntity user);
}