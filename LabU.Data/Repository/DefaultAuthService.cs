using LabU.Core.Interfaces;
using LabU.Core.Utils;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultAuthService : BaseRepository, IAuthService
    {
        public DefaultAuthService(DataContext context): base(context)
        {
            
        }
        
        public async Task<bool> TryAuthUserAsync(string login, string password)
        {
            var entity = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Username == login);
            if (entity == null || !entity.IsActiveAccount)
                return false;

            var correctHash = entity.PasswordHash == Hasher.HashPassword(password);
            return correctHash;
        }

        public int GetLoginAttemptsCount(string login, int maxAttemptsCount = 5)
        {
            return _context.Users.AsNoTracking().FirstOrDefault(u => u.Username == login)?.AccessFiledCount ?? 0;
        }

        public int AddLoginAttempt(int userId)
        {
            return _context.Users.Where(u => u.Id == userId).ExecuteUpdate(u => u.SetProperty(p => p.AccessFiledCount, p => p.AccessFiledCount + 1));
        }

        public async Task<bool> ChangePasswordAsync(int userId, string oldPassword, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            if (Hasher.HashPassword(oldPassword) != user.PasswordHash)
                return false;

            user.PasswordHash = Hasher.HashPassword(newPassword);
            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangePasswordAsync(int userId, string newPassword)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;

            user.PasswordHash = Hasher.HashPassword(newPassword);
            _context.Update(user);
            await _context.SaveChangesAsync();

            return true;
        }

        
    }
}
