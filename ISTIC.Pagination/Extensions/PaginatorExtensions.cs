using ISTIC.Pagination.Core;
using ISTIC.Pagination.Enums;
using System.Linq.Expressions;

namespace ISTIC.Pagination.Extensions;

public static class PaginatorExtensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PageRequest request)
    {
        return query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
    }

    public static IQueryable<T> PaginateBy<T, TKey>(this IQueryable<T> query, PageRequest request, Expression<Func<T, TKey>> sortBy)
    {
        query = request.SortDirection == SortDirection.Ascending
            ? query.OrderBy(sortBy)
            : query.OrderByDescending(sortBy);

        return query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
    }
}
