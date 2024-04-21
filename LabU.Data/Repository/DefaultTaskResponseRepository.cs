using LabU.Core.Entities;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultTaskResponseRepository : ITaskResponseService, IDisposable
    {
        public DefaultTaskResponseRepository(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;

        public bool AddResponse(TaskResponseEntity response)
        {
            throw new NotImplementedException();
        }

        public bool EditResponse(TaskResponseEntity response)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<TaskResponseEntity>> GetAllAsync(int taskId)
        {
            return await _context.TaskResponses
                .AsNoTracking()
                .Where(r => r.TaskId == taskId)
                .Include(r => r.Attachment)
                .Include(r => r.Sender)
                .Include(r => r.Task)
                .ToListAsync();
        }

        public bool RemoveResponse(TaskResponseEntity response)
        {
            throw new NotImplementedException();
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
