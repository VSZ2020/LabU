using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;

namespace LabU.Data.Repository;

public class DefaultSubjectsRepository: BaseRepository, ISubjectRepository
{
    public DefaultSubjectsRepository(DataContext ctx): base(ctx)
    {
    }

    public async Task<IEnumerable<SubjectEntity>> GetSubjectsAsync(int userId)
    {
        return await _context.Subjects
            .AsNoTracking()
            .Include(s => s.Users)
            .Where(s => s.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }

    public Task<IEnumerable<SubjectEntity>> GetSubjectsAsync(
        Expression<Func<SubjectEntity, bool>>? filter = null,
        Func<IQueryable<SubjectEntity>, IOrderedQueryable<SubjectEntity>>? orderBy = null,
        string? includeProps = null)
    {
        return base.GetAllAsync(filter, orderBy, includeProps);
    }

    public async Task<SubjectEntity?> FindSubjectByIdAsync(int id)
    {
        return await _context.Subjects
            .AsNoTracking()
            .Include(s => s.Users)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<BasePersonEntity>> GetAttachedUsers(int subjectId)
    {
        var students = await GetAttachedStudents(subjectId);
        var teacher = await GetAttachedTeachers(subjectId);
        return students.Cast<BasePersonEntity>().Concat(teacher.Cast<BasePersonEntity>());
    }

    public async Task<IEnumerable<StudentEntity>> GetAttachedStudents(int subjectId)
    {
        var students = await _context.Students
                .AsNoTracking()
                .Include(e => e.Subjects)
                .Where(e => e.Subjects!.Any(t => t.Id == subjectId))
                .ToArrayAsync();

        return students;    
    }

    public async Task<IEnumerable<TeacherEntity>> GetAttachedTeachers(int subjectId)
    {
        var teachers = await _context.Teachers
                .AsNoTracking()
                .Include(e => e.Subjects)
                .Where(e => e.Subjects!.Any(t => t.Id == subjectId))
                .ToArrayAsync();

        return teachers;
    }

    public async Task<bool> AddSubjectAsync(SubjectEntity item)
    {
        await _context.Subjects.AddAsync(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditSubjectAsync(SubjectEntity item)
    {
        var entity = await _context.Subjects.FirstOrDefaultAsync(s => s.Id == item.Id);
        if (entity == null)
            return false;

        entity.Name = item.Name;
        _context.Subjects.Update(entity);
        _context.SaveChanges();
        return true;
    }

    public async Task<bool> RemoveSubjectAsync(int id)
    {
        await _context.Subjects.Where(s => s.Id == id).ExecuteDeleteAsync();
        return true;
    }

    public async Task<bool> UpdateAttachedUsersAsync(int subjectId, int[] usersIds)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);
        if (subject == null)
            return false;

        var attachedUsers = _context.PersonSubjectTable.Where(p => p.SubjectId == subjectId).Select(p => p.UserId).ToArray();
        var usersToRemove = attachedUsers.Where(id => !usersIds.Contains(id)).ToArray();
        var usersToAdd = usersIds.Where(id => !attachedUsers.Contains(id)).ToArray();

        foreach (var userId in usersToRemove)
        {
            var entity = _context.PersonSubjectTable.FirstOrDefault(e => e.SubjectId == subjectId && e.UserId == userId);
            if (entity != null)
                _context.PersonSubjectTable.Remove(entity);
        }

        foreach (var userId in usersToAdd)
        {
            _context.PersonSubjectTable.Add(new PersonSubjectTable() { SubjectId = subjectId, UserId = userId});
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAttachedTeachersAsync(int subjectId, int[] usersIds)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);
        if (subject == null)
            return false;

        var teachersIds = await _context.Teachers.Select(t => t.Id).ToArrayAsync();
        var attachedTeachers = _context.PersonSubjectTable.Where(p => p.SubjectId == subjectId && teachersIds.Contains(p.UserId)).Select(p => p.UserId).ToArray();
        var usersToRemove = attachedTeachers.Where(id => !usersIds.Contains(id)).ToArray();
        var usersToAdd = usersIds.Where(id => !attachedTeachers.Contains(id)).ToArray();

        foreach (var userId in usersToRemove)
        {
            var entity = _context.PersonSubjectTable.FirstOrDefault(e => e.SubjectId == subjectId && e.UserId == userId);
            if (entity != null)
                _context.PersonSubjectTable.Remove(entity);
        }

        foreach (var userId in usersToAdd)
        {
            _context.PersonSubjectTable.Add(new PersonSubjectTable() { SubjectId = subjectId, UserId = userId });
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateAttachedStudentsAsync(int subjectId, int[] usersIds)
    {
        var subject = _context.Subjects.FirstOrDefault(s => s.Id == subjectId);
        if (subject == null)
            return false;

        var studentsIds = await _context.Students.Select(t => t.Id).ToArrayAsync();
        var attachedStudents = _context.PersonSubjectTable.Where(p => p.SubjectId == subjectId && studentsIds.Contains(p.UserId)).Select(p => p.UserId).ToArray();
        var usersToRemove = attachedStudents.Where(id => !usersIds.Contains(id)).ToArray();
        var usersToAdd = usersIds.Where(id => !attachedStudents.Contains(id)).ToArray();

        foreach (var userId in usersToRemove)
        {
            var entity = _context.PersonSubjectTable.FirstOrDefault(e => e.SubjectId == subjectId && e.UserId == userId);
            if (entity != null)
                _context.PersonSubjectTable.Remove(entity);
        }

        foreach (var userId in usersToAdd)
        {
            _context.PersonSubjectTable.Add(new PersonSubjectTable() { SubjectId = subjectId, UserId = userId });
        }

        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> AttachPerson(int userId, int subjectId)
    {
        var relationship = await _context.PersonSubjectTable.FirstOrDefaultAsync(t => t.UserId == userId && t.SubjectId == subjectId);
        if (relationship != null)
            return false;
        await _context.PersonSubjectTable.AddAsync(new PersonSubjectTable() { UserId = userId, SubjectId = subjectId });
        _context.SaveChanges();
        return true;
    }

    public async Task<bool> DetachPersonAsync(int userId, int subjectId)
    {
        var relationship = await _context.PersonSubjectTable.FirstOrDefaultAsync(t => t.UserId == userId && t.SubjectId == subjectId);
        if (relationship == null)
            return false;
        _context.PersonSubjectTable.Remove(relationship);
        _context.SaveChanges();
        return true;
    }
}