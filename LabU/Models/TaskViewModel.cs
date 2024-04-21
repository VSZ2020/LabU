using System.ComponentModel.DataAnnotations;
using LabU.Core;

namespace LabU.Models;

public class TaskViewModel
{
    public int Id { get; set; }

    /// <summary>
    /// Название задания
    /// </summary>
    [Display(Name = "Наименование")]
    public string Name { get; set; } = "Задание без названия";

    /// <summary>
    /// Описание задания
    /// </summary>
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Текущий статус ответа на задание: Отправлено, На проверке, Отправлено на доработку, Зачтено и др.
    /// </summary>
    [Display(Name = "Состояние")]
    public string? Status { get; set; }

    /// <summary>
    /// Срок ответа на задание
    /// </summary>
    [Display(Name = "Срок ответа")]
    [DataType(DataType.DateTime)]
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Доступность задания
    /// </summary>
    public bool IsAvailable { get; set; }

    /// <summary>
    /// Перечень проверяющих преподавателей
    /// </summary>
    [Display(Name = "Проверяющие")]
    public string? Reviewers { get; set; }

    /// <summary>
    /// Дисциплина, к которой привязано задание
    /// </summary>
    [Display(Name="Дисциплина")]
    public string? Subject { get; set; }

    public int SubjectId { get; set; }

    /// <summary>
    /// Перечень студентов, которым назначено задание
    /// </summary>
    [Display(Name="Состав команды")]
    public string? Students { get; set; }
}