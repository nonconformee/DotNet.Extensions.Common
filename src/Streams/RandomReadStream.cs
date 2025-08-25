
namespace nonconformee.DotNet.Extensions.Streams;

/// <summary>
/// A read-only <see cref="Stream"/> that produces random bytes.
/// </summary>
/// <remarks>
/// Characteristics:
/// - Readable: <see cref="CanRead"/> is <c>true</c>.
/// - Not seekable: <see cref="CanSeek"/> is <c>false</c>; <see cref="Length"/> and <see cref="Position"/> are unsupported.
/// - Not writable: <see cref="CanWrite"/> is <c>false</c>.
/// - Endless: every read request (with a positive count) is fully satisfied; the stream never reports end-of-stream.
/// - Backed by a provided <see cref="Random"/> instance or a lazily created one.
/// - <see cref="Flush"/> and <see cref="FlushAsync(CancellationToken)"/> have no effect.
/// - <see cref="IDisposable.Dispose"/> and <see cref="IAsyncDisposable.DisposeAsync"/> have no effect.
/// </remarks>
public sealed class RandomReadStream : Stream
{
    private readonly object _sync = new();
    private readonly Random _random;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="random">Optional randomizer. If <see langword="null"/> or not provided, a new instance is created.</param>
    public RandomReadStream(Random? random = null)
    {
        _random = random ?? new Random();
    }

    /// <inheritdoc cref="Stream.CanRead"/>
    public override bool CanRead => true;

    /// <inheritdoc cref="Stream.CanSeek"/>
    public override bool CanSeek => false;

    /// <inheritdoc cref="Stream.CanWrite"/>
    public override bool CanWrite => false;

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override long Length => throw new NotSupportedException("This stream has no defined length.");

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override long Position
    {
        get => throw new NotSupportedException("This stream does not support seeking or position.");
        set => throw new NotSupportedException("This stream does not support seeking or position.");
    }

    /// <inheritdoc cref="Stream.Flush"/>
    public override void Flush(){}

    /// <inheritdoc cref="Stream.FlushAsync(CancellationToken)"/>
    public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc cref="Stream.Read(byte[], int, int)"/>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || offset > buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
        if (count < 0 || (offset + count) > buffer.Length) throw new ArgumentOutOfRangeException(nameof(count));

        if (count == 0) return 0;

        FillRandom(buffer, offset, count);
        return count;
    }

    /// <inheritdoc cref="Stream.Read(Span{byte})"/>
    public override int Read(Span<byte> buffer)
    {
        if (buffer.Length == 0) return 0;

        FillRandom(buffer);
        return buffer.Length;
    }

    /// <inheritdoc cref="Stream.ReadAsync(Memory{byte},CancellationToken)"/>
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (buffer.Length == 0) return ValueTask.FromResult(0);

        FillRandom(buffer.Span);
        return ValueTask.FromResult(buffer.Length);
    }

    /// <inheritdoc cref="Stream.ReadAsync(byte[],int,int,CancellationToken)"/>
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(Read(buffer, offset, count));
    }

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override long Seek(long offset, SeekOrigin origin)
        => throw new NotSupportedException("Seeking is not supported.");

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override void SetLength(long value)
        => throw new NotSupportedException("Setting length is not supported.");

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override void Write(byte[] buffer, int offset, int count)
        => throw new NotSupportedException("Writing is not supported.");

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override void Write(ReadOnlySpan<byte> buffer)
        => throw new NotSupportedException("Writing is not supported.");

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => Task.FromException(new NotSupportedException("Writing is not supported."));

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        => ValueTask.FromException(new NotSupportedException("Writing is not supported."));

    /// <summary>Not supported.</summary>
    /// <exception cref="NotSupportedException">Not supported by this <see cref="Stream"/> implementation.</exception>
    public override void WriteByte(byte value)
        => throw new NotSupportedException("Writing is not supported.");

    /// <inheritdoc cref="Stream.DisposeAsync()"/>
    public override ValueTask DisposeAsync() => base.DisposeAsync();

    protected override void Dispose(bool disposing) => base.Dispose(disposing);

    private void FillRandom(byte[] buffer, int offset, int count)
    {
        lock (_sync)
        {
            if (offset == 0 && count == buffer.Length)
            {
                _random.NextBytes(buffer);
            }
            else
            {
                Span<byte> span = buffer.AsSpan(offset, count);
                FillRandom(span);
            }
        }
    }

    private void FillRandom(Span<byte> span)
    {
        var temp = new byte[span.Length];

        lock (_sync)
        {
            _random.NextBytes(temp);
        }

        temp.AsSpan().CopyTo(span);
    }
}