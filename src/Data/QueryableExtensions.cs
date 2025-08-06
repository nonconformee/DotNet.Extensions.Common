
namespace nonconformee.DotNet.Extensions.Data;

/// <summary>
/// Provides extension methods for <see cref="IQueryable{T}"/>.
/// </summary>
public static class QueryableExtensions
{
    /// <summary>
    /// Limits the query result to a specific page, if pagination is used.
    /// </summary>
    /// <typeparam name="T">The type of elements being queried.</typeparam>
    /// <param name="queryable">The query.</param>
    /// <param name="page">The zero-indexed page. <see langword="null"/> if pagination is not used.</param>
    /// <param name="pageSize">The size of the page, number of elements. <see langword="null"/> if pagination is not used.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Only <paramref name="page"/> or <paramref name="pageSize"/> is specified (not <see langword="null"/>) but not both.</exception>
    /// <remarks>For convenience, <paramref name="page"/> and <paramref name="pageSize"/> can be <see langword="null"/> if pagination is not used.
    /// This allows to use <see cref="Page"/> if pagination is optional without having a branch to build the corresponding query.</remarks>
    public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int? page, int? pageSize)
    {
        if (page is null && pageSize is not null)
        {
            throw new ArgumentException("Page cannot be null if page size is set.", nameof(page));
        }

        if (page is not null && pageSize is null)
        {
            throw new ArgumentException("Page size cannot be null if page is set.", nameof(pageSize));
        }

        if(pageSize < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(pageSize), "Page size cannot be less than zero."); 
        }

        if (page is null && pageSize is null)
        {
            return queryable;
        }

        if(pageSize == 0)
        {
            return queryable
                .Where(x => false);
        }

        var usedPageSize = pageSize!.Value;
        var usedPage = page!.Value * usedPageSize;

        return queryable
            .Skip(usedPage)
            .Take(usedPageSize);
    }
}
