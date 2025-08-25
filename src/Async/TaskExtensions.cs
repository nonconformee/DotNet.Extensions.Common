
namespace nonconformee.DotNet.Extensions.Async;

/// <summary>
/// Provides extension methods for <see cref="Task"/> and <see cref="Task{T}"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Waits for the specified task to complete within the given timeout period.
    /// </summary>
    /// <remarks>
    /// If the task does not complete within the specified timeout, a <see cref="TimeoutException"/> is thrown.
    /// The original task (<paramref name="task"/> is not canceled or modified in any way.
    /// </remarks>
    /// <param name="task">The task to wait for. Cannot be <see langword="null"/>.</param>
    /// <param name="timeout">The maximum amount of time to wait for the task to complete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the wait operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    /// <exception cref="TimeoutException">The task did not complete within the specified <paramref name="timeout"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled by the provided <paramref name="cancellationToken"/>.</exception>"
    public static async Task WithTimeout(this Task task, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        var timeoutTask = cancellationToken.AsCompletedTask(timeout);
        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if(completedTask == timeoutTask)
        {
            if (await timeoutTask.ConfigureAwait(false))
            {
                throw new OperationCanceledException(cancellationToken);
            }

            throw new TimeoutException($"Task did not complete in {timeout}.");
        }

        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Waits for the specified task to complete within the given timeout period.
    /// </summary>
    /// <remarks>
    /// If the task does not complete within the specified timeout, a <see cref="TimeoutException"/> is thrown.
    /// The original task (<paramref name="task"/> is not canceled or modified in any way.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to wait for. Cannot be <see langword="null"/>.</param>
    /// <param name="timeout">The maximum amount of time to wait for the task to complete.</param>
    /// <param name="cancellationToken">A cancellation token to cancel the wait operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    /// <exception cref="TimeoutException">The task did not complete within the specified <paramref name="timeout"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation was canceled by the provided <paramref name="cancellationToken"/>.</exception>"
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        var timeoutTask = cancellationToken.AsCompletedTask(timeout);
        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if (completedTask == timeoutTask)
        {
            if (await timeoutTask.ConfigureAwait(false))
            {
                throw new OperationCanceledException(cancellationToken);
            }

            throw new TimeoutException($"Task did not complete in {timeout}.");
        }

        return await task.ConfigureAwait(false);
    }

    public static async void WithExceptionHandler(this Task task, Action<Exception> onException, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onException(ex);
        }
    }

    public static async void WithExceptionHandler<T>(this Task<T> task, Action<Exception> onException, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onException(ex);
        }
    }
}
