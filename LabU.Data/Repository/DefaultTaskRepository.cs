using LabU.Core;
using LabU.Core.Entities;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class DefaultTaskRepository: BaseRepository,  ITaskRepository
    {
        public DefaultTaskRepository(DataContext context): base(context)
        {}
        
        public async Task<IEnumerable<TaskEntity>> GetTasksAsync(int subjectId, int personId = 0)
        {
            Expression<Func<TaskEntity, bool>> filter = e => (subjectId != 0 ? e.SubjectId == subjectId : true) && (personId != 0 ? e.Users.Any(u => u.Id == personId) : true);
            return await _context.Tasks.AsNoTracking()
                .Include(t => t.Users)
                .Include(t => t.Subject)
                .Where(filter).ToListAsync();
        }

        public Task<IEnumerable<TaskEntity>> GetTasksAsync(
            Expression<Func<TaskEntity, bool>>? filter = null, 
            Func<IQueryable<TaskEntity>, IOrderedQueryable<TaskEntity>>? orderBy = null, 
            string? includeProps = "")
        {
            return base.GetAllAsync(filter, orderBy, includeProps);
        }

        public async Task<TaskEntity?> FindByIdAsync(int id)
        {
            var entity = await _context.Tasks
                .AsNoTracking()
                .Include(e => e.Subject)
                .Include(t => t.Users)
                .Include(t => t.Responses)
                .FirstOrDefaultAsync(e => e.Id == id); 

            return entity;
        }

        public async Task<TaskEntity?> FindByIdAsync(int id, string? includeProps = null)
        {
            var query = _context.Tasks.AsNoTracking();

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach(var prop in includeProps.Split(',', StringSplitOptions.RemoveEmptyEntries)){
                    query = query.Include(prop);
                }
            }
            return await query.FirstOrDefaultAsync(t => t.Id == id);
        }

        public bool IsOwnedByUser(int taskId, int userId)
        {
            return _context.Tasks.AsNoTracking()
                .Include(e => e.Users)
                .FirstOrDefault(t => t.Id == taskId && t.Users!.Any(u => u.Id == userId)) != null;
        }

        public async Task<IEnumerable<TeacherEntity>> GetAttachedReviewersAsync(int taskId)
        {
            var teachers = await _context.Teachers
                .AsNoTracking()
                .Include(e => e.Tasks)
                .Where(e => e.Tasks!.Any(t => t.Id == taskId))
                .ToArrayAsync();

            return teachers;
        }

        public async Task<IEnumerable<StudentEntity>> GetAttachedStudentsAsync(int taskId)
        {
            var students = await _context.Students
                .AsNoTracking()
                .Include(e => e.Tasks)
                .Where(e => e.Tasks!.Any(t => t.Id == taskId))
                .ToArrayAsync();

            return students;
        }

        public bool AddTask(TaskEntity task)
        {
            _context.Tasks.Add(task);
            return true;
        }

        public bool EditTask(TaskEntity task)
        {
            var taskEntity = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskEntity == null) 
                return false;
            _context.Tasks.Attach(taskEntity);
            _context.Tasks.Update(taskEntity);
            return true;
        }

        public bool RemoveTask(TaskEntity task)
        {
            var taskEntity = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskEntity == null) 
                return false;
            _context.Tasks.Remove(taskEntity);
            return true;
        }


        public async Task<bool> AttachPerson(int userId, int taskId)
        {
            var relationship = await _context.PersonTaskTable.FirstOrDefaultAsync(t => t.UserId == userId && t.TaskId == taskId);
            if (relationship != null)
                return false;
            await _context.PersonTaskTable.AddAsync(new TaskPersonTable() { UserId = userId, TaskId = taskId});
            return true;
        }

        public async Task<bool> DetachPersonAsync(int userId, int taskId)
        {
            var relationship = await _context.PersonTaskTable.FirstOrDefaultAsync(t => t.UserId == userId && t.TaskId == taskId);
            if (relationship == null)
                return false;
            _context.PersonTaskTable.Remove(relationship);
            return true;
        }

        public async Task<bool> ChangeTaskStatus(int taskId, ResponseState newStatus)
        {
            await _context.Tasks.Where(t => t.Id == taskId).ExecuteUpdateAsync(t => t.SetProperty(p => p.Status, p => newStatus));
            return true;
        }
    }
}
