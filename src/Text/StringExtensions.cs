
namespace nonconformee.DotNet.Extensions.Text;

public static class StringExtensions
{
    /// <summary>
    /// Returns <see langword="null"/> if the string is <see langword="null"/> or empty, otherwise returns the original string.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns><see langword="null"/> if the string is <see langword="null"/> or empty, otherwise returns the original string.</returns>
    public static string? NullIfNullOrEmpty(this string? value)
        => string.IsNullOrEmpty(value) ? null : value;

    /// <summary>
    /// Returns <see langword="null"/> if the string is <see langword="null"/> or consists only of whitespace, otherwise returns the original string.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns><see langword="null"/> if the string is <see langword="null"/> or consists only of whitespace, otherwise returns the original string.</returns>
    public static string? NullIfNullOrWhiteSpace(this string? value)
        => string.IsNullOrWhiteSpace(value) ? null : value;

    /// <summary>
    /// Returns an empty string if the string is <see langword="null"/> or empty, otherwise returns the original string.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>An empty string if the string is <see langword="null"/> or empty, otherwise returns the original string.</returns>
    public static string EmptyIfNullOrEmpty(this string? value)
        => string.IsNullOrEmpty(value) ? string.Empty : value;

    /// <summary>
    /// Returns an empty string if the string is <see langword="null"/> or consists only of whitespace, otherwise returns the original string.
    /// </summary>
    /// <param name="value">The string to test.</param>
    /// <returns>An empty string if the string is <see langword="null"/> or consists only of whitespace, otherwise returns the original string.</returns>
    public static string EmptyIfNullOrWhiteSpace(this string? value)
        => string.IsNullOrWhiteSpace(value) ? string.Empty : value;

    /// <summary>
    /// Reverses the characters in the string.
    /// </summary>
    /// <param name="value">The string. Cannot be <see langword="null"/>.</param>
    /// <returns>The reversed string.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    public static string Reverse(this string value)
    {
        if (value is null) throw new ArgumentNullException(nameof(value));

        char[] charArray = value.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
}
