namespace LinqExtensions
{
    public static class EnumerableExtensions
    {
        public static bool SequenceEqual<T>(this IEnumerable<T> item1, IEnumerable<T>
        item2)
        {
            if (item1 == item2)
                return true;
            else if (item1 == null || item2 == null)
                return false;
            return item1.OrderBy(p => p).SequenceEqual(item2.OrderBy(p => p));
        }
    }
}