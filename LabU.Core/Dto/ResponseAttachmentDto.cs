namespace LabU.Core.Dto;

public class ResponseAttachmentDto: BaseDto
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
}