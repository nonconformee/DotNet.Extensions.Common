
using System.Threading;

namespace nonconformee.DotNet.Extensions.Async;

// TODO : Pass additional state
// TODO : WithFinalizer
// TODO : WithDelay
// TODO : WithRetry
// TODO : WithThrottling
// TODO : WithCircuitBreaker
// TODO : WithFallback 
// TODO : WithTimeoutHandler
// TODO : WithCancellationHandler

/// <summary>
/// Provides extension methods for <see cref="Task"/> and <see cref="Task{T}"/>.
/// </summary>
public static class TaskExtensions
{
    /// <summary>
    /// Adds support for cancellation to the specified task.
    /// </summary>
    /// <remarks>
    /// This method allows a caller to attach a <see cref="CancellationToken"/> to an existing task,
    /// enabling the task to be canceled if the token is triggered. If the cancellation token is canceled before the
    /// task completes, the returned task will throw an <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <param name="task">The task to monitor for completion or cancellation. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
    /// <returns>A task that completes with the result of the original task if it finishes successfully, or throws an <see
    /// cref="OperationCanceledException"/> if the operation is canceled.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation is canceled via the provided <paramref name="cancellationToken"/>.</exception>
    public static async Task WithCancellation(this Task task, CancellationToken cancellationToken)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        var cancellationTask = cancellationToken.WithCompletionTask();
        var completedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

        if (completedTask == cancellationTask)
        {
            await cancellationTask.ConfigureAwait(false);
            throw new OperationCanceledException(cancellationToken);
        }

        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Adds support for cancellation to the specified task.
    /// </summary>
    /// <remarks>
    /// This method allows a caller to attach a <see cref="CancellationToken"/> to an existing task,
    /// enabling the task to be canceled if the token is triggered. If the cancellation token is canceled before the
    /// task completes, the returned task will throw an <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to monitor for completion or cancellation. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> that can be used to cancel the task.</param>
    /// <returns>A task that completes with the result of the original task if it finishes successfully, or throws an <see
    /// cref="OperationCanceledException"/> if the operation is canceled.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation is canceled via the provided <paramref name="cancellationToken"/>.</exception>
    public static async Task<T> WithCancellation<T>(this Task<T> task, CancellationToken cancellationToken)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));

        var cancellationTask = cancellationToken.WithCompletionTask();
        var completedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

        if (completedTask == cancellationTask)
        {
            await cancellationTask.ConfigureAwait(false);
            throw new OperationCanceledException(cancellationToken);
        }

        return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Awaits the specified task and invokes a cancellation handler if the task is canceled due to the provided <see cref="CancellationToken"/>.
    /// </summary>
    /// <remarks>
    /// This method monitors the provided task and the cancellation token concurrently. If the
    /// cancellation token is triggered before the task completes, the <paramref name="onCancellation"/> action is
    /// invoked. Otherwise, the method awaits the completion of the task.
    /// </remarks>
    /// <param name="task">The task to monitor for completion or cancellation. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to signal cancellation.</param>
    /// <param name="onCancellation">An action to invoke if the task is canceled. The action receives the original task and the cancellation token as
    /// parameters.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="onCancellation"/> is <see langword="null"/>.</exception>
    public static async Task WithCancellationHandler(this Task task, CancellationToken cancellationToken, Action<Task, CancellationToken> onCancellation)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        if (onCancellation == null) throw new ArgumentNullException(nameof(onCancellation));

        var cancellationTask = cancellationToken.WithCompletionTask();
        var completedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

        if (completedTask == cancellationTask)
        {
            await cancellationTask.ConfigureAwait(false);
            onCancellation(task, cancellationToken);
            return;
        }

        await task.ConfigureAwait(false);


    }

    /// <summary>
    /// Awaits the specified task and invokes a cancellation handler if the task is canceled due to the provided <see cref="CancellationToken"/>.
    /// </summary>
    /// <remarks>
    /// This method monitors the provided task and the cancellation token concurrently. If the
    /// cancellation token is triggered before the task completes, the <paramref name="onCancellation"/> action is
    /// invoked. Otherwise, the method awaits the completion of the task.
    /// The cancellation handler returns a value of type <typeparamref name="T"/> to be used as the result of the task in case of cancellation.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to monitor for completion or cancellation. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to signal cancellation.</param>
    /// <param name="onCancellation">An action to invoke if the task is canceled. The action receives the original task and the cancellation token as
    /// parameters.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="onCancellation"/> is <see langword="null"/>.</exception>
    public static async Task<T> WithCancellationHandler<T>(this Task<T> task, CancellationToken cancellationToken, Func<Task, CancellationToken, T> onCancellation)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        if (onCancellation == null) throw new ArgumentNullException(nameof(onCancellation));

        var cancellationTask = cancellationToken.WithCompletionTask();
        var completedTask = await Task.WhenAny(task, cancellationTask).ConfigureAwait(false);

        if (completedTask == cancellationTask)
        {
            await cancellationTask.ConfigureAwait(false);
            return onCancellation(task, cancellationToken);
        }

        return await task.ConfigureAwait(false);
    }

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

        var timeoutTask = cancellationToken.WithCompletionTask(timeout);
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

        var timeoutTask = cancellationToken.WithCompletionTask(timeout);
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

    /// <summary>
    /// Executes a task with a specified timeout and invokes a callback if the timeout is exceeded.
    /// </summary>
    /// <remarks>
    /// This method waits for the specified task to complete within the given timeout. If the task
    /// does not complete in time,  the <paramref name="onTimeout"/> callback is invoked. If the timeout is triggered
    /// and the cancellation token is canceled,  an <see cref="OperationCanceledException"/> is thrown.
    /// </remarks>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="timeout">The maximum duration to wait for the task to complete before the timeout is triggered.</param>
    /// <param name="onTimeout">A callback that is invoked when the timeout is exceeded. The callback receives the original task, the timeout
    /// duration,  and the cancellation token. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
    /// cref="CancellationToken.None"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="onTimeout"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation is canceled via the <paramref name="cancellationToken"/>.</exception>
    public static async Task WithTimeoutHandler(this Task task, TimeSpan timeout, Action<Task, TimeSpan, CancellationToken> onTimeout, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        if (onTimeout == null) throw new ArgumentNullException(nameof(onTimeout));

        var timeoutTask = cancellationToken.WithCompletionTask(timeout);
        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if (completedTask == timeoutTask)
        {
            if (await timeoutTask.ConfigureAwait(false))
            {
                throw new OperationCanceledException(cancellationToken);
            }

            onTimeout(task, timeout, cancellationToken);
            return;
        }

        await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Executes a task with a specified timeout and invokes a callback if the timeout is exceeded.
    /// </summary>
    /// <remarks>
    /// This method waits for the specified task to complete within the given timeout. If the task
    /// does not complete in time,  the <paramref name="onTimeout"/> callback is invoked. If the timeout is triggered
    /// and the cancellation token is canceled,  an <see cref="OperationCanceledException"/> is thrown.
    /// The cancellation handler returns a value of type <typeparamref name="T"/> to be used as the result of the task in case of cancellation.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="timeout">The maximum duration to wait for the task to complete before the timeout is triggered.</param>
    /// <param name="onTimeout">A callback that is invoked when the timeout is exceeded. The callback receives the original task, the timeout
    /// duration,  and the cancellation token. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">An optional <see cref="CancellationToken"/> that can be used to cancel the operation. Defaults to <see
    /// cref="CancellationToken.None"/>.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="onTimeout"/> is <see langword="null"/>.</exception>
    /// <exception cref="OperationCanceledException">The operation is canceled via the <paramref name="cancellationToken"/>.</exception>
    public static async Task<T> WithTimeoutHandler<T>(this Task<T> task, TimeSpan timeout, Func<Task, TimeSpan, CancellationToken, T> onTimeout, CancellationToken cancellationToken = default)
    {
        if (task == null) throw new ArgumentNullException(nameof(task));
        if (onTimeout == null) throw new ArgumentNullException(nameof(onTimeout));

        var timeoutTask = cancellationToken.WithCompletionTask(timeout);
        var completedTask = await Task.WhenAny(task, timeoutTask).ConfigureAwait(false);

        if (completedTask == timeoutTask)
        {
            if (await timeoutTask.ConfigureAwait(false))
            {
                throw new OperationCanceledException(cancellationToken);
            }

            return onTimeout(task, timeout, cancellationToken);
        }

        return await task.ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the specified task and invokes the provided exception handler if an exception occurs.
    /// </summary>
    /// <remarks>
    /// This method ensures that any exception thrown during the execution of the task is caught and passed to the specified exception handler.
    /// The task is awaited asynchronously, and the exception handler is invoked synchronously.
    /// <paramref name="onException"/>is not called for <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="onException">The action to invoke when an exception is thrown. The exception is passed as a parameter to this action. Cannot be <see langword="null"/>.</param>
    /// <returns>The task to await the completion of <paramref name="task"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="onException"/> is <see langword="null"/>.</exception>
    public static async Task WithExceptionHandler(this Task task, Action<Exception> onException)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));
        if (onException is null) throw new ArgumentNullException(nameof(onException));

        try
        {
            await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            onException(ex);
        }
    }

    /// <summary>
    /// Executes the specified task and invokes the provided exception handler if an exception occurs.
    /// </summary>
    /// <remarks>
    /// This method ensures that any exception thrown during the execution of the task is caught and passed to the specified exception handler.
    /// The task is awaited asynchronously, and the exception handler is invoked synchronously.
    /// The exception handler returns a value of type <typeparamref name="T"/> to be used as the result of the task in case of an exception.
    /// <paramref name="onException"/>is not called for <see cref="OperationCanceledException"/>.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="onException">The action to invoke when an exception is thrown. The exception is passed as a parameter to this action. Cannot be <see langword="null"/>.</param>
    /// <returns>The task to await the completion of <paramref name="task"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="onException"/> is <see langword="null"/>.</exception>
    public static async Task<T> WithExceptionHandler<T>(this Task<T> task, Func<Exception, T> onException)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));
        if (onException is null) throw new ArgumentNullException(nameof(onException));

        try
        {
            return await task.ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            return onException(ex);
        }
    }

    /// <summary>
    /// Creates a <see cref="CancellationToken"/> to the specified task that is canceled when the task completes.
    /// </summary>
    /// <param name="task">The task to monitor for completion. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">When this method returns, contains a <see cref="CancellationToken"/> that is canceled when the task completes.</param>
    /// <returns>The original task.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    public static Task WithCompletionToken (this Task task, out CancellationToken cancellationToken)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));

        CancellationTokenSource cts = new CancellationTokenSource();
        task.ContinueWith(_ => cts.Cancel());
        cancellationToken = cts.Token;

        return task;
    }

    /// <summary>
    /// Creates a <see cref="CancellationToken"/> to the specified task that is canceled when the task completes.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to monitor for completion. Cannot be <see langword="null"/>.</param>
    /// <param name="cancellationToken">When this method returns, contains a <see cref="CancellationToken"/> that is canceled when the task completes.</param>
    /// <returns>The original task.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="task"/> is <see langword="null"/>.</exception>
    public static Task<T> WithCompletionToken<T>(this Task<T> task, out CancellationToken cancellationToken)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));

        CancellationTokenSource cts = new CancellationTokenSource();
        task.ContinueWith(_ => cts.Cancel());
        cancellationToken = cts.Token;

        return task;
    }

    /// <summary>
    /// Executes the specified task within the provided <see cref="SynchronizationContext"/>.
    /// </summary>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="synchronizationContext">The <see cref="SynchronizationContext"/> to use for the task execution. Cannot be <see langword="null"/>.</param>
    /// <returns>The task used to await the completion of <paramref name="task"/> in <paramref name="synchronizationContext"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="synchronizationContext"/> is <see langword="null"/>.</exception>
    public static Task RunInSynchronizationContext(this Task task, SynchronizationContext synchronizationContext)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));
        if (synchronizationContext is null) throw new ArgumentNullException(nameof(synchronizationContext));

        var tcs = new TaskCompletionSource<object?>();

        synchronizationContext.Post(async state =>
        {
            try
            {
                await task.ConfigureAwait(false);
                tcs.SetResult(state);
            }
            catch (OperationCanceledException oce)
            {
                tcs.SetCanceled(oce.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }, null);

        return tcs.Task;
    }

    /// <summary>
    /// Executes the specified task within the provided <see cref="SynchronizationContext"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <param name="synchronizationContext">The <see cref="SynchronizationContext"/> to use for the task execution. Cannot be <see langword="null"/>.</param>
    /// <returns>The task used to await the completion of <paramref name="task"/> in <paramref name="synchronizationContext"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> or <paramref name="synchronizationContext"/> is <see langword="null"/>.</exception>
    public static Task<T> RunInSynchronizationContext<T>(this Task<T> task, SynchronizationContext synchronizationContext)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));
        if (synchronizationContext is null) throw new ArgumentNullException(nameof(synchronizationContext));

        var tcs = new TaskCompletionSource<T>();

        synchronizationContext.Post(async state =>
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                tcs.SetResult(result);
            }
            catch (OperationCanceledException oce)
            {
                tcs.SetCanceled(oce.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }, null);

        return tcs.Task;
    }

    /// <summary>
    /// Executes the specified task within the <see cref="ThreadPool"/>.
    /// </summary>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <returns>The task used to await the completion of <paramref name="task"/> in <see cref="ThreadPool"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    public static Task RunInThreadPool(this Task task)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));

        var tcs = new TaskCompletionSource<object?>();

        ThreadPool.QueueUserWorkItem(async state =>
        {
            try
            {
                await task.ConfigureAwait(false);
                tcs.SetResult(state);
            }
            catch (OperationCanceledException oce)
            {
                tcs.SetCanceled(oce.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }, null);

        return tcs.Task;
    }

    /// <summary>
    /// Executes the specified task within the <see cref="ThreadPool"/>.
    /// </summary>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to execute. Cannot be <see langword="null"/>.</param>
    /// <returns>The task used to await the completion of <paramref name="task"/> in <see cref="ThreadPool"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    public static Task<T> RunInThreadPool<T>(this Task<T> task)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));

        var tcs = new TaskCompletionSource<T>();

        ThreadPool.QueueUserWorkItem(async state =>
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                tcs.SetResult(result);
            }
            catch (OperationCanceledException oce)
            {
                tcs.SetCanceled(oce.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        }, null);

        return tcs.Task;
    }

    /// <summary>
    /// Executes the specified task on a separate thread and returns a <see cref="Task"/> that represents the completion of the operation.
    /// </summary>
    /// <remarks>
    /// This method allows executing a task on a dedicated thread, which can be useful for isolating
    /// long-running or resource-intensive operations. The optional <paramref name="threadConfig"/> parameter can be
    /// used to customize the thread's behavior, such as setting its name or priority.
    /// </remarks>
    /// <param name="task">The task to be executed on a separate thread. Cannot be <see langword="null"/>.</param>
    /// <param name="threadConfig">An optional action to configure the <see cref="Thread"/> before it starts. If <see langword="null"/>, no additional configuration is applied.</param>
    /// <returns>
    /// A <see cref="Task"/> that completes when the specified task finishes execution. If the task is canceled, the
    /// returned task will be in the canceled state. If the task throws an exception, the returned task will propagate
    /// the exception.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    public static Task RunInThread(this Task task, Action<Thread>? threadConfig = null)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));

        var tcs = new TaskCompletionSource<object?>();

        Thread thread = new Thread(async () =>
        {
            try
            {
                await task.ConfigureAwait(false);
                tcs.SetResult(null);
            }
            catch (OperationCanceledException oce)
            {
                tcs.SetCanceled(oce.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        threadConfig?.Invoke(thread);

        thread.Start();

        return tcs.Task;
    }

    /// <summary>
    /// Executes the specified task on a separate thread and returns a <see cref="Task"/> that represents the completion of the operation.
    /// </summary>
    /// <remarks>
    /// This method allows executing a task on a dedicated thread, which can be useful for isolating
    /// long-running or resource-intensive operations. The optional <paramref name="threadConfig"/> parameter can be
    /// used to customize the thread's behavior, such as setting its name or priority.
    /// </remarks>
    /// <typeparam name="T">The type of the result produced by the task.</typeparam>
    /// <param name="task">The task to be executed on a separate thread. Cannot be <see langword="null"/>.</param>
    /// <param name="threadConfig">An optional action to configure the <see cref="Thread"/> before it starts. If <see langword="null"/>, no additional configuration is applied.</param>
    /// <returns>
    /// A <see cref="Task"/> that completes when the specified task finishes execution. If the task is canceled, the
    /// returned task will be in the canceled state. If the task throws an exception, the returned task will propagate
    /// the exception.
    /// </returns>
    /// <exception cref="ArgumentNullException"><paramref name="task"/> is <see langword="null"/>.</exception>
    public static Task<T> RunInThread<T>(this Task<T> task, Action<Thread>? threadConfig = null)
    {
        if (task is null) throw new ArgumentNullException(nameof(task));

        var tcs = new TaskCompletionSource<T>();

        Thread thread = new Thread(async () =>
        {
            try
            {
                var result = await task.ConfigureAwait(false);
                tcs.SetResult(result);
            }
            catch (OperationCanceledException oce)
            {
                tcs.SetCanceled(oce.CancellationToken);
            }
            catch (Exception ex)
            {
                tcs.SetException(ex);
            }
        });

        threadConfig?.Invoke(thread);

        thread.Start();

        return tcs.Task;
    }

    public static bool IsTaskOf(this Type type, Type resultType)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (resultType is null) throw new ArgumentNullException(nameof(resultType));

        return type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(Task<>)
            && type.GetGenericArguments()[0] == resultType;
    }

    public static bool IsTaskOf<T>(this Type type) => type.IsTaskOf(typeof(T));

    public static bool IsValueTaskOf(this Type type, Type resultType)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));
        if (resultType is null) throw new ArgumentNullException(nameof(resultType));

        return type.IsGenericType
            && type.GetGenericTypeDefinition() == typeof(ValueTask<>)
            && type.GetGenericArguments()[0] == resultType;
    }

    public static bool IsValueTaskOf<T>(this Type type) => type.IsValueTaskOf(typeof(T));
}
