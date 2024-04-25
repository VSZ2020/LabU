using LabU.Core.Entities;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultTaskResponseRepository : BaseRepository, ITaskResponseService
    {
        public DefaultTaskResponseRepository(DataContext context): base(context)
        {
        }

        public async Task<bool> AddResponseAsync(TaskResponseEntity response)
        {
            await _context.TaskResponses.AddAsync(response);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditResponseAsync(TaskResponseEntity response)
        {
            var entity = await _context.TaskResponses.FirstOrDefaultAsync(r => r.Id == response.Id);
            if (entity == null)
                return false;

            entity.SubmissionDate = response.SubmissionDate;
            entity.SenderId = response.SenderId;
            entity.TaskId = response.TaskId;
            entity.Comment = response.Comment;

            _context.TaskResponses.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TaskResponseEntity>> GetTaskResponsesAsync(int taskId)
        {
            return await _context.TaskResponses
                .AsNoTracking()
                .Where(r => r.TaskId == taskId)
                .Include(r => r.Attachment)
                .Include(r => r.Sender)
                .Include(r => r.Task)
                .ToListAsync();
        }

        public async Task<bool> RemoveResponseAsync(int responseId)
        {
            await _context.TaskResponses.Where(r => r.Id == responseId).ExecuteDeleteAsync();
            return true;
        }
    }
}
