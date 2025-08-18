
namespace nonconformee.DotNet.Extensions.Numbers;

/// <summary>
/// Provides extension methods for <see langword="float"/> (<see cref="Single"/>).
/// </summary>
public static class SingleExtensions
{
    /// <summary>
    /// Determines whether the specified single-precision floating-point value is not a number (NaN).
    /// </summary>
    /// <remarks>This method is an extension method for the <see cref="float"/> type and provides a
    /// convenient way to check if a value is NaN. A value is considered NaN if it does not represent a valid numeric
    /// value.</remarks>
    /// <param name="value">The single-precision floating-point value to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is not a number (NaN); otherwise, <see langword="false"/>.</returns>
    public static bool IsNaN(this float value)
        => float.IsNaN(value);

    /// <summary>
    /// Determines whether the specified single-precision floating-point number evaluates to positive or negative
    /// infinity.
    /// </summary>
    /// <remarks>This method is a convenience extension for <see cref="float.IsInfinity(float)"/>.</remarks>
    /// <param name="value">The single-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is positive infinity or negative infinity; otherwise, <see
    /// langword="false"/>.</returns>
    public static bool IsInfinity(this float value)
        => float.IsInfinity(value);

    /// <summary>
    /// Determines whether the specified single-precision floating-point number is a finite value.
    /// </summary>
    /// <param name="value">The single-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a finite number; otherwise, <see langword="false"/>.</returns>
    public static bool IsFinite(this float value)
        => !float.IsNaN(value) && !float.IsInfinity(value);

    /// <summary>
    /// Determines whether the specified single-precision floating-point number evaluates to negative infinity.
    /// </summary>
    /// <param name="value">The single-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is negative infinity; otherwise, <see langword="false"/>.</returns>
    public static bool IsNegativeInfinity(this float value)
        => float.IsNegativeInfinity(value);

    /// <summary>
    /// Determines whether the specified single-precision floating-point number evaluates to positive infinity.
    /// </summary>
    /// <param name="value">The single-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is positive infinity; otherwise, <see langword="false"/>.</returns>
    public static bool IsPositiveInfinity(this float value)
        => float.IsPositiveInfinity(value);
}
