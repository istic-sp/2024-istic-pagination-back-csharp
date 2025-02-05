using ISTIC.Pagination.Core;
using ISTIC.Pagination.Enums;
using System.Linq.Expressions;
using System.Reflection;

namespace ISTIC.Pagination.Extensions;

public static class PaginatorExtensions
{
    /// <summary>
    /// Pagina a consulta com base na página e tamanho de página especificados.
    /// </summary>
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, PageRequest request)
    {
        return query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
    }

    /// <summary>
    /// Pagina e ordena a consulta com base na propriedade especificada.
    /// </summary>
    public static IQueryable<T> PaginateBy<T, TKey>(this IQueryable<T> query, PageRequest request, Expression<Func<T, TKey>> sortBy)
    {
        query = request.SortDirection == SortDirection.Ascending
            ? query.OrderBy(sortBy)
            : query.OrderByDescending(sortBy);

        return query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize);
    }

    /// <summary>
    /// Pagina e ordena a consulta de forma dinâmica com base na propriedade especificada.
    /// </summary>
    /// <remarks>
    /// ⚠ **Atenção:** A propriedade informada em <paramref name="SortingProperty"/> do objeto PageRequest deve ser **traduzível para SQL**.
    /// Caso contrário, ocorrerá um erro de execução do Entity Framework.
    /// </remarks>
    public static IQueryable<T> PaginateBy<T>(this IQueryable<T> query, PageRequest request)
    {
        if (!string.IsNullOrWhiteSpace(request.SortingProperty))
        {
            LambdaExpression lambdaExpression = CreateNestedPropertyExpression<T>(request.SortingProperty);

            Type propertyType = lambdaExpression.ReturnType;
            bool isNullable = Nullable.GetUnderlyingType(propertyType) != null || !propertyType.IsValueType;

            IOrderedQueryable<T> orderedQuery = request.SortDirection == SortDirection.Ascending
                ? Queryable.OrderBy(query, (dynamic)lambdaExpression)
                : Queryable.OrderByDescending(query, (dynamic)lambdaExpression);

            if (isNullable)
                orderedQuery = request.SortDirection == SortDirection.Ascending
                    ? Queryable.ThenBy(orderedQuery, (dynamic)lambdaExpression)
                    : Queryable.ThenByDescending(orderedQuery, (dynamic)lambdaExpression);

            return orderedQuery
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);
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
