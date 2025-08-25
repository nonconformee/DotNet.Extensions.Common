
namespace nonconformee.DotNet.Extensions.Numbers;

/// <summary>
/// Provides extension methods for <see langword="double"/> (<see cref="Double"/>).
/// </summary>
public static class DoubleExtensions
{
    /// <summary>
    /// Determines whether the specified double-precision floating-point value is not a number (NaN).
    /// </summary>
    /// <remarks>
    /// This method is an extension method for the <see cref="double"/> type and provides a convenient way to check if a value is NaN. A value is considered NaN if it does not represent a valid numeric value.
    /// Mirrors IEEE 754 semantics.
    /// </remarks>
    /// <param name="value">The double-precision floating-point value to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is not a number (NaN); otherwise, <see langword="false"/>.</returns>
    public static bool IsNaN(this double value)
        => double.IsNaN(value);

    /// <summary>
    /// Determines whether the specified double-precision floating-point number evaluates to positive or negative
    /// infinity.
    /// </summary>
    /// <remarks>
    /// This method is an extension method for the <see cref="double"/> type and provides a convenient way to check if a value is infinite (positive or negative).
    /// Mirrors IEEE 754 semantics.
    /// </remarks>
    /// <param name="value">The double-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is positive infinity or negative infinity; otherwise, <see
    /// langword="false"/>.</returns>
    public static bool IsInfinity(this double value)
        => double.IsInfinity(value);

    /// <summary>
    /// Determines whether the specified double-precision floating-point number is a finite value.
    /// </summary>
    /// <remarks>
    /// This method is an extension method for the <see cref="double"/> type and provides a convenient way to check if a value is finite (positive or negative).
    /// Mirrors IEEE 754 semantics.
    /// </remarks>
    /// <param name="value">The double-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is a finite number; otherwise, <see langword="false"/>.</returns>
    public static bool IsFinite(this double value)
        => !double.IsNaN(value) && !double.IsInfinity(value);

    /// <summary>
    /// Determines whether the specified double-precision floating-point number evaluates to negative infinity.
    /// </summary>
    /// <remarks>
    /// This method is an extension method for the <see cref="double"/> type and provides a convenient way to check if a value is negative infinite.
    /// Mirrors IEEE 754 semantics.
    /// </remarks>
    /// <param name="value">The double-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is negative infinity; otherwise, <see langword="false"/>.</returns>
    public static bool IsNegativeInfinity(this double value)
        => double.IsNegativeInfinity(value);

    /// <summary>
    /// Determines whether the specified double-precision floating-point number evaluates to positive infinity.
    /// </summary>
    /// <remarks>
    /// This method is an extension method for the <see cref="double"/> type and provides a convenient way to check if a value is positive infinite.
    /// Mirrors IEEE 754 semantics.
    /// </remarks>
    /// <param name="value">The double-precision floating-point number to evaluate.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> is positive infinity; otherwise, <see langword="false"/>.</returns>
    public static bool IsPositiveInfinity(this double value)
        => double.IsPositiveInfinity(value);
}
