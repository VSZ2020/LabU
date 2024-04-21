using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultRoleService : IRoleService, IDisposable
    {
        public DefaultRoleService(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;

        public async Task<bool> CreateAsync(RoleEntity role)
        {
            await _context.Roles.AddAsync(role);
            return true;
        }

        public async Task<RoleEntity?> FindByIdAsync(int id)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<RoleEntity?> FindByNameAsync(string name)
        {
            return await _context.Roles.FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<RoleEntity>> GetRoles()
        {
            return await _context.Roles.AsNoTracking().Include(r => r.Users).ToListAsync();
        }

        public bool HasRole(RoleEntity role)
        {
            return _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == role.Id && r.Name == role.Name) != null;
        }

        public bool HasRole(string roleName)
        {
            return _context.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Name == roleName) != null;
        }

        public async Task<bool> RemoveAsync(RoleEntity role)
        {
            var entity = await _context.Roles.FirstOrDefaultAsync(r => r.Id == role.Id);
            if (entity == null)
                return false;
            _context.Roles.Remove(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(RoleEntity role)
        {
            //_context.Roles.Attach(role);
            _context.Roles.Entry(role).State = EntityState.Modified;
            _context.Roles.Update(role);
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
