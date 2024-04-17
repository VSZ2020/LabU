using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    /// <summary>
    /// Ответ на задание
    /// </summary>
    [Table("task_responses")]
    public class TaskResponseEntity : BaseEntity
    {
        /// <summary>
        /// Дата отправки ответа
        /// </summary>
        [DataType(DataType.DateTime)]
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Комментарий или сообщение к ответу
        /// </summary>
        public string? Comment { get; set; }

        /// <summary>
        /// Вложение к ответу на задание
        /// </summary>
        public ResponseAttachmentEntity? Attachment { get; set; }

        #region Task
        [Required]
        public int TaskId { get; set; }

        /// <summary>
        /// Идентификатор связанного задания, на который дан ответ
        /// </summary>
        [ForeignKey(nameof(TaskId))]
        public TaskEntity? Task { get; set; }
        #endregion

        #region Sender
        public int SenderId { get; set; }

        /// <summary>
        /// Отправитель
        /// </summary>
        [ForeignKey(nameof(SenderId))]
        public BasePersonEntity? Sender { get; set; }
        #endregion
    }
}
