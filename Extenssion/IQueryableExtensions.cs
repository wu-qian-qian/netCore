using Forum.DomainModels;
using System.Linq;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace LinqExtensions
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ToPageBySortDesc<T, T2>(this IQueryable<T>
        source, IPages page, Expression<Func<T, T2>> expression)
        {
            source = source.OrderByDescending(expression);
            return source.ToPage(page);
        }
        public static IQueryable<T> ToPage<T>(this IQueryable<T> source, IPages page)
        {
            return source.Skip(page.Count).Take(page.MaxCount);
        }
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> source, bool
        isOk, Expression<Func<T, bool>> ex)
        {
            return isOk ? source.Where(ex) : source;
        }
    }
}