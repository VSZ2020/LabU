using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;

namespace LabU.Core.Interfaces
{
    public interface ISubjectService
    {
        Task<IEnumerable<SubjectEntity>> GetAllAsync(int userId);
        Task<IEnumerable<SubjectEntity>> GetAllAsync(Expression<Func<SubjectEntity, bool>>? filter = null);

        Task<SubjectEntity?> FindSubjectByIdAsync(int id);
        Task<IEnumerable<BasePersonEntity>> GetAttachedUsers(int subjectId);
        Task<IEnumerable<StudentEntity>> GetAttachedStudents(int subjectId);
        Task<IEnumerable<TeacherEntity>> GetAttachedTeachers(int subjectId);

        bool AddSubject(SubjectEntity item);
        bool EditSubject(SubjectEntity item);
        bool RemoveSubject(SubjectEntity item);

        bool AttachUsers(int[] usersIds);
    }
}
