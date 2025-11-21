
using nonconformee.DotNet.Extensions.Collections;
using System.Collections;
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
    /// The output starts from <paramref name="exception"/> to the innermost exception (through <see cref="Exception.InnerException"/>).
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <param name="separator">The optional separator to be inserted between exception messages. Default value is <c>--- inner exception---</c>. Use <see langword="null"/> to not insert separators.</param>
    /// <returns>The full message.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullMessage(this Exception exception, string? separator = "--- inner exception ---")
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var sb = new StringBuilder();

        exception.GetAllExceptions().ForEach(e =>
        {
            if ((separator is not null) && (sb.Length > 0))
            {
                sb.AppendLine(separator);
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
    /// The output starts from <paramref name="exception"/> to the innermost exception (through <see cref="Exception.InnerException"/>).
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <param name="separator">The optional separator to be inserted between exception stack traces. Default value is <c>--- inner exception---</c>. Use <see langword="null"/> to not insert separators.</param>
    /// <param name="noStackTrace">The optional text inserted as a stack trace entry if an exception does not provide its own stack trace. Default value is <c>(no stack trace available)</c>. Use <see langword="null"/> to omit exceptions without stack traces.</param>
    /// <returns>The full stack trace.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetFullStackTrace(this Exception exception, string? separator = "--- inner exception ---", string? noStackTrace = "(no stack trace available)")
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var sb = new StringBuilder();

        exception.GetAllExceptions().ForEach(e =>
        {
            if ((separator is not null) && (sb.Length > 0))
            {
                sb.AppendLine(separator);
            }

            var stackTrace = e.StackTrace;

            if(string.IsNullOrEmpty(stackTrace))
            {
                if(noStackTrace is not null)
                {
                    sb.AppendLine(noStackTrace);
                }
            }
            else
            {
                sb.AppendLine(stackTrace);
            }
        });

        return sb.ToString().Trim();
    }

    /// <summary>
    /// Creates a formatted string containing detailed information about the exception, including selected properties and optionally all inner exceptions.
    /// </summary>
    /// <remarks>
    /// This method is intended to provide human-readyble text describing exceptions (e.g. for debugging or logging).
    /// The output starts from <paramref name="exception"/> to the innermost exception (through <see cref="Exception.InnerException"/>).
    /// </remarks>
    /// <param name="exception">The exception. Cannot be <see langword="null"/>.</param>
    /// <param name="details">A bitwise combination of values that specifies which details to include in the output. The default is <see cref="ExceptionDetails.All"/>.</param>
    /// <param name="recursive"><see langword="true"/> to include details from all inner exceptions recursively, <see langword="false"/> otherwise. The default is <see langword="true"/>.</param>
    /// <param name="separator">The optional separator to be inserted between exception details. Default value is <c>--- inner exception---</c>. Use <see langword="null"/> to not insert separators.</param>
    /// <param name="emptyOrNullValue">The optional text used for values which are <see langword="null"/> or an empty string. Default value is <c>(nothing)</c>. Use <see langword="null"/> to omit empty or null values in the output.</param>
    /// <returns>The exception details.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="exception"/> is <see langword="null"/>.</exception>
    public static string GetDetails(this Exception exception, ExceptionDetails details = ExceptionDetails.All, bool recursive = true, string? separator = "--- inner exception ---", string? emptyOrNullValue = "(nothing)")
    {
        if (exception is null) throw new ArgumentNullException(nameof(exception));

        var sb = new StringBuilder();

        exception.GetAllExceptions().ForEach(e =>
        {
            if ((separator is not null) && (sb.Length > 0))
            {
                sb.AppendLine(separator);
            }

            var type = e.GetType();

            if (details.HasFlag(ExceptionDetails.TypeName))
            {
                var typeName = type.Name;
                if (!string.IsNullOrWhiteSpace(typeName))
                {
                    sb.AppendLine($"Type: {typeName}");
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"Type: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.TypeNamespace))
            {
                var typeNamespace = type.Namespace;
                if (!string.IsNullOrWhiteSpace(typeNamespace))
                {
                    sb.AppendLine($"Namespace: {typeNamespace}");
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"Namespace: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.TypeAssembly))
            {
                var typeAssembly = type.Assembly.FullName;
                if(!string.IsNullOrWhiteSpace(typeAssembly))
                {
                    sb.AppendLine($"Assembly: {typeAssembly}");
                }
                else if(emptyOrNullValue is not null)
                {
                    sb.AppendLine($"Assembly: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.Message))
            {
                var message = e.Message;
                if (!string.IsNullOrWhiteSpace(message))
                {
                    sb.AppendLine($"Message: {message}");
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"Message: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.HResult))
            {
                sb.AppendLine($"HResult: {e.HResult.ToString(CultureInfo.InvariantCulture)}");
            }

            if (details.HasFlag(ExceptionDetails.Source))
            {
                var source = e.Source;
                if (!string.IsNullOrWhiteSpace(source))
                {
                    sb.AppendLine($"Source: {source}");
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"Source: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.TargetSite))
            {
                var targetSite = e.TargetSite?.Name;
                if (!string.IsNullOrWhiteSpace(targetSite))
                {
                    sb.AppendLine($"TargetSite: {targetSite}");
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"TargetSite: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.HelpLink))
            {
                var helpLink = e.HelpLink;
                if (!string.IsNullOrWhiteSpace(helpLink))
                {
                    sb.AppendLine($"HelpLink: {helpLink}");
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"HelpLink: {emptyOrNullValue}");
                }
            }

            if (details.HasFlag(ExceptionDetails.Data) && e.Data is not null)
            {
                foreach(DictionaryEntry kv in e.Data)
                {
                    var key = kv.Key?.ToString();
                    var value = kv.Value?.ToString();

                    if(key is null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        sb.AppendLine($"Data: {key}: {value}");
                    }
                    else if (emptyOrNullValue is not null)
                    {
                        sb.AppendLine($"Data: {key}: {emptyOrNullValue}");
                    }
                }
            }

            if (details.HasFlag(ExceptionDetails.Properties))
            {
                var properties = type
                    .GetProperties()
                    .Where(x => x.DeclaringType != typeof(Exception));

                foreach (var property in properties)
                {
                    var name = property.Name;
                    var value = property.GetValue(e)?.ToString();

                    if (!string.IsNullOrWhiteSpace(value))
                    {
                        sb.AppendLine($"{name}: {value}");
                    }
                    else if (emptyOrNullValue is not null)
                    {
                        sb.AppendLine($"{name}: {emptyOrNullValue}");
                    }
                }
            }

            if (details.HasFlag(ExceptionDetails.StackTrace))
            {
                var stackTrace = e.StackTrace;
                if (!string.IsNullOrWhiteSpace(stackTrace))
                {
                    sb.AppendLine("StackTrace:");
                    sb.AppendLine(stackTrace);
                }
                else if (emptyOrNullValue is not null)
                {
                    sb.AppendLine($"StackTrace: {emptyOrNullValue}");
                }
            }
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
