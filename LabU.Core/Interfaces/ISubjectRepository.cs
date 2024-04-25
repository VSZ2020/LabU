using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;

namespace LabU.Core.Interfaces
{
    public interface ISubjectRepository
    {
        Task<IEnumerable<SubjectEntity>> GetSubjectsAsync(int userId);
        Task<IEnumerable<SubjectEntity>> GetSubjectsAsync(Expression<Func<SubjectEntity, bool>>? filter = null, Func<IQueryable<SubjectEntity>, IOrderedQueryable<SubjectEntity>>? orderBy = null, string? includeProps = null);

        Task<SubjectEntity?> FindSubjectByIdAsync(int id);
        Task<IEnumerable<BasePersonEntity>> GetAttachedUsers(int subjectId);
        Task<IEnumerable<StudentEntity>> GetAttachedStudents(int subjectId);
        Task<IEnumerable<TeacherEntity>> GetAttachedTeachers(int subjectId);

        Task<bool> AddSubjectAsync(SubjectEntity item);
        Task<bool> EditSubjectAsync(SubjectEntity item);
        Task<bool> RemoveSubjectAsync(int id);

        Task<bool> UpdateAttachedUsersAsync(int subjectId, int[] usersIds);

        Task<bool> UpdateAttachedTeachersAsync(int subjectId, int[] usersIds);
        Task<bool> UpdateAttachedStudentsAsync(int subjectId, int[] usersIds);

        Task<bool> AttachPerson(int userId, int taskId);
        Task<bool> DetachPersonAsync(int userId, int taskId);
    }
}
