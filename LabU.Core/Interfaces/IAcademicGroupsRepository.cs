using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using System.Linq.Expressions;

namespace LabU.Core.Interfaces
{
    public interface IAcademicGroupsRepository
    {
        Task<IEnumerable<AcademicGroupEntity>> GetAllGroupsAsync(
            Expression<Func<AcademicGroupEntity, bool>>? filter = null,
            Func<IQueryable<AcademicGroupEntity>, IOrderedQueryable<AcademicGroupEntity>>? orderBy = null,
            string? includeProps = "");

        public Task<bool> AddAsync(AcademicGroupEntity item);
        public Task<bool> UpdateAsync(AcademicGroupEntity item);
        public Task<bool> RemoveAsync(int id);
    }
}
