
namespace nonconformee.DotNet.Extensions.Exceptions;

public static class ExceptionExtensions
{
    public static string GetFullMessage(this Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception), "Exception cannot be null.");
        }
        var message = exception.Message;
        var innerException = exception.InnerException;
        while (innerException != null)
        {
            message += $" --> {innerException.Message}";
            innerException = innerException.InnerException;
        }
        return message;
    }

    public static string GetFullStackTrace(this Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception), "Exception cannot be null.");
        }
        var stackTrace = exception.StackTrace;
        var innerException = exception.InnerException;
        while (innerException != null)
        {
            stackTrace += $"\nInner Exception: {innerException.StackTrace}";
            innerException = innerException.InnerException;
        }
        return stackTrace;
    }

    public static string GetFullDetails(this Exception exception)
    {
        if (exception == null)
        {
            throw new ArgumentNullException(nameof(exception), "Exception cannot be null.");
        }
        return $"{exception.GetType().Name}: {exception.GetFullMessage()}\nStack Trace:\n{exception.GetFullStackTrace()}";
    }
}
