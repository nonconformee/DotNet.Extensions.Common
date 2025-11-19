
namespace nonconformee.DotNet.Extensions.Async;

/// <summary>
/// Provides extension methods for <see cref="CancellationToken"/>.
/// </summary>
public static class CancellationTokenExtensions
{
    // TODO : Pass additional state

    /// <summary>
    /// Registers a callback to be executed when the associated <see cref="CancellationToken"/> is canceled.
    /// </summary>
    /// <remarks>
    /// The callback is executed asynchronously and will not block the thread that triggers the cancellation.
    /// The error handler is executed synchronously on the thread that invokes the callback.
    /// If <paramref name="useSynchronizationContext"/> is <see langword="true"/>, the callback will be scheduled to run in the synchronization context captured at the time of registration, which may be useful in UI or other context-sensitive environments.
    /// <paramref name="onError"/>is not called for <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation.</param>
    /// <param name="callback">The asynchronous callback to invoke when the token is canceled. Cannot be <see langword="null"/>.</param>
    /// <param name="onError">The optional synchronous error handler to invoke when an exception occurs. Can be <see langword="null"/>.</param>
    /// <param name="useSynchronizationContext">A value indicating whether the callback should be executed in the current synchronization context. The default value is <see langword="false"/>.</param>
    /// <returns>A <see cref="CancellationTokenRegistration"/> instance that can be used to unregister the callback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public static CancellationTokenRegistration WithCallbackAsync(this CancellationToken cancellationToken, Func<CancellationToken, Task> callback, Action<CancellationToken, Exception>? onError = null, bool useSynchronizationContext = false)
    {
        if (callback is null) throw new ArgumentNullException(nameof(callback));

        SynchronizationContext? synchronizationContext = useSynchronizationContext ? SynchronizationContext.Current : null;

        return cancellationToken.Register(static async state =>
        {
            var (cb, ct, eh, sc) = ((Func<CancellationToken, Task>, CancellationToken, Action<CancellationToken, Exception>?, SynchronizationContext))state!;

            try
            {
                if(sc is not null)
                {
                    await cb(ct).RunInSynchronizationContext(sc).ConfigureAwait(false);
                }
                else
                {
                    await cb(ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                eh?.Invoke(ct, ex);
            }
        }, (callback, cancellationToken, onError, synchronizationContext));
    }

    /// <summary>
    /// Registers a callback to be executed when the associated <see cref="CancellationToken"/> is canceled.
    /// </summary>
    /// <remarks>
    /// The callback is executed synchronously on the thread that invokes the callback.
    /// The error handler is executed synchronously on the thread that invokes the callback.
    /// If <paramref name="useSynchronizationContext"/> is <see langword="true"/>, the callback will be scheduled to run in the synchronization context captured at the time of registration, which may be useful in UI or other context-sensitive environments.
    /// <paramref name="onError"/>is not called for <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> to monitor for cancellation.</param>
    /// <param name="callback">The synchronous callback to invoke when the token is canceled. Cannot be <see langword="null"/>.</param>
    /// <param name="onError">The optional synchronous error handler to invoke when an exception occurs. Can be <see langword="null"/>.</param>
    /// <param name="useSynchronizationContext">A value indicating whether the callback should be executed in the current synchronization context. The default value is <see langword="false"/>.</param>
    /// <returns>A <see cref="CancellationTokenRegistration"/> instance that can be used to unregister the callback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="callback"/> is <see langword="null"/>.</exception>
    public static CancellationTokenRegistration WithCallback(this CancellationToken cancellationToken, Action<CancellationToken> callback, Action<CancellationToken, Exception>? onError = null, bool useSynchronizationContext = false)
    {
        if (callback is null) throw new ArgumentNullException(nameof(callback));

        SynchronizationContext? synchronizationContext = useSynchronizationContext ? SynchronizationContext.Current : null;
        
        return cancellationToken.Register(static state =>
        {
            var (cb, ct, eh, sc) = ((Action<CancellationToken>, CancellationToken, Action<CancellationToken, Exception>?, SynchronizationContext))state!;
            
            try
            {
                if (sc is not null)
                {
                    sc.Post(x => cb((CancellationToken)x!), ct);
                }
                else
                {
                    cb(ct);
                }
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                eh?.Invoke(ct, ex);
            }
        }, (callback, cancellationToken, onError, synchronizationContext));
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
    /// <returns>A task that completes with <see langword="true"/> if the token is canceled, or <see langword="false"/> if the timeout elapses first.</returns>
    public static Task<bool> WithCompletionTask(this CancellationToken cancellationToken, TimeSpan? timeout = null)
    {
        timeout ??= Timeout.InfiniteTimeSpan;

        if (timeout == TimeSpan.Zero)
        {
            return Task.FromResult(cancellationToken.IsCancellationRequested);
        }

        var tcs = new TaskCompletionSource<bool>();

        var timer = new Timer(state =>
        {
            var localTcs = (TaskCompletionSource<bool>)state!;
            localTcs.TrySetResult(false);
        }, tcs, timeout.Value, Timeout.InfiniteTimeSpan);

        var registration = cancellationToken.Register(state =>
        {
            var localTcs = (TaskCompletionSource<bool>)state!;
            localTcs.TrySetResult(true);
        }, tcs);

        tcs.Task.ContinueWith(_ =>
        {
            timer.Dispose();
            registration.Dispose();
        });

        return tcs.Task;
    }
}

