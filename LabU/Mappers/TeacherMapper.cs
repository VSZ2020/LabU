using LabU.Core.Entities;
using LabU.Models;

namespace LabU.Mappers
{
    public class TeacherMapper
    {
        public static TeacherViewModel Map(TeacherEntity entity)
        {
            return new TeacherViewModel()
            {
                Id = entity.Id,
                MiddleName = entity.MiddleName,
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                Function = entity.Function,
                Address = entity.Address,
            };
        }
    }
}
