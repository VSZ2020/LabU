using System.ComponentModel.DataAnnotations;

namespace LabU.Models
{
    public class AddTaskResponseViewModel
    {
        /// <summary>
        /// Комментарий или сообщение к ответу
        /// </summary>
        [Display(Name = "Примечание")]
        public string? Comment { get; set; }

        /// <summary>
        /// Вложение к ответу на задание
        /// </summary>
        [Display(Name = "Вложение")]
        [StringLength(100, ErrorMessage = "Название файла не должно превышать 100 символов")]
        public string? AttachmentName { get; set; }

        public int TaskId { get; set; }
    }
}
