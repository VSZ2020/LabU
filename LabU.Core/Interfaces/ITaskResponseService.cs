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
        Task<IEnumerable<TaskResponseEntity>> GetAllAsync(int taskId);

        bool AddResponse(TaskResponseEntity response);
        bool EditResponse(TaskResponseEntity response);
        bool RemoveResponse(TaskResponseEntity response);
    }
}
