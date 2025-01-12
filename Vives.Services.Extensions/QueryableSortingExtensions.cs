using System.Linq.Expressions;
using Vives.Services.Model;

namespace Vives.Services.Extensions;

/// <summary>
/// Simplified code using ChatGPT
/// </summary>
public static class QueryableSortingExpressions
{
    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, Sorting? sorting)
        where T : class
    {
        if (sorting is null)
        {
            return (IOrderedQueryable<T>)source;
        }

        var sortString = sorting.ToOrderQueryString();
        return source.OrderBy(sortString);
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, Sorting? sorting)
        where T : class
    {
        if (sorting is null)
        {
            return (IOrderedQueryable<T>)source;
        }

        var sortString = sorting.ToOrderQueryString();
        return source.ThenBy(sortString);
    }

    public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string? sortString)
        where T : class
    {
        if (string.IsNullOrWhiteSpace(sortString))
        {
            // If the sort string is empty or null, return the original IQueryable
            return (IOrderedQueryable<T>)source;
        }

        // Split the sorting string by commas
        var sortingPairs = SortingPairs(sortString);

        // Create the initial expression for OrderBy or OrderByDescending
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sortingPair in sortingPairs)
        {
            // Get the property information for the sorting property
            var propertyInfo = typeof(T).GetProperty(sortingPair.PropertyName);
            if (propertyInfo == null)
            {
                // Property not found, skip it
                continue;
            }

            // Create the lambda expression for the sorting property
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyExpression = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(propertyExpression, parameter);

            // Apply the sorting based on whether it is Ascending or Descending
            if (orderedQuery == null)
            {
                orderedQuery = sortingPair.IsDescending
                    ? Queryable.OrderByDescending(source, (dynamic)lambda)
                    : Queryable.OrderBy(source, (dynamic)lambda);
            }
            else
            {
                orderedQuery = sortingPair.IsDescending
                    ? Queryable.ThenByDescending(orderedQuery, (dynamic)lambda)
                    : Queryable.ThenBy(orderedQuery, (dynamic)lambda);
            }
        }

        return orderedQuery ?? (IOrderedQueryable<T>)source;
    }

    public static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string? sortString)
       where T : class
    {
        if (string.IsNullOrWhiteSpace(sortString))
        {
            // If the sort string is empty or null, return the original IQueryable
            return source;
        }

        // Split the sorting string by commas
        var sortingPairs = SortingPairs(sortString);

        // Create the initial expression for OrderBy or OrderByDescending
        IOrderedQueryable<T>? orderedQuery = null;

        foreach (var sortingPair in sortingPairs)
        {
            // Get the property information for the sorting property
            var propertyInfo = typeof(T).GetProperty(sortingPair.PropertyName);
            if (propertyInfo == null)
            {
                // Property not found, skip it
                continue;
            }

            // Create the lambda expression for the sorting property
            var parameter = Expression.Parameter(typeof(T), "x");
            var propertyExpression = Expression.Property(parameter, propertyInfo);
            var lambda = Expression.Lambda(propertyExpression, parameter);
            

            orderedQuery = sortingPair.IsDescending
                ? Queryable.ThenByDescending(orderedQuery, (dynamic)lambda)
                : Queryable.ThenBy(orderedQuery, (dynamic)lambda);

        }

        return orderedQuery ?? source;
    }

    private static IEnumerable<Sorting> SortingPairs(string sortString)
    {
        var sortingPairs = sortString.Split(',')
            .Select(part => part.Trim())
            .Select(part => new Sorting
            {
                PropertyName = part.EndsWith(" desc", StringComparison.OrdinalIgnoreCase)
                    ? part.Substring(0, part.Length - 5).Trim()
                    : part.EndsWith(" asc", StringComparison.OrdinalIgnoreCase)
                        ? part.Substring(0, part.Length - 4).Trim()
                        : part.Trim(),
                IsDescending = part.EndsWith(" desc", StringComparison.OrdinalIgnoreCase)
            });
        return sortingPairs;
    }


}