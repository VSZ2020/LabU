using LabU.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    [Table("tasks")]
    public class TaskEntity : BaseEntity
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
        /// Перечень прикрепленных пользователей
        /// </summary>
        public List<BasePersonEntity>? Users { get; set; }
        
        public List<TaskPersonTable>? TaskPersonTable { get; set; }

        #region Subject
        public int SubjectId { get; set; }

        /// <summary>
        /// Дисциплина, к которой привязано задание
        /// </summary>
        [ForeignKey(nameof(SubjectId))]
        public SubjectEntity? Subject { get; set; }
        #endregion

        /// <summary>
        /// Перечень ответов на задание
        /// </summary>
        public List<TaskResponseEntity>? Responses { get; set; }
    }
}
