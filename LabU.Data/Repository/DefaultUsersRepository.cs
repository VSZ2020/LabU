using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class DefaultUsersRepository : IUserService, IDisposable
    {
        public DefaultUsersRepository(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;


        public async Task<bool> AddToRoleAsync(int[] userIds, int roleId)
        {
            var entities = Enumerable.Range(0, userIds.Length).Select(i => new UserRoleTable() { RoleId = roleId, UserId = userIds[i] }).ToList();
            await _context.AddRangeAsync(entities);
            return true;
        }

        public async Task<bool> AddToRoleAsync(UserEntity user, RoleEntity role)
        {
            var entity = await _context.UserRoleTable.FirstOrDefaultAsync(u => u.RoleId == role.Id && u.UserId == user.Id);
            if (entity != null)
                return false;
            _context.UserRoleTable.Add(new UserRoleTable() { RoleId = role.Id, UserId = user.Id });

            return true;
        }

        public async Task<bool> UpdateRolesAsync(UserEntity user, RoleEntity[] roles)
        {
            var entity = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == user.Id);
            if (entity != null)
            {
                entity.Roles!.Clear();
                foreach(var role in roles)
                {
                    entity.Roles.Add(role);
                }
                _context.Entry(entity).State = EntityState.Modified;
            }
            return false;
        }

        public async Task<bool> UpdateUserRolesAsync(int userId, int[] roles)
        {
            var entity = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);
            if (entity != null)
            {
                entity.Roles!.Clear();
                foreach (var role in _context.Roles.Where(r => roles.Contains(r.Id)))
                {
                    entity.Roles.Add(role);
                }
                _context.Entry(entity).State = EntityState.Modified;
                return true;
            }
            return false;
        }

        public Task<bool> ChangePasswordAsync(UserEntity user, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ChangePasswordAsync(UserEntity user, string newPassword)
        {
            throw new NotImplementedException();
        }

        public async Task<int> CreateUserAsync(UserEntity user)
        {
            var userEntity = await _context.Users.AddAsync(user);
            
            return userEntity.Entity.Id;
        }

        public async Task<UserEntity?> FindByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().Include(u => u.Roles).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserEntity?> FindByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<UserEntity?> FindByUsernameAsync(string username)
        {
            return await _context.Users.AsNoTracking().Include(u => u.Roles).FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<IEnumerable<RoleEntity>> GetUserRolesAsync(UserEntity user)
        {
            return await _context.Roles
                .Include(r => r.Users)
                .AsNoTracking()
                .Where(r => r.Users!.Any(u => u.Id == user.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetAllUsers()
        {
            return await _context.Users
                .AsNoTracking()
                .Include(r => r.Roles)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserEntity>> GetUsersAsync(
            Expression<Func<UserEntity, bool>>? filter = null,
            Func<IQueryable<UserEntity>, IOrderedQueryable<UserEntity>>? orderBy = null,
            string? includeProps = "")
        {
            IQueryable<UserEntity> users = _context.Users.AsNoTracking();

            if (filter != null)
                users = users.Where(filter);

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    users = users.Include(prop);
                }
            }

            return orderBy != null ? await orderBy(users).ToListAsync() : await users.ToListAsync();
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

            return true;
        }

        public async Task<bool> RemoveFromRoleAsync(int[] userIds, int roleId)
        {
            var entities = await _context.UserRoleTable.Where(u => u.RoleId == roleId && userIds.Contains(u.UserId)).ToListAsync();
            _context.RemoveRange(entities);
            return true;
        }

        public async Task<bool> RemoveUserAsync(UserEntity user)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            if (entity == null)
                return false;

            _context.Users.Remove(entity);
            return true;
        }

        public async Task<bool> RemoveUserAsync(int id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (entity == null)
                return false;

            _context.Users.Remove(entity);
            return true;
        }

        public bool ResetLoginAttemptsCount(UserEntity user)
        {
            var entity = _context.Users.FirstOrDefault(u => u.Id == user.Id);
            if (entity != null)
            {
                entity.AccessFiledCount = 0;
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateUserAsync(UserEntity user)
        {
            _context.Users.Entry(user).State = EntityState.Modified;
            _context.Users.Update(user);
            return true;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
