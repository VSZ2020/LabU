using LabU.Core;
using System.ComponentModel.DataAnnotations.Schema;

namespace LabU.Core.Entities
{
    /// <summary>
    /// Вложение к ответу на задание
    /// </summary>
    [Table("responses_attachments")]
    public class ResponseAttachmentEntity : BaseEntity
    {
        /// <summary>
        /// Название файла
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Полный путь к месту хранения файла
        /// </summary>
        public string Path { get; set; }

        public FileType FileType { get; set; }

        /// <summary>
        /// Размер файла в байтах
        /// </summary>
        public long FileSizeBytes { get; set; }
        
        public int ResponseId { get; set; }
        
        [ForeignKey(nameof(ResponseId))]
        public TaskResponseEntity? TaskResponse { get; set; }
    }
}
