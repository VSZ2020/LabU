using System.Collections.Immutable;
using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;
using LabU.Core.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LabU.Data.Repository
{
    public class DefaultTaskService: ITaskService
    {
        public DefaultTaskService(DataContext context)
        {
            _context = context;
        }

        private readonly DataContext _context;
        
        public Task<IEnumerable<TaskEntity>> GetAllAsync(int subjectId, int personId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<TaskEntity>> GetAllAsync(Expression<Func<TaskEntity, bool>>? filter = null, Func<IQueryable<TaskEntity>, IOrderedQueryable<TaskEntity>>? orderBy = null, string? includeProps = "")
        {
            throw new NotImplementedException();
        }

        public async Task<TaskEntity?> FindByIdAsync(int id)
        {
            var entity = await _context.Tasks
                .AsNoTracking()
                .Include(e => e.Subject)
                .FirstOrDefaultAsync(e => e.Id == id); ;

            return entity;
        }

        public bool IsOwnedByUser(int taskId, int userId)
        {
            return _context.Tasks.AsNoTracking().Include(e => e.Users)
                .FirstOrDefault(t => t.Id == taskId && t.Users.Any(u => u.Id == userId)) != null;
        }

        public async Task<IEnumerable<TeacherEntity>> GetAttachedReviewersAsync(int taskId)
        {
            var teachers = await _context.Teachers
                .AsNoTracking()
                .Include(e => e.Tasks)
                .Where(e => e.Tasks.Any(t => t.Id == taskId))
                .ToArrayAsync();

            return teachers;
        }

        public async Task<IEnumerable<StudentEntity>> GetAttachedStudentsAsync(int taskId)
        {
            var students = await _context.Students
                .AsNoTracking()
                .Include(e => e.Tasks)
                .Where(e => e.Tasks.Any(t => t.Id == taskId))
                .ToArrayAsync();

            return students;
        }

        public bool AddTask(TaskEntity task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
            return true;
        }

        public bool EditTask(TaskEntity task)
        {
            var taskEntity = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskEntity == null) 
                return false;
            _context.Tasks.Attach(taskEntity);
            _context.Tasks.Update(taskEntity);
            _context.SaveChanges();
            return true;
        }

        public bool RemoveTask(TaskEntity task)
        {
            var taskEntity = _context.Tasks.FirstOrDefault(t => t.Id == task.Id);
            if (taskEntity == null) 
                return false;
            _context.Tasks.Remove(taskEntity);
            _context.SaveChanges();
            return true;
        }
    }
}
