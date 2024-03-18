using Forum.DomainModels;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Froum.Infrastructure
{
    public static class EFCoreExtensions
    {
        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="builder"></param>
        public static void EnableSoftDeletionGlobalFilter(this ModelBuilder builder)
        {

            var hasSofts = builder.Model.GetEntityTypes().Where(p => p.ClrType.IsAssignableFrom(typeof(ISoftDelete)));
            foreach (var entityType in hasSofts)
            {
                var isDelete = entityType.FindProperty(nameof(ISoftDelete.IsDelete));
                var parameter = Expression.Parameter(entityType.ClrType, "p");
                var filter = Expression.Lambda
                    (Expression.Not(Expression.Property(parameter, isDelete.PropertyInfo))
                    , parameter);
                entityType.SetQueryFilter(filter);
            }
        }
        public static IQueryable<T> Query<T>(this DbContext ctx) where T : class, IEntity
        {
            return ctx.Set<T>().AsNoTracking();
        }
    }
}
