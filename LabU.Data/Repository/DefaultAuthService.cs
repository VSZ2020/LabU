using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultAuthService : IAuthService, IDisposable
    {
        public DefaultAuthService(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;
        
        public async Task<bool?> TryAuthUserAsync(string login, string password)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == login && u.PasswordHash == password) != null;
        }

        public int GetLoginAttemptsCount(string login, int maxAttemptsCount = 5)
        {
            return _context.Users.FirstOrDefault(u => u.Username == login)?.AccessFiledCount ?? 0;
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
