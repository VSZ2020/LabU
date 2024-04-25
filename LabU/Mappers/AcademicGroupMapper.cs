using LabU.Core.Entities;
using LabU.Models;

namespace LabU.Mappers
{
    public class AcademicGroupMapper
    {
        public static AcademicGroupViewModel Map(AcademicGroupEntity entity)
        {
            return new AcademicGroupViewModel()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }
    }
}
