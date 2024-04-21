using LabU.Core;
using System.ComponentModel.DataAnnotations;

namespace LabU.Models;

public class SubjectViewModel
{
    public int Id { get; set; }

    /// <summary>
    /// Название дисциплины
    /// </summary>
    [Display(Name = "Название")]
    public string Name { get; set; }

    /// <summary>
    /// Описание дисциплины
    /// </summary>
    [Display(Name = "Описание")]
    public string? Description { get; set; }

    /// <summary>
    /// Учебный год в формате "yyyy-yyyy". Пример: 2023-2024.
    /// </summary>
    [Display(Name = "Учебный год")]    
    public string? AcademicYear { get; set; }

    /// <summary>
    /// Тип учебного семестра - осенний или весенний
    /// </summary>
    [Display(Name = "Семестр")]
    public string? AcademicTerm { get; set; }

    /// <summary>
    /// Прикрепленные преподаватели
    /// </summary>
    [Display(Name = "Преподаватели")]
    public string? Teachers { get; set; }

    /// <summary>
    /// Студенты, прикрепленные к данной дисциплине
    /// </summary>
    [Display(Name = "Студенты")]
    public string? Students { get; set; }
}