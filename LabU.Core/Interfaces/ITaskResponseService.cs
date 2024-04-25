using LabU.Core.Dto;
using LabU.Core.Entities;

namespace LabU.Core.Interfaces
{
    public interface ITaskResponseService
    {
        /// <summary>
        /// Возвращает список всех ответов для указанного задания
        /// </summary>
        /// <param name="taskId">Идентификатор задания</param>
        /// <returns></returns>
        Task<IEnumerable<TaskResponseEntity>> GetTaskResponsesAsync(int taskId);

        Task<bool> AddResponseAsync(TaskResponseEntity response);
        Task<bool> EditResponseAsync(TaskResponseEntity response);
        Task<bool> RemoveResponseAsync(int responseId);
    }
}
