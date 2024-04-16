using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;
using LabU.Core.Interfaces;

namespace LabU.Services.Data;

public class DefaultSubjectsService: ISubjectService
{
    public Task<IEnumerable<SubjectDto>> GetAllAsync(int userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SubjectDto>> GetAllAsync(Expression<Func<SubjectEntity, bool>>? filter = null)
    {
        throw new NotImplementedException();
    }

    public Task<SubjectDto?> FindSubjectByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BasePersonEntity>> GetAttachedUsers(int subjectId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<StudentDto>> GetAttachedStudents(int subjectId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TeacherDto>> GetAttachedTeachers(int subjectId)
    {
        throw new NotImplementedException();
    }

    public bool AddSubject(SubjectDto item)
    {
        throw new NotImplementedException();
    }

    public bool EditSubject(SubjectDto item)
    {
        throw new NotImplementedException();
    }

    public bool RemoveSubject(SubjectDto item)
    {
        throw new NotImplementedException();
    }

    public bool AttachUsers(int[] usersIds)
    {
        throw new NotImplementedException();
    }
}