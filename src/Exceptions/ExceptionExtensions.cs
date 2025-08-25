
using System.Globalization;
using System.Text;

namespace nonconformee.DotNet.Extensions.Exceptions;

/// <summary>
/// Provides extension methods for <see cref="Exception"/>.
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Retrieves the full message of an exception, including messages from all inner exceptions.
    /// </summary>
    /// <remarks>
    /// This method is intended to provide human-readyble text describing exceptions (e.g. for debugging or logging).
    /// If separators are added (<paramref name="addSeparator"/> is <see langword="true"/>) the string <c>--- inner exception ---</c> is added between the messages of each exception.
    /// The output starts from <paramref name="exception"/> to the innermost exception (through <see cref="Exception.InnerException"/>).
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <param name="addSeparator">Specifies whether a separator shhall be inserted between exception messages. Default value is <see langword="true"/>.</param>
    /// <returns>The full message.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullMessage(this Exception exception, bool addSeparator = true)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var sb = new StringBuilder();

        exception.GetAllExceptions().ForEach(e =>
        {
            if (addSeparator && sb.Length > 0)
            {
                sb.AppendLine("--- inner exception ---");
            }

            sb.AppendLine(e.Message);
        });

        return sb.ToString().Trim();

    }

    /// <summary>
    /// Retrieves the full stack trace of an exception, including stack traces from all inner exceptions.
    /// </summary>
    /// <remarks>
    /// This method is intended to provide human-readyble text describing exceptions (e.g. for debugging or logging).
    /// For exceptions for which no stack trace is available, <c>(no stack trace available)</c> is inserted.
    /// If separators are added (<paramref name="addSeparator"/> is <see langword="true"/>) the string <c>--- inner exception ---</c> is added between the stack traces of each exception.
    /// The output starts from <paramref name="exception"/> to the innermost exception (through <see cref="Exception.InnerException"/>).
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <param name="addSeparator">Specifies whether a separator shhall be inserted between exception messages. Default value is <see langword="true"/>.</param>
    /// <returns>The full stack trace.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullStackTrace(this Exception exception, bool addSeparator = true)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var sb = new StringBuilder();

        exception.GetAllExceptions().ForEach(e =>
        {
            if(addSeparator && sb.Length > 0)
            {
                sb.AppendLine("--- inner exception ---");
            }

            var stackTrace = e.StackTrace;

            if(string.IsNullOrEmpty(stackTrace))
            {
                sb.AppendLine("(no stack trace available)");
            }
            else
            {
                sb.AppendLine(stackTrace);
            }
        });

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Retrieves the full details of an exception, including from all inner exceptions.
    /// </summary>
    /// <remarks>
    /// This method is intended to provide human-readyble text describing exceptions (e.g. for debugging or logging).
    /// If separators are added (<paramref name="addSeparator"/> is <see langword="true"/>) the string <c>--- inner exception ---</c> is added between the messages of each exception.
    /// The output starts from <paramref name="exception"/> to the innermost exception (through <see cref="Exception.InnerException"/>).
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <param name="addSeparator">Specifies whether a separator shhall be inserted between exception messages. Default value is <see langword="true"/>.</param>
    /// <returns>The full details.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullDetails(this Exception exception, bool addSeparator = true)
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var sb = new StringBuilder();

        exception.GetAllExceptions().ForEach(e =>
        {
            if (addSeparator && sb.Length > 0)
            {
                sb.AppendLine("--- inner exception ---");
            }

            var type = e.GetType().Name;
            var message = e.Message;
            var targetSite = e.TargetSite?.Name ?? "(unknown)";
            var source = e.Source ?? "(unknown)";
            var hresult = e.HResult.ToString(CultureInfo.InvariantCulture);
            var stackTrace = e.StackTrace ?? "(no stack trace available)";

            sb.AppendLine($"Type: {type}");
            sb.AppendLine($"Message: {message}");
            sb.AppendLine($"TargetSite: {targetSite}");
            sb.AppendLine($"Source: {source}");
            sb.AppendLine($"HResult: {hresult}");
            sb.AppendLine($"StackTrace:");
            sb.AppendLine($"{stackTrace}");
        });

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Retrieves a list of all exceptions in the chain, starting from the provided exception and including all inner exceptions.
    /// </summary>
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
