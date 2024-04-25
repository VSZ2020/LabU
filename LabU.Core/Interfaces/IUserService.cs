using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using System.Linq.Expressions;

namespace LabU.Core.Interfaces;

public interface IUserService
{
    Task<UserEntity?> FindByIdAsync(int id);
    Task<UserEntity?> FindByUsernameAsync(string username);
    Task<UserEntity?> FindByEmailAsync(string email);

    /// <summary>
    /// Возвращает список ролей пользователя
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<IEnumerable<RoleEntity>> GetUserRolesAsync(UserEntity user);

    Task<IEnumerable<UserEntity>> GetUsers();
    Task<IEnumerable<UserEntity>> GetUsersAsync(
        Expression<Func<UserEntity, bool>>? filter = null,
        Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
        string? includeProps = "");
    Task<IEnumerable<BasePersonEntity>> GetAllPersons();
    
    /// <summary>
    /// Добавляет роль пользователю
    /// </summary>
    /// <param name="user"></param>
    /// <param name="role"></param>
    /// <returns></returns>
    Task<bool> AddToRoleAsync(UserEntity user, RoleEntity role);

    Task<bool> AddToRoleAsync(int[] userIds, int roleId);

    Task<bool> UpdateUserRolesAsync(int userId, int[] roles);

    /// <summary>
    /// Удаляет пользователя из роли
    /// </summary>
    /// <param name="user">Пользователь</param>
    /// <param name="role">Роль</param>
    /// <returns></returns>
    Task<bool> RemoveFromRoleAsync(UserEntity user, RoleEntity role);

    Task<bool> RemoveFromRoleAsync(int[] userIds, int roleId);

    bool ResetLoginAttemptsCount(UserEntity user);
    
    Task<int> CreateUserAsync(UserEntity user);
    Task<bool> UpdateUserAsync(UserEntity user);
    Task<bool> RemoveUserAsync(UserEntity user);
    Task<bool> RemoveUserAsync(int id);
}