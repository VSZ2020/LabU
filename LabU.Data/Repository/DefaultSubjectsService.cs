using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;
using LabU.Core.Interfaces;

namespace LabU.Data.Repository;

public class DefaultSubjectsService: ISubjectService
{
    public Task<IEnumerable<SubjectEntity>> GetAllAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SubjectEntity>> GetAllAsync(Expression<Func<SubjectEntity, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }

    public Task<SubjectEntity?> FindSubjectByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BasePersonEntity>> GetAttachedUsers(int subjectId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StudentEntity>> GetAttachedStudents(int subjectId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TeacherEntity>> GetAttachedTeachers(int subjectId)
    {
        throw new NotImplementedException();
    }

    public bool AddSubject(SubjectEntity item)
    {
        throw new NotImplementedException();
    }

    public bool EditSubject(SubjectEntity item)
    {
        throw new NotImplementedException();
    }

    public bool RemoveSubject(SubjectEntity item)
    {
        throw new NotImplementedException();
    }

    public bool AttachUsers(int[] usersIds)
    {
        throw new NotImplementedException();
    }
}