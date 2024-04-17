namespace LabU.Core.Dto;

public class BasePersonDto: BaseDto
{
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string? Email { get; set; }

    public string FullName => $"{LastName} {FirstName ?? ""} {MiddleName ?? ""}";
    public string ShortName => $"{LastName ?? "-"} {(!string.IsNullOrEmpty(FirstName) ? FirstName[0] + '.' : "")}{(!string.IsNullOrEmpty(MiddleName) ? MiddleName[0] + '.' : "")}";
}

public class StudentDto: BasePersonDto
{
    /// <summary>
    /// Номер курса обучения
    /// </summary>
    public int Cource { get; set; }

    /// <summary>
    /// Идентификатор команды/бригады
    /// </summary>
    public int CommandId { get; set; }
    
    public int AcademicGroupId { get; set; }
}

public class TeacherDto : BasePersonDto
{
    /// <summary>
    /// Должность преподавателя
    /// </summary>
    public string? Function { get; set; }

    public string? Address { get; set; }
}