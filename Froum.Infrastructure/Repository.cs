using EntityFrameworkCore;
using Forum.DomainModels;
using Microsoft.EntityFrameworkCore;
namespace Froum.Infrastructure
{
    /// <summary>
    /// id默认使用Guid
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T?> GetAsync(Guid id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task InserAsync(T entity);
        Task<List<T>> GetListAsync();
        Task<IQueryable<T>> GetQueryableAsync();
    }
    public interface IEfCoreRepository<T> where T : BaseEntity
    {
    }
    public class EfCoreRepository<T, TDbContext> : IEfCoreRepository<T> where T :
    BaseEntity where TDbContext : DbContext
    {
        protected TDbContext dbContext { get; init; }
        public EfCoreRepository(TDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public virtual async Task<T?> GetAsync(Guid id)
        {
            return await (dbContext.Set<T>().FirstOrDefaultAsync(p => p.Id == id));
        }
        public virtual async Task UpdateAsync(T entity)
        {
            await Task.Run(() => { dbContext.Update(entity); });
        }
        public virtual async Task DeleteAsync(T entity)
        {
            await Task.Run(async () => {
                entity.SoftDelete(); await
            UpdateAsync(entity);
            });
        }
        public virtual async Task InserAsync(T entity)
        {
            await dbContext.AddAsync(entity);
        }
        public virtual async Task<List<T>> GetListAsync()
        {
            return await dbContext.Query<T>().ToListAsync();
        }
        public virtual async Task<IQueryable<T>> GetQueryableAsync()
        {
            return await Task.Run<IQueryable<T>>(() => {
                return
            dbContext.Query<T>();
            });
        }
    }
}