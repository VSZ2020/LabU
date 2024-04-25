using System.ComponentModel.DataAnnotations;

namespace LabU.Models;

public class BasePersonViewModel
{
    public int Id { get; set; }

    [Display(Name = "Фамилия")]
    public string? LastName { get; set; }

    [Display(Name = "Курс")]
    public string? FirstName { get; set; }

    [Display(Name = "Отчество")]
    public string? MiddleName { get; set; }

    [Display(Name = "E-mail")]
    public string? Email { get; set; }
    
    [Display(Name = "ФИО")]
    public string FullName => $"{LastName} {FirstName ?? ""} {MiddleName ?? ""}";

    [Display(Name = "ФИО")]
    public string ShortName => $"{LastName ?? "-"} {(!string.IsNullOrEmpty(FirstName) ? FirstName[0] + '.' : "")}{(!string.IsNullOrEmpty(MiddleName) ? MiddleName[0] + '.' : "")}";
}

public class StudentViewModel : BasePersonViewModel
{
    /// <summary>
    /// Номер курса обучения
    /// </summary>
    [Display(Name = "Курс обучения")]
    public int Cource { get; set; }

    /// <summary>
    /// Идентификатор команды/бригады
    /// </summary>
    [Display(Name = "Номер команды")]
    public int CommandId { get; set; }

    public int? AcademicGroupId { get; set; }

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