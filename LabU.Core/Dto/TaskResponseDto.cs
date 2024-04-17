using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LabU.Core.Entities;

namespace LabU.Core.Dto;

public class TaskResponseDto: BaseDto
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
    public ResponseAttachmentDto? Attachment { get; set; }
    
    public int TaskId { get; set; }
    public string? TaskName { get; set; }
    
    public int SenderId { get; set; }
    public string? SenderName { get; set; }
}