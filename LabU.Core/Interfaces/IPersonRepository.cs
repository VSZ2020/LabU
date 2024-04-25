using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using System.Linq.Expressions;

namespace LabU.Core.Interfaces
{
    public interface IPersonRepository
    {
        Task<IEnumerable<TeacherEntity>> GetTeachers(
            Expression<Func<TeacherEntity, bool>>? filter = null,
            Func<IQueryable<TeacherEntity>, IOrderedQueryable<TeacherEntity>>? orderBy = null,
            string? includeProps = "");

        Task<IEnumerable<StudentEntity>> GetStudents(
            Expression<Func<StudentEntity, bool>>? filter = null,
            Func<IQueryable<StudentEntity>, IOrderedQueryable<StudentEntity>>? orderBy = null,
            string? includeProps = "");

        Task<TeacherEntity?> FindTeacherByIdAsync(int id);
        Task<StudentEntity?> FindStudentByIdAsync(int id);

        Task<bool> CreateTeacher(TeacherEntity entity);
        Task<bool> EditTeacherAsync(TeacherEntity entity);
        Task<bool> RemoveTeacher(int id);

        Task<bool> CreateStudent(StudentEntity entity);
        Task<bool> EditStudentAsync(StudentEntity entity);
        Task<bool> RemoveStudent(int id);
    }
}
