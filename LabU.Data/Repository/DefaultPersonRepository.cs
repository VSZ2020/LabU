using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class DefaultPersonRepository : IPersonRepository, IDisposable
    {
        public DefaultPersonRepository(DataContext context)
        {
            _context = context;
        }

        readonly DataContext _context;

        public async Task<TeacherEntity?> FindTeacherByIdAsync(int id)
        {
            return await _context.Teachers.AsNoTracking().Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<StudentEntity?> FindStudentByIdAsync(int id)
        {
            return await _context.Students.AsNoTracking().Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
        }


        public async Task<IEnumerable<TeacherEntity>> GetTeachers(
            Expression<Func<TeacherEntity, bool>>? filter = null,
            Func<IQueryable<TeacherEntity>, IOrderedQueryable<TeacherEntity>>? orderBy = null,
            string? includeProps = "")
        {
            var entities = _context.Teachers.AsNoTracking();

            if (filter != null)
                entities = entities.Where(filter);

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    entities = entities.Include(prop);
                }
            }

            return orderBy != null ? await orderBy(entities).ToListAsync() : await entities.ToListAsync();
        }


        public async Task<IEnumerable<StudentEntity>> GetStudents(
            Expression<Func<StudentEntity, bool>>? filter = null,
            Func<IQueryable<StudentEntity>, IOrderedQueryable<StudentEntity>>? orderBy = null,
            string? includeProps = "")
        {
            var entities = _context.Students.AsNoTracking();

            if (filter != null)
                entities = entities.Where(filter);

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    entities = entities.Include(prop);
                }
            }

            return orderBy != null ? await orderBy(entities).ToListAsync() : await entities.ToListAsync();
        }

        public async Task<bool> CreateStudent(StudentEntity entity)
        {
            await _context.Students.AddAsync(entity);
            return true;
        }

        public async Task<bool> CreateTeacher(TeacherEntity entity)
        {
            await _context.Teachers.AddAsync(entity);
            return true;
        }

        public async Task<bool> EditStudent(StudentEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;

            _context.Students.Update(entity);
            return true;
        }

        public async Task<bool> EditTeacher(TeacherEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.Teachers.Update(entity);
            return true;
        }

        public async Task<bool> RemoveStudent(int id)
        {
            var entity = await _context.Students.FirstOrDefaultAsync(t => t.Id == id);
            if (entity == null)
                return false;

            _context.Students.Remove(entity);
            return true;
        }

        public async Task<bool> RemoveTeacher(int id)
        {
            var entity = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (entity == null)
                return false;

            _context.Teachers.Remove(entity);
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
