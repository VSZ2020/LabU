using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class DefaultPersonRepository : BaseRepository, IPersonRepository
    {
        public DefaultPersonRepository(DataContext context): base(context)
        {
        }

        public async Task<TeacherEntity?> FindTeacherByIdAsync(int id)
        {
            return await _context.Teachers.AsNoTracking().Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<StudentEntity?> FindStudentByIdAsync(int id)
        {
            return await _context.Students.AsNoTracking().Include(t => t.Account).FirstOrDefaultAsync(t => t.Id == id);
        }


        public Task<IEnumerable<TeacherEntity>> GetTeachers(
            Expression<Func<TeacherEntity, bool>>? filter = null,
            Func<IQueryable<TeacherEntity>, IOrderedQueryable<TeacherEntity>>? orderBy = null,
            string? includeProps = "")
        {
            return base.GetAllAsync(filter, orderBy, includeProps);
        }


        public Task<IEnumerable<StudentEntity>> GetStudents(
            Expression<Func<StudentEntity, bool>>? filter = null,
            Func<IQueryable<StudentEntity>, IOrderedQueryable<StudentEntity>>? orderBy = null,
            string? includeProps = "")
        {
            return base.GetAllAsync(filter, orderBy, includeProps);
        }

        public async Task<bool> CreateStudent(StudentEntity entity)
        {
            await _context.Students.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateTeacher(TeacherEntity entity)
        {
            await _context.Teachers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditStudentAsync(StudentEntity student)
        {
            var entity = await _context.Students.FirstOrDefaultAsync(t => t.Id == student.Id);
            if (entity == null)
                return false;

            entity.LastName = student.LastName;
            entity.FirstName = student.FirstName;
            entity.MiddleName = student.MiddleName;
            entity.Course = student.Course;
            entity.AcademicGroupId = student.AcademicGroupId;
            entity.CommandId = student.CommandId;

            _context.Students.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EditTeacherAsync(TeacherEntity teacher)
        {
            var entity = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == teacher.Id);
            if (entity == null)
                return false;

            entity.LastName = teacher.LastName;
            entity.FirstName = teacher.FirstName;
            entity.MiddleName = teacher.MiddleName;
            entity.Function = teacher.Function;
            entity.Address = teacher.Address;
            
            _context.Teachers.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStudent(int id)
        {
            var entity = await _context.Students.FirstOrDefaultAsync(t => t.Id == id);
            if (entity == null)
                return false;

            _context.Students.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveTeacher(int id)
        {
            var entity = await _context.Teachers.FirstOrDefaultAsync(t => t.Id == id);
            if (entity == null)
                return false;

            _context.Teachers.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
