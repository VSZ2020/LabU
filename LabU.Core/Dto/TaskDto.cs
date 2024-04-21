using System.ComponentModel.DataAnnotations;

namespace LabU.Core.Dto
{
    public class TaskDto: BaseDto
    {
        /// <summary>
        /// Название задания
        /// </summary>
        public string Name { get; set; } = "Задание без названия";

        /// <summary>
        /// Описание задания
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Текущий статус ответа на задание: Отправлено, На проверке, Отправлено на доработку, Зачтено и др.
        /// </summary>
        public ResponseState? Status { get; set; }

        /// <summary>
        /// Срок ответа на задание
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; }

        /// <summary>
        /// Доступность задания
        /// </summary>
        public bool IsAvailable { get; set; }

        /// <summary>
        /// Перечень проверяющих преподавателей
        /// </summary>
        public string? Reviewers { get; set; }

        /// <summary>
        /// Дисциплина, к которой привязано задание
        /// </summary>
        public string? Subject { get; set; }

        public int SubjectId { get; set; }

        /// <summary>
        /// Перечень студентов, которым назначено задание
        /// </summary>
        public string? Students { get; set; }
    }
}
