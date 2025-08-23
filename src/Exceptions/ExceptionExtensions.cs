
namespace nonconformee.DotNet.Extensions.Exceptions;

/// <summary>
/// Provides extension methods for <see cref="Exception"/>.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Retrieves the full message of an exception, including messages from all inner exceptions.
    /// </summary>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <returns>The full message.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullMessage(this Exception exception)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var message = exception.Message.Trim();
        var innerException = exception.InnerException;

        while (innerException != null)
        {
            message += $"{Environment.NewLine} --> {innerException.Message.Trim()}";
            innerException = innerException.InnerException;
        }

        return message.Trim();
    }

    /// <summary>
    /// Retrieves the full stack trace of an exception, including stack traces from all inner exceptions.
    /// </summary>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <returns>The full stack trace.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullStackTrace(this Exception exception)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var stackTrace = exception.StackTrace?.Trim();
        var innerException = exception.InnerException;

        if (string.IsNullOrEmpty(stackTrace))
        {
            return "(no stack trace available)";
        }

        while (innerException != null)
        {
            var current = innerException.StackTrace?.Trim();

            if (string.IsNullOrEmpty(current))
            {
                current = "(no stack trace available)";
            }

            stackTrace += $"{Environment.NewLine}Inner Exception:{Environment.NewLine}{current}";
            innerException = innerException.InnerException;
        }

        return stackTrace.Trim();
    }

    /// <summary>
    /// Retrieves the full details of an exception, including its type, message, and stack trace.
    /// </summary>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <returns>The full type, message, and stack trace.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullDetails(this Exception exception)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        return $"{exception.GetType().Name}:{Environment.NewLine}{exception.GetFullMessage()}{Environment.NewLine}Stack Trace:{Environment.NewLine}{exception.GetFullStackTrace()}";
    }

    /// <summary>
    /// Retrieves a list of all exceptions in the chain, starting from the provided exception and including all inner exceptions.
    /// </summary>
    /// <remarks>
    /// This method retrieves all exceptions in the chain, starting from the provided exception and including all inner exceptions.
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <returns>A list of all exceptions in the chain.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static List<Exception> GetAllExceptions(this Exception exception)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var exceptions = new List<Exception> { exception };
        var innerException = exception.InnerException;

        while (innerException != null)
        {
            exceptions.Add(innerException);
            innerException = innerException.InnerException;
        }

        return exceptions;
    }
}
