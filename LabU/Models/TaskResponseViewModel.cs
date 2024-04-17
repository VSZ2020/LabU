using System.ComponentModel.DataAnnotations;

namespace LabU.Models;

public class TaskResponseViewModel
{
    public int Id { get; set; }
    
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
    public string? AttachmentName { get; set; }
    
    public int TaskId { get; set; }
    public string? TaskName { get; set; }
    
    public int SenderId { get; set; }
    public string? SenderName { get; set; }
}