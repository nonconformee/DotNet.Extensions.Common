
namespace nonconformee.DotNet.Extensions.Async;

/// <summary>
/// Provides extension methods for <see cref="Task"/> and <see cref="Task{T}"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Waits for the specified task to complete within the given timeout period.
    /// </summary>
    /// <remarks>If the task does not complete within the specified timeout, a <see cref="TimeoutException"/>
    /// is thrown. The original task is not canceled or modified in any way.</remarks>
    /// <param name="task">The task to wait for. Cannot be <see langword="null"/>.</param>
    /// <param name="timeout">The maximum amount of time to wait for the task to complete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the wait operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    /// <exception cref="TimeoutException">Thrown if the task does not complete within the specified <paramref name="timeout"/>.</exception>
    public static async Task WithTimeout(this Task task, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task)
        {
            throw new TimeoutException($"Task did not complete in {timeout}.");
        }

        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified task with a timeout, throwing a <see cref="TimeoutException"/> if the task does not
    /// complete within the specified time.
    /// </summary>
    /// <remarks>This method waits for the specified task to complete or for the timeout to elapse, whichever
    /// occurs first. If the timeout elapses before the task completes, a <see cref="TimeoutException"/> is
    /// thrown.</remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to execute with a timeout. Cannot be <see langword="null"/>.</param>
    /// <param name="timeout">The maximum amount of time to wait for the task to complete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the wait operation.</param>
    /// <returns>The result of the task if it completes within the specified timeout.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    /// <exception cref="TimeoutException">Thrown if the task does not complete within the specified <paramref name="timeout"/>.</exception>
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        if (await Task.WhenAny(task, Task.Delay(timeout, cancellationToken)) != task)
        {
            throw new TimeoutException($"Task did not complete in {timeout}.");
        }

        return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified task and handles any exceptions that occur during its execution.
    /// </summary>
    /// <remarks>This method ensures that any exceptions thrown by the task are caught and can be handled
    /// using the <paramref name="onException"/> callback. If no callback is provided, exceptions are silently
    /// swallowed. The method does not propagate exceptions to the caller.</remarks>
    /// <param name="task">The task to execute. This parameter cannot be <see langword="null"/>.</param>
    /// <param name="onException">An optional callback that is invoked if an exception is thrown during the execution of the task. The exception
    /// is passed as a parameter to the callback.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to observe cancellation requests.</param>
    /// <exception cref="ArgumentNullException">Thrown if the <paramref name="task"/> parameter is <see langword="null"/>.</exception>
    public static async void WithExceptionHandler(this Task task, Action<Exception>? onException = null, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (onException != null) onException(ex);
        }
    }

    /// <summary>
    /// Executes the specified task and invokes the provided exception handler if an exception occurs.
    /// </summary>
    /// <remarks>This method is intended for fire-and-forget scenarios where exceptions need to be handled
    /// explicitly.  The method does not propagate exceptions or return the result of the task. Use caution when relying
    /// on this method,  as it suppresses unhandled exceptions unless an <paramref name="onException"/> handler is
    /// provided.</remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="onException">An optional action to handle exceptions that occur during the execution of the task. If <see langword="null"/>,
    /// exceptions are ignored.</param>
    /// <param name="cancellationToken">An optional cancellation token that can be used to observe cancellation requests. This parameter is not used to
    /// cancel the task itself.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    public static async void WithExceptionHandler<T>(this Task<T> task, Action<Exception>? onException = null, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (onException != null) onException(ex);
        }
    }

    /// <summary>
    /// Executes the specified task and handles any exceptions that occur during its execution.
    /// </summary>
    /// <remarks>This method allows you to attach an exception handler to a task without disrupting its
    /// execution flow. The method does not rethrow exceptions, and the task continues to execute asynchronously. Use
    /// this method to log or handle exceptions in a non-blocking manner.</remarks>
    /// <param name="task">The task to execute. This parameter cannot be <see langword="null"/>.</param>
    /// <param name="onException">An optional callback that is invoked if an exception is thrown during the execution of the task. The callback
    /// receives the exception and the cancellation token as parameters. If <see langword="null"/>, exceptions are not
    /// handled explicitly.</param>
    /// <param name="cancellationToken">A cancellation token that can be passed to the <paramref name="onException"/> callback. This token is not used
    /// to cancel the execution of the task itself.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    public static async void WithExceptionHandler(this Task task, Func<Exception, CancellationToken, Task>? onException = null, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (onException != null) await onException(ex, cancellationToken);
        }
    }

    /// <summary>
    /// Executes the specified task and handles any exceptions that occur during its execution.
    /// </summary>
    /// <remarks>This method allows you to attach an exception handler to a task without disrupting its
    /// execution flow. The method does not rethrow exceptions, and the task continues to execute asynchronously. Use
    /// this method to log or handle exceptions in a non-blocking manner.</remarks>
    /// <param name="task">The task to execute. This parameter cannot be <see langword="null"/>.</param>
    /// <param name="onException">An optional callback that is invoked if an exception is thrown during the execution of the task. The callback
    /// receives the exception and the cancellation token as parameters. If <see langword="null"/>, exceptions are not
    /// handled explicitly.</param>
    /// <param name="cancellationToken">A cancellation token that can be passed to the <paramref name="onException"/> callback. This token is not used
    /// to cancel the execution of the task itself.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    public static async void WithExceptionHandler<T>(this Task<T> task, Func<Exception, CancellationToken, Task>? onException = null, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if(onException != null) await onException(ex, cancellationToken);
        }
    }
}
