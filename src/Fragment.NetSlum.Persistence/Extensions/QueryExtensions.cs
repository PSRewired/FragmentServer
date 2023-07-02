using System.Collections.Generic;
using System.Linq;

namespace Fragment.NetSlum.Persistence.Extensions;

public static class QueryExtensions
{
    public static IEnumerable<TSource> Paginate<TSource>(this IEnumerable<TSource> query, int page, int pageSize)
    {
        return query
            .Skip((page < 1 ? 0 : page - 1) * pageSize)
            .Take(pageSize);
    }

    public static IQueryable<TSource> Paginate<TSource>(this IQueryable<TSource> query, int page, int pageSize)
    {
        return query
            .Skip((page < 1 ? 0 : page - 1) * pageSize)
            .Take(pageSize);
    }
}
