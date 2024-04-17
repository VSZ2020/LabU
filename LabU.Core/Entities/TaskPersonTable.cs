namespace LabU.Core.Entities;

public class TaskPersonTable
{
    public int UserId { get; set; }
    public BasePersonEntity? Person { get; set; }
    
    public int TaskId { get; set; }
    public TaskEntity? Task { get; set; }
}