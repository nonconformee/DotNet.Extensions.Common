
namespace nonconformee.DotNet.Extensions.Async;

/// <summary>
/// Provides extension methods for <see cref="CancellationToken"/>.
/// </summary>
public static class CancellationTokenExtensions
{
    /// <summary>
    /// Registers a callback to be executed when the associated <see cref="CancellationToken"/> is canceled.
    /// </summary>
    /// <remarks>
    /// The callback is executed asynchronously and will not block the thread that triggers the cancellation.
    /// If <paramref name="useSynchronizationContext"/> is <see langword="true"/>, the callback will be scheduled to run in the synchronization context captured at the time of registration, which may be useful in UI or other context-sensitive environments.
    /// </remarks>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation.</param>
    /// <param name="callback">The asynchronous callback to invoke when the token is canceled. Cannot be <see langword="null"/>.</param>
    /// <param name="useSynchronizationContext">A value indicating whether the callback should be executed in the current synchronization context. The default value is <see langword="false"/>.</param>
    /// <returns>A <see cref="CancellationTokenRegistration"/> instance that can be used to unregister the callback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public static CancellationTokenRegistration RegisterAsync(this CancellationToken cancellationToken, Func<Task> callback, bool useSynchronizationContext = false)
    {
        if (callback == null) throw new ArgumentNullException(nameof(callback));

        return cancellationToken.Register(async () =>
        {
            if (useSynchronizationContext)
            {
                await Task.Yield();
            }
            await callback().ConfigureAwait(false);
        }, useSynchronizationContext);
    }

    /// <summary>
    /// Creates a task that is completed when the specified <see cref="CancellationToken"/> is triggered or after an optional timeout.
    /// </summary>
    /// <remarks>
    /// This method can be used to await the cancellation of a <see cref="CancellationToken"/> while also supporting an optional timeout.
    /// If both the token is canceled and the timeout elapses simultaneously, the task may complete with either result depending on the order of execution.
    /// </remarks>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation.</param>
    /// <param name="timeout">An optional <see cref="TimeSpan"/> specifying the maximum time to wait before the task completes. If optional or <see langword="null"/>, the task will wait indefinitely for the token to be canceled.</param>
    /// <returns>A <see cref="Task{TResult}"/> that completes with <see langword="true"/> if the token is canceled, or <see langword="false"/> if the timeout elapses first.</returns>
    public static Task<bool> AsCompletedTask(this CancellationToken cancellationToken, TimeSpan? timeout = null)
    {
        var tcs = new TaskCompletionSource<bool>();
        cancellationToken.Register(() => tcs.TrySetResult(true));
        Task.Delay(timeout ?? Timeout.InfiniteTimeSpan).ContinueWith(_ => tcs.TrySetResult(false));
        return tcs.Task;
    }
}
