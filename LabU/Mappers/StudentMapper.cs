using LabU.Core.Entities;
using LabU.Models;

namespace LabU.Mappers
{
    public class StudentMapper
    {
        public static StudentViewModel Map(StudentEntity entity)
        {
            return new StudentViewModel()
            {
                Id = entity.Id,
                MiddleName = entity.MiddleName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                CommandId = entity.CommandId,
                Cource = entity.Course,
                AcademicGroupId = entity.AcademicGroupId,
                AcademicGroupName = entity.AcademicGroup?.Name ?? "-",
            };
        }
    }
}
