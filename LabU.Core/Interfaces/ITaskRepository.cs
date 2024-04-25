using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;

namespace LabU.Core.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetTasksAsync(int subjectId, int personId = 0);

        Task<IEnumerable<TaskEntity>> GetTasksAsync(
            Expression<Func<TaskEntity,bool>>? filter = null, 
            Func<IQueryable<TaskEntity>,IOrderedQueryable<TaskEntity>>? orderBy = null,
            string? includeProps = "");
        
        Task<TaskEntity?> FindByIdAsync(int id);
        Task<TaskEntity?> FindByIdAsync(int id, string? includeProps = null);

        /// <summary>
        /// Проверяет принадлежность задания пользователю
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        bool IsOwnedByUser(int taskId, int userId);

        Task<IEnumerable<TeacherEntity>> GetAttachedReviewersAsync(int taskId);
        Task<IEnumerable<StudentEntity>> GetAttachedStudentsAsync(int taskId);

        bool AddTask(TaskEntity task);
        bool EditTask(TaskEntity task);
        bool RemoveTask(TaskEntity task);

        Task<bool> AttachPerson(int userId, int taskId);
        Task<bool> DetachPersonAsync(int userId, int taskId);

        Task<bool> ChangeTaskStatus(int taskId, ResponseState newStatus);
    }
}
