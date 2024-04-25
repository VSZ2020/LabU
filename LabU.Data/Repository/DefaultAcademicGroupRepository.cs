using LabU.Core.Entities;
using LabU.Core.Entities.Identity;
using LabU.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class DefaultAcademicGroupRepository : BaseRepository, IAcademicGroupsRepository
    {
        public DefaultAcademicGroupRepository(DataContext context): base(context)
        {
           
        }

        public Task<IEnumerable<AcademicGroupEntity>> GetAllGroupsAsync(Expression<Func<AcademicGroupEntity, bool>>? filter = null, Func<IQueryable<AcademicGroupEntity>, IOrderedQueryable<AcademicGroupEntity>>? orderBy = null, string? includeProps = "")
        {
            return base.GetAllAsync(filter, orderBy, includeProps);
        }

        public async Task<bool> AddAsync(AcademicGroupEntity item)
        {
            await _context.AcademicGroups.AddAsync(item);
            return true;
        }

        public Task<bool> UpdateAsync(AcademicGroupEntity item)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> RemoveAsync(int id)
        {
            await _context.AcademicGroups.Where(e => e.Id == id).ExecuteDeleteAsync();
            return true;
        }

    }
}
