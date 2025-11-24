
using System.Drawing;

namespace nonconformee.DotNet.Extensions.Streams;

/// <summary>
/// A read-only <see cref="Stream"/> that produces random bytes.
/// </summary>
/// <remarks>
/// Reading: The stream can be read; <see cref="CanRead"/> is always <see langword="true"/>.
/// Seeking: The stream can be seeked if a length is specified upon construction (see <see cref="RandomReadStream(long?, byte, byte, Random?)"/>); <see cref="CanSeek"/> is <see langword="true"/> if a length is specified, <see langword="false"/> otherwise.
/// Writing: The stream cannot be written to; <see cref="CanWrite"/> is always <see langword="false"/>. Any attempt to write to the stream (accessing or calling any member which has the word <c>Write</c> in it...) will throw a <see cref="NotSupportedException"/>.
/// Timeouts: The stream does not support timeouts; <see cref="CanTimeout"/> is always <see langword="false"/>.
/// Length: If a length is specified upon construction (see <see cref="RandomReadStream(long?, byte, byte, Random?)"/>), the stream has that length; otherwise, accessing <see cref="Length"/> or calling <see cref="SetLength(long)"/> throws <see cref="NotSupportedException"/>. The length can be changed via <see cref="SetLength(long)"/> if a length was specified upon construction. If the current position is beyond the new length, it is adjusted to be equal to the new length.
/// Position: If a length is specified upon construction (see <see cref="RandomReadStream(long?, byte, byte, Random?)"/>), the stream maintains a position; otherwise, accessing <see cref="Position"/> or calling <see cref="Seek(long, SeekOrigin)"/> throws <see cref="NotSupportedException"/>. The position can be changed via <see cref="Position"/> or <see cref="Seek(long, SeekOrigin)"/> if a length was specified upon construction. If the position is set beyond the length of the stream, the streams length will become equal to the new position.
/// Flushing: <see cref="Flush"/> and <see cref="FlushAsync(CancellationToken)"/> have no effect.
/// Closing/Disposing: <see cref="Stream.Close"/>, <see cref="IDisposable.Dispose"/> and <see cref="IAsyncDisposable.DisposeAsync"/> have no effect.
/// Async: All operations are synchronous, asynchronous methods are implemented by calling their synchronous counterparts.
/// </remarks>
public sealed class RandomReadStream : Stream
{
    private readonly object _sync = new();
    private readonly byte _min;
    private readonly byte _max;
    private readonly Random _random;

    private long? _length;
    private long? _offset;

    /// <summary>
    /// Creates a new instance of <see cref="RandomReadStream"/>.
    /// </summary>
    /// <param name="length">The total number of bytes the stream will generate. If <see langword="null"/>, the stream is infinite. Default value is <see langword="null"/>.</param>
    /// <param name="min">The inclusive lower bound of the random byte values. Default value is 0.</param>
    /// <param name="max">The inclusive upper bound of the random byte values. Default value is 255.</param>
    /// <param name="random">An optional <see cref="Random"/> instance to use for generating the random values. If <see langword="null"/>, a new instance is created. Default value is <see langword="null"/>.</param>
    public RandomReadStream(long? length = null, byte min = 0, byte max = 255, Random? random = null)
    {
        _length = length;
        _offset = length is null ? null : 0;

        _min = min;
        _max = max;
        _random = random ?? new Random();
    }

    /// <inheritdoc cref="Stream.CanRead"/>
    public override bool CanRead => true;

    /// <inheritdoc cref="Stream.CanSeek"/>
    public override bool CanSeek => _length is not null;

    /// <inheritdoc cref="Stream.CanWrite"/>
    public override bool CanWrite => false;

    /// <inheritdoc cref="Stream.CanTimeout"/>
    public override bool CanTimeout => false;

    /// <inheritdoc cref="Stream.Length"/>
    public override long Length
    {
        get => _length ?? throw new NotSupportedException("Length is not supported.");
    }

    /// <inheritdoc cref="Stream.Position"/>
    public override long Position
    {
        get => _offset ?? throw new NotSupportedException("Seeking or position is not supported.");
        set => Seek(value, SeekOrigin.Begin);
    }

    /// <inheritdoc cref="Stream.Flush"/>
    public override void Flush(){}

    /// <inheritdoc cref="Stream.FlushAsync(CancellationToken)"/>
    public override Task FlushAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    /// <inheritdoc cref="Stream.Read(byte[], int, int)"/>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0 || offset >= buffer.Length) throw new ArgumentOutOfRangeException(nameof(offset));
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

    /// <inheritdoc cref="Stream.ReadAsync(byte[],int,int,CancellationToken)"/>
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (count == 0) return Task.FromResult(0);

        return Task.FromResult(Read(buffer, offset, count));
    }

    /// <inheritdoc cref="Stream.ReadAsync(Memory{byte},CancellationToken)"/>
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (buffer.Length == 0) return ValueTask.FromResult(0);

        return ValueTask.FromResult(Read(buffer.Span));
    }

    /// <inheritdoc cref="Stream.Seek(long, SeekOrigin)"/>
    public override long Seek(long offset, SeekOrigin origin)
    {
        if (_offset is null) throw new NotSupportedException("Seeking or position is not supported.");

        switch (origin)
        {
            case SeekOrigin.Begin:
                if(offset < 0) throw new IOException("Attempted to seek before the beginning of the stream.");
                _offset = offset;
                break;

            case SeekOrigin.Current:
                if ((_offset! + offset) < 0) throw new IOException("Attempted to seek before the beginning of the stream.");
                _offset += offset;
                break;

            case SeekOrigin.End:
                if ((_length! - 1 + offset) < 0) throw new IOException("Attempted to seek before the beginning of the stream.");
                _offset = _length! - 1 + offset;
                break;

            default:
                throw new ArgumentException("Invalid origin.", nameof(origin));
        }

        if (_length < _offset)
        {
            _length = _offset;
        }

        return _offset!.Value;
    }

    /// <inheritdoc cref="Stream.SetLength(long)"/>
    public override void SetLength(long value)
    {
        if (_length is null) throw new NotSupportedException("Length is not supported.");

        _length = value;

        if (_offset > _length)
        {
            _offset = _length;
        }
    }

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override void Write(byte[] buffer, int offset, int count)
        => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override void Write(ReadOnlySpan<byte> buffer)
        => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        => Task.FromException(new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}."));

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
        => ValueTask.FromException(new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}."));

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override void WriteByte(byte value)
        => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
        => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override void EndWrite(IAsyncResult asyncResult)
        => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");

    /// <summary>Writing is not supported.</summary>
    /// <exception cref="NotSupportedException">Writing is not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override int WriteTimeout
    {
        get => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");
        set => throw new NotSupportedException($"Writing is not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");
    }

    /// <summary>Timeouts are not supported.</summary>
    /// <exception cref="NotSupportedException">Timeouts are not supported by the <see cref="Stream"/> implementation of <see cref="RandomReadStream"/>.</exception>
    public override int ReadTimeout
    {
        get => throw new NotSupportedException($"Timeouts are not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");
        set => throw new NotSupportedException($"Timeouts are not supported by the {nameof(Stream)} implementation of {nameof(RandomReadStream)}.");
    }

    /// <inheritdoc cref="Stream.DisposeAsync()"/>
    public override ValueTask DisposeAsync() => base.DisposeAsync();

    /// <inheritdoc cref="Stream.Dispose(bool)"/>
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