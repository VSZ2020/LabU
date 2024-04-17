﻿using System.Linq.Expressions;
using LabU.Core.Dto;
using LabU.Core.Entities;

namespace LabU.Core.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync(int subjectId, int personId = 0);

        Task<IEnumerable<TaskEntity>> GetAllAsync(
            Expression<Func<TaskEntity,bool>>? filter = null, 
            Func<IQueryable<TaskEntity>,IOrderedQueryable<TaskEntity>>? orderBy = null,
            string? includeProps = "");
        
        Task<TaskEntity?> FindByIdAsync(int id);

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
    }
}
