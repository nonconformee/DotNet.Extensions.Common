
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace nonconformee.DotNet.Extensions.Reflection;

/// <summary>
/// Provides extension methods for <see cref="Delegate"/>.
/// </summary>
public static class DelegateExtensions
{
    /// <summary>
    /// Dynamically invokes the method represented by the current delegate, with the specified arguments, and unwraps any <see cref="TargetInvocationException"/> to throw the inner exception directly.
    /// </summary>
    /// <param name="del">The delegate to invoke. Cannot be <see langword="null"/>.</param>
    /// <param name="args">An array of objects that are the arguments to pass to the method represented by the delegate. If the method represented by the delegate does not require arguments, <paramref name="args"/> should be <see langword="null"/> or an empty array.</param>
    /// <returns>The object returned by the method represented by the delegate.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="del"/> is <see langword="null"/>.</exception>
    /// <exception cref="TargetParameterCountException">The number of elements in the <paramref name="args"/> array does not match the number of parameters expected by the method represented by the delegate.</exception>
    /// <exception cref="MethodAccessException">The caller does not have permission to access the method represented by the delegate.</exception>
    /// <exception cref="InvalidOperationException">The method represented by the delegate is not a static method, and the first element of <paramref name="args"/> does not contain a reference to a valid target object.</exception>
    /// <exception cref="ArgumentException">One or more elements of the <paramref name="args"/> array cannot be converted to the corresponding parameter type of the method represented by the delegate.</exception>
    /// <exception cref="TargetInvocationException">An error occurred while invoking the method represented by the delegate. The inner exception contains the actual exception thrown by the invoked method.</exception>
    public static object? DynamicInvokeUnwrapped(this Delegate del, params object?[]? args)
    {
        if (del is null) throw new ArgumentNullException(nameof(del));

        try
        {
            return del.DynamicInvoke(args);
        }
        catch (TargetInvocationException ex) when (ex.InnerException is not null)
        {
            ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
            throw;
        }
    }

    /// <summary>
    /// Combines two delegates of the same type into a single delegate, ensuring null safety.
    /// </summary>
    /// <remarks>This method is a null-safe wrapper around <see cref="Delegate.Combine(Delegate, Delegate)"/>.
    /// It ensures that combining a <see langword="null"/> delegate with another delegate does not throw an
    /// exception.</remarks>
    /// <typeparam name="TDelegate">The type of the delegate to combine.</typeparam>
    /// <param name="a">The first delegate to combine. Can be <see langword="null"/>.</param>
    /// <param name="b">The second delegate to combine. Can be <see langword="null"/>.</param>
    /// <returns>A delegate that represents the combined invocation list of the input delegates.  If both <paramref name="a"/>
    /// and <paramref name="b"/> are <see langword="null"/>, returns <see langword="null"/>. If only one delegate is
    /// non-<see langword="null"/>, returns the non-<see langword="null"/> delegate.</returns>
    public static TDelegate? CombineSafe<TDelegate>(this TDelegate? a, TDelegate? b) where TDelegate : Delegate
    {
        if (a is null) return b;
        if (b is null) return a;
        return (TDelegate)Delegate.Combine(a, b);
    }

    /// <summary>
    /// Removes the last occurrence of the specified delegate from the invocation list of the source delegate.
    /// </summary>
    /// <remarks>This method safely handles <see langword="null"/> values for both the source and value
    /// delegates.  If <paramref name="value"/> is <see langword="null"/>, the source delegate is returned
    /// unchanged.</remarks>
    /// <typeparam name="TDelegate">The type of the delegate.</typeparam>
    /// <param name="source">The source delegate from which to remove the specified delegate. Can be <see langword="null"/>.</param>
    /// <param name="value">The delegate to remove from the invocation list. Can be <see langword="null"/>.</param>
    /// <returns>A new delegate with the specified delegate removed from the invocation list, or the original delegate  if the
    /// specified delegate is not found. Returns <see langword="null"/> if the source delegate is <see
    /// langword="null"/>.</returns>
    public static TDelegate? RemoveSafe<TDelegate>(this TDelegate? source, TDelegate? value) where TDelegate : Delegate
    {
        if (source is null) return null;
        if (value is null) return source;
        return (TDelegate?)Delegate.Remove(source, value);
    }

    /// <summary>
    /// Converts the result of a delegate invocation into a <see cref="Task"/>.
    /// </summary>
    /// <param name="delegate">The delegate to invoke. The delegate must return a <see cref="Task"/>, <see cref="ValueTask"/>, or <see
    /// langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> representing the result of the delegate invocation.  If the delegate returns a <see
    /// cref="ValueTask"/>, it is converted to a <see cref="Task"/>. If the delegate returns <see langword="null"/>, a
    /// completed <see cref="Task"/> is returned.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="delegate"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the delegate does not return a <see cref="Task"/>, <see cref="ValueTask"/>, or <see langword="null"/>.</exception>
    public static Task AsTask(Delegate @delegate)
    {
        if (@delegate is null) throw new ArgumentNullException(nameof(@delegate));

        return @delegate.DynamicInvokeUnwrapped() switch
        {
            Task task => task,
            ValueTask valueTask => valueTask.AsTask(),
            null => Task.CompletedTask,
            _ => throw new InvalidOperationException("The delegate did not return a Task or ValueTask."),
        };
    }

    /// <summary>
    /// Converts the result of a delegate invocation into a <see cref="Task"/>.
    /// </summary>
    /// <param name="expectedReturnType">The type of the return type expected by the delegate.</typeparam>
    /// <param name="delegate">The delegate to invoke. The delegate must return a <see cref="Task"/>, <see cref="ValueTask"/>, or <see
    /// langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> representing the result of the delegate invocation.  If the delegate returns a <see
    /// cref="ValueTask"/>, it is converted to a <see cref="Task"/>. If the delegate returns <see langword="null"/>, a
    /// completed <see cref="Task"/> is returned.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="delegate"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the delegate does not return a <see cref="Task"/>, <see cref="ValueTask"/>, or <see langword="null"/>.</exception>
    public static Task<object> AsTask(Delegate @delegate, Type? expectedReturnType)
    {
        if (@delegate is null) throw new ArgumentNullException(nameof(@delegate));

        return @delegate.DynamicInvokeUnwrapped() switch
        {
            Task task => task,
            ValueTask valueTask => valueTask.AsTask(),
            null => Task.CompletedTask,
            _ => throw new InvalidOperationException("The delegate did not return a Task or ValueTask."),
        };
    }

    /// <summary>
    /// Converts the result of a delegate invocation into a <see cref="Task"/>.
    /// </summary>
    /// <typeparam name="T">The type of the return type expected by the delegate.</typeparam>
    /// <param name="delegate">The delegate to invoke. The delegate must return a <see cref="Task"/>, <see cref="ValueTask"/>, or <see
    /// langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> representing the result of the delegate invocation.  If the delegate returns a <see
    /// cref="ValueTask"/>, it is converted to a <see cref="Task"/>. If the delegate returns <see langword="null"/>, a
    /// completed <see cref="Task"/> is returned.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="delegate"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the delegate does not return a <see cref="Task"/>, <see cref="ValueTask"/>, or <see langword="null"/>.</exception>
    public static Task<T> AsTask<T>(Delegate @delegate)
        => (T)AsTask(@delegate, typeof(T));
}
