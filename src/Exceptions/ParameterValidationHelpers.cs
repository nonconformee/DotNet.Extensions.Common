
using System.Runtime.CompilerServices;

namespace nonconformee.DotNet.Extensions.Exceptions;

public static class ParameterValidationHelpers
{
    public static void ThrowIfIndexArgumentIsOutOfRange<T> (this ICollection<T> collection, int index, int? count = null, [CallerArgumentExpression("collection")] string argExpr = null)
    {
        if (collection is null)
        {
            throw new ArgumentNullException(argExpr);
        }

        if (index < 0)
        {
            throw new ArgumentOutOfRangeException(argExpr, $"Index {index} cannot be negative.");
        }

        if (index >= collection.Count)
        {
            throw new ArgumentOutOfRangeException(argExpr, $"Index {index} exceeds the collection size {collection.Count}.");
        }

        if(count.HasValue && (index + count.Value) > collection.Count)
        {
            throw new ArgumentOutOfRangeException(argExpr, $"Count {count.Value} exceeds the collection size {collection.Count} from index {index}.");
        }
    }

    public static void ThrowIfArgumentIsOutOfRange (this int value, int min, int max)
    {
        if(value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(nameof(value), $"Value {value} is out of range. It must be between {min} and {max}.");
        }
    }
}
