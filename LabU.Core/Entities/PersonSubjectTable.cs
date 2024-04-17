namespace LabU.Core.Entities;

public class PersonSubjectTable
{
    public int SubjectId { get; set; }
    public SubjectEntity? Subject { get; set; }
    
    public int UserId { get; set; }
    public BasePersonEntity? Person { get; set; }
}