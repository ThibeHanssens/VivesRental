namespace Vives.Services.Model.Extensions
{
    public static class PagingQueryExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, Paging paging)
        {
            if (paging.Offset < 0)
            {
                paging.Offset = 0;
            }

            if (paging.Limit <= 0)
            {
                paging.Limit = 1;
            }

            if (paging.Limit > 1000)
            {
                paging.Limit = 1000;
            }

            query = query
                .Skip(paging.Offset)
                .Take(paging.Limit);

            return query;
        }
    }
}
