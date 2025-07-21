
namespace nonconformee.DotNet.Extensions.Exceptions;

public static class ParameterValidationHelpers
{
    public static void ThrowIfIndexArgumentIsOutOfRange<T> (this ICollection<T> collection, int index, int? count = null)
    {
        if (index < 0 || index >= collection.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(index), $"Index {index} exceeds the collection size {collection.Count}.");
        }

        if(count.HasValue && (index + count.Value) > collection.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(count), $"Count {count.Value} exceeds the collection size {collection.Count} from index {index}.");
        }
    }
}
