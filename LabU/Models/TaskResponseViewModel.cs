using System.ComponentModel.DataAnnotations;

namespace LabU.Models;

public class TaskResponseViewModel
{
    public int Id { get; set; }

    /// <summary>
    /// Дата отправки ответа
    /// </summary>
    [Display(Name = "Дата отправки")]
    [DataType(DataType.DateTime)]
    public DateTime SubmissionDate { get; set; }

    /// <summary>
    /// Комментарий или сообщение к ответу
    /// </summary>
    [Display(Name = "Примечание")]
    public string? Comment { get; set; }

    /// <summary>
    /// Вложение к ответу на задание
    /// </summary>
    [Display(Name = "Вложение")]
    [MaxLength(100, ErrorMessage = "Название файла не должно превышать 100 символов")]
    public string? AttachmentName { get; set; }
    
    public int TaskId { get; set; }
    [Display(Name = "Название задания")]
    public string? TaskName { get; set; }
    
    public int SenderId { get; set; }
    [Display(Name = "Отправитель")]
    public string? SenderName { get; set; }

    public AttachmentViewModel? Attachment { get; set; }
}