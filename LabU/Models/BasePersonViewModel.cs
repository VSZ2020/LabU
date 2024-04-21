using System.ComponentModel.DataAnnotations;

namespace LabU.Models;

public class BasePersonViewModel
{
    public int Id { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? Email { get; set; }

    public string FullName => $"{LastName} {FirstName ?? ""} {MiddleName ?? ""}";
    public string ShortName => $"{LastName ?? "-"} {(!string.IsNullOrEmpty(FirstName) ? FirstName[0] + '.' : "")}{(!string.IsNullOrEmpty(MiddleName) ? MiddleName[0] + '.' : "")}";
}

public class StudentViewModel : BasePersonViewModel
{
    /// <summary>
    /// Номер курса обучения
    /// </summary>
    [Display(Name = "Курс")]
    public int Cource { get; set; }

    /// <summary>
    /// Идентификатор команды/бригады
    /// </summary>
    [Display(Name = "Номер команды")]
    public int CommandId { get; set; }

    public int AcademicGroupId { get; set; }

    [Display(Name = "Группа")]
    public string? AcademicGroupName { get; set; }
}

public class TeacherViewModel : BasePersonViewModel
{
    /// <summary>
    /// Должность преподавателя
    /// </summary>
    [Display(Name = "Должность")]
    public string? Function { get; set; }

    [Display(Name = "Место работы")]
    public string? Address { get; set; }
}