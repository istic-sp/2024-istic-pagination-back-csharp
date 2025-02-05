using ISTIC.Pagination.Core;
using ISTIC.Pagination.Enums;
using System.Linq.Expressions;
using System.Reflection;

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

    public static IQueryable<T> PaginateBy<T>(this IQueryable<T> query, PageRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SortingProperty))
        {
            var orderByExpression = CreateNestedPropertyExpression<T>(request.SortingProperty);

            query = request.SortDirection == SortDirection.Ascending
                ? Queryable.OrderBy(query, (dynamic)orderByExpression)
                : Queryable.OrderByDescending(query, (dynamic)orderByExpression);
        }

        return query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
    }

    private static LambdaExpression CreateNestedPropertyExpression<T>(string propertyPath)
    {
        var properties = propertyPath.Split('.');

        var parameter = Expression.Parameter(typeof(T), "x");
        Expression propertyAccess = parameter;

        foreach (var property in properties)
        {
            var propertyInfo = propertyAccess.Type.GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
                throw new ArgumentException($"Property {property} not found in {propertyAccess.Type.FullName}");

            propertyAccess = Expression.Property(propertyAccess, propertyInfo);
        }

        return Expression.Lambda(propertyAccess, parameter);
    }
}
