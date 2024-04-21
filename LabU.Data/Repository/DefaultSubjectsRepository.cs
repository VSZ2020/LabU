using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;

namespace LabU.Data.Repository;

public class DefaultSubjectsRepository: ISubjectRepository, IDisposable
{
    public DefaultSubjectsRepository(DataContext ctx)
    {
        _context = ctx;
    }

    private readonly DataContext _context;

    public async Task<IEnumerable<SubjectEntity>> GetAllAsync(int userId)
    {
        return await _context.Subjects
            .AsNoTracking()
            .Include(s => s.Users)
            .Where(s => s.Users.Any(u => u.Id == userId))
            .ToListAsync();
    }

    public async Task<IEnumerable<SubjectEntity>> GetAllAsync(
        Expression<Func<SubjectEntity, bool>>? filter = null,
        Func<IQueryable<SubjectEntity>, IOrderedQueryable<SubjectEntity>>? orderBy = null,
        string? includeProps = null)
    {
        var subjects = _context.Subjects.AsNoTracking();

        if (filter != null)
            subjects = subjects.Where(filter);

        if (!string.IsNullOrEmpty(includeProps))
        {
            foreach (var prop in includeProps.Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                subjects = subjects.Include(prop);
            }
        }

        return orderBy != null ? await orderBy(subjects).ToListAsync() : await subjects.ToListAsync();
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

    public bool AddSubject(SubjectEntity item)
    {
        _context.Subjects.Add(item);
        return true;
    }

    public bool EditSubject(SubjectEntity item)
    {
        _context.Subjects.Attach(item);
        _context.Subjects.Update(item);
        return true;
    }

    public bool RemoveSubject(SubjectEntity item)
    {
        _context.Subjects.Remove(item);
        return true;
    }

    public bool AttachUsers(int subjectId, int[] usersIds)
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

        return true;
    }


    public async Task<bool> AttachPerson(int userId, int subjectId)
    {
        var relationship = await _context.PersonSubjectTable.FirstOrDefaultAsync(t => t.UserId == userId && t.SubjectId == subjectId);
        if (relationship != null)
            return false;
        await _context.PersonSubjectTable.AddAsync(new PersonSubjectTable() { UserId = userId, SubjectId = subjectId });
        return true;
    }

    public async Task<bool> DetachPersonAsync(int userId, int subjectId)
    {
        var relationship = await _context.PersonSubjectTable.FirstOrDefaultAsync(t => t.UserId == userId && t.SubjectId == subjectId);
        if (relationship == null)
            return false;
        _context.PersonSubjectTable.Remove(relationship);
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