using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class DefaultUsersRepository : BaseRepository, IUserService
    {
        public DefaultUsersRepository(DataContext context): base(context)
        {
            
        }


        public async Task<bool> AddToRoleAsync(int[] userIds, int roleId)
        {
            var entities = Enumerable.Range(0, userIds.Length).Select(i => new UserRoleTable() { RoleId = roleId, UserId = userIds[i] }).ToList();
            await _context.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddToRoleAsync(UserEntity user, RoleEntity role)
        {
            var entity = await _context.UserRoleTable.FirstOrDefaultAsync(u => u.RoleId == role.Id && u.UserId == user.Id);
            if (entity != null)
                return false;
            _context.UserRoleTable.Add(new UserRoleTable() { RoleId = role.Id, UserId = user.Id });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserRolesAsync(int userId, int[] roles)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (entity == null)
                return false;

            var userRoles = _context.UserRoleTable.Where(r => r.UserId == userId).AsNoTracking().Select(r => r.RoleId);
            var rolesToAdd = roles.Except(userRoles);
            var rolesToRemove = userRoles.Except(roles);

            foreach (var role in rolesToAdd)
                _context.Add(new UserRoleTable() { UserId = userId, RoleId = role});

            foreach (var role in rolesToRemove)
                _context.Remove(new UserRoleTable() { UserId = userId, RoleId = role });
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<int> CreateUserAsync(UserEntity user)
        {
            var userEntity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return userEntity.Entity.Id;
        }

        public async Task<UserEntity?> FindByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> FindByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserEntity?> FindByUsernameAsync(string username)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(u => u.Roles)
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<RoleEntity>> GetUserRolesAsync(UserEntity user)
        {
            return await _context.Roles
                .Include(r => r.Users)
                .AsNoTracking()
                .Where(r => r.Users!.Any(u => u.Id == user.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetUsers()
        {
            return await _context.Users
                .AsNoTracking()
                .Include(r => r.Roles)
                .ToListAsync();
        }

        public Task<IEnumerable<UserEntity>> GetUsersAsync(
            Expression<Func<UserEntity, bool>>? filter = null,
            Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
            string? includeProps = "")
        {
            return base.GetAllAsync(filter, orderBy, includeProps);
        }

        public async Task<IEnumerable<BasePersonEntity>> GetAllPersons()
        {
            var teachers = await _context.Teachers.AsNoTracking().Cast<BasePersonEntity>().ToListAsync();
            var students = await _context.Students.AsNoTracking().Cast<BasePersonEntity>().ToListAsync();
            return teachers.Concat(students);
        }

        public async Task<bool> RemoveFromRoleAsync(UserEntity user, RoleEntity role)
        {
            var entity = await _context.UserRoleTable.FirstOrDefaultAsync(u => u.RoleId == role.Id && u.UserId == user.Id);
            if (entity == null)
                return false;
            _context.UserRoleTable.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveFromRoleAsync(int[] userIds, int roleId)
        {
            var entities = await _context.UserRoleTable
                .Where(u => u.RoleId == roleId && userIds.Contains(u.UserId))
                .ToListAsync();
            _context.RemoveRange(entities);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserAsync(UserEntity user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (entity == null)
                return false;

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveUserAsync(int id)
        {
            await _context.Users.Where(u => u.Id == id).ExecuteDeleteAsync();

            return true;
        }

        public bool ResetLoginAttemptsCount(UserEntity user)
        {
            _context.Users.Where(u => u.Id == user.Id).ExecuteUpdate(u => u.SetProperty(p => p.AccessFiledCount, p => 0));
            return true;
        }

        public async Task<bool> UpdateUserAsync(UserEntity user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (entity == null)
                return false;

            entity.Username = user.Username;
            entity.LastVisit = user.LastVisit;
            entity.IsActiveAccount = user.IsActiveAccount;
            entity.IsEmailConfirmed = user.IsEmailConfirmed;
            entity.SecurityStamp = user.SecurityStamp;
            entity.Email = user.Email;
            entity.AccessFiledCount = user.AccessFiledCount;

            _context.Users.Update(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
