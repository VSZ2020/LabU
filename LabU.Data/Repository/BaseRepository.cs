using LabU.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LabU.Data.Repository
{
    public class BaseRepository: IDisposable
    {
        public BaseRepository(DataContext context)
        {
            _context = context;
        }

        protected readonly DataContext _context;


        //Source: https://learn.microsoft.com/ru-ru/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application
        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>>? filter = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null, string? includeProps = "")
            where T: BaseEntity
        {
            IQueryable<T> items = _context.Set<T>();

            if (filter != null)
                items = items.Where(filter);

            if (!string.IsNullOrEmpty(includeProps))
            {
                foreach (var prop in includeProps.Split(',', StringSplitOptions.RemoveEmptyEntries))
                {
                    items = items.Include(prop);
                }
            }

            return orderBy != null ? await orderBy(items).ToListAsync() : await items.ToListAsync();
        }



        #region IDisposable
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion
    }
}
