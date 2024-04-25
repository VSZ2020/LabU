using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultRoleService : BaseRepository, IRoleService
    {
        public DefaultRoleService(DataContext context): base(context)
        {
        }

        public async Task<bool> CreateAsync(RoleEntity role)
        {
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
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
            return await _context.Roles
                .AsNoTracking()
                .Include(r => r.Users)
                .ToListAsync();
        }

        public bool HasRole(RoleEntity role)
        {
            return _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == role.Id && r.Name == role.Name) != null;
        }

        public bool HasRole(string roleName)
        {
            return _context.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name == roleName) != null;
        }

        public async Task<bool> RemoveAsync(int roleId)
        {
            await _context.Roles.Where(r => r.Id == roleId).ExecuteDeleteAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(RoleEntity role)
        {
            await _context.Roles.Where(r => r.Id == role.Id).ExecuteUpdateAsync(r => r.SetProperty(p => p.Name, p => role.Name).SetProperty(p => p.NormalizedName, p => role.NormalizedName));
            return true;
        }
    }
}
