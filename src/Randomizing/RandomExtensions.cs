
using nonconformee.DotNet.Extensions.Numbers;
using System.Net.Sockets;
using System.Text;

namespace nonconformee.DotNet.Extensions.Randomizing;

/// <summary>
/// Provides extension methods for <see cref="Random"/>.
/// </summary>
public static class RandomExtensions
{
    private const string _loremIpsumRaw = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit. Sed lectus. Integer euismod lacus luctus magna. Quisque cursus, metus vitae pharetra auctor, sem massa mattis sem, at interdum magna augue eget diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Morbi lacinia molestie dui. Praesent blandit dolor. Sed non quam. In vel mi sit amet augue congue elementum. Morbi in ipsum sit amet pede facilisis laoreet. Donec lacus nunc, viverra nec, blandit vel, egestas et, augue. Vestibulum tincidunt malesuada tellus. Ut ultrices ultrices enim. Curabitur sit amet mauris. Morbi in dui quis est pulvinar ullamcorper. Nulla facilisi. Integer lacinia sollicitudin massa. Cras metus. Sed aliquet risus a tortor. Integer id quam. Morbi mi. Quisque nisl felis, venenatis tristique, dignissim in, ultrices sit amet, augue. Proin sodales libero eget ante. Nulla quam. Aenean laoreet. Vestibulum nisi lectus, commodo ac, facilisis ac, ultricies eu, pede. Ut orci risus, accumsan porttitor, cursus quis, aliquet eget, justo. Sed pretium blandit orci. Ut eu diam at pede suscipit sodales. Aenean lectus elit, fermentum non, convallis id, sagittis at, neque. Nullam mauris orci, aliquet et, iaculis et, viverra vitae, ligula. Nulla ut felis in purus aliquam imperdiet. Maecenas aliquet mollis lectus. Vivamus consectetuer risus et tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit. Sed lectus. Integer euismod lacus luctus magna. Quisque cursus, metus vitae pharetra auctor, sem massa mattis sem, at interdum magna augue eget diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Morbi lacinia molestie dui. Praesent blandit dolor. Sed non quam. In vel mi sit amet augue congue elementum. Morbi in ipsum sit amet pede facilisis laoreet. Donec lacus nunc, viverra nec, blandit vel, egestas et, augue. Vestibulum tincidunt malesuada tellus. Ut ultrices ultrices enim. Curabitur sit amet mauris. Morbi in dui quis est pulvinar ullamcorper. Nulla facilisi. Integer lacinia sollicitudin massa. Cras metus. Sed aliquet risus a tortor. Integer id quam. Morbi mi. Quisque nisl felis, venenatis tristique, dignissim in, ultrices sit amet, augue. Proin sodales libero eget ante. Nulla quam. Aenean laoreet. Vestibulum nisi lectus, commodo ac, facilisis ac, ultricies eu, pede. Ut orci risus, accumsan porttitor, cursus quis, aliquet eget, justo. Sed pretium blandit orci. Ut eu diam at pede suscipit sodales. Aenean lectus elit, fermentum non, convallis id, sagittis at, neque. Nullam mauris orci, aliquet et, iaculis et, viverra vitae, ligula. Nulla ut felis in purus aliquam imperdiet. Maecenas aliquet mollis lectus. Vivamus consectetuer risus et tortor. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Integer nec odio. Praesent libero. Sed cursus ante dapibus diam. Sed nisi. Nulla quis sem at nibh elementum imperdiet. Duis sagittis ipsum. Praesent mauris. Fusce nec tellus sed augue semper porta. Mauris massa. Vestibulum lacinia arcu eget nulla. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Curabitur sodales ligula in libero. Sed dignissim lacinia nunc. Curabitur tortor. Pellentesque nibh. Aenean quam. In scelerisque sem at dolor. Maecenas mattis. Sed convallis tristique sem. Proin ut ligula vel nunc egestas porttitor. Morbi lectus risus, iaculis vel, suscipit quis, luctus non, massa. Fusce ac turpis quis ligula lacinia aliquet. Mauris ipsum. Nulla metus metus, ullamcorper vel, tincidunt sed, euismod in, nibh. Quisque volutpat condimentum velit. Class aptent taciti sociosqu ad litora torquent per conubia nostra, per inceptos himenaeos. Nam nec ante. Sed lacinia, urna non tincidunt mattis, tortor neque adipiscing diam, a cursus ipsum ante quis turpis. Nulla facilisi. Ut fringilla. Suspendisse potenti. Nunc feugiat mi a tellus consequat imperdiet. Vestibulum sapien. Proin quam. Etiam ultrices. Suspendisse in justo eu magna luctus suscipit. Sed lectus. Integer euismod lacus luctus magna. Quisque cursus, metus vitae pharetra auctor, sem massa mattis sem, at interdum magna augue eget diam. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia Curae; Morbi lacinia molestie dui. Praesent blandit dolor. Sed non quam. In vel mi sit amet augue congue elementum. Morbi in ipsum si.";
    private static readonly object _loremIpsumGenerateLock = new object();
    private static readonly List<string> _loremIpsumWords = new List<string>();


    /// <summary>
    /// Fills a <see cref="Stream" /> with random bytes.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="stream"> The <see cref="Stream" /> to fill. Cannot be <see langword="null"/>. </param>
    /// <param name="length"> The amount of bytes to fill in the <see cref="Stream" /> at its current position. Can be <see langword="null"/> to fill the entire stream. </param>
    /// <returns> The number of written bytes to the <see cref="Stream" />. The actual amount of bytes written might be lower than specified by <paramref name="length"/> if the stream is shorter.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> or <paramref name="stream" /> is null. </exception>
    /// <exception cref="ArgumentException"> <paramref name="stream" /> is not writeable. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
    /// <remarks>If the length is not specified by <paramref name="length"/>, <paramref name="stream"/> must be seekable so the end of the stream can be determined.
    /// Otherwise, the stream could be filled in an endless loop (e.g. when using <see cref="NetworkStream"/>).</remarks>
    public static int FillStream(this Random randomizer, Stream stream, int? length = null)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (!stream.CanWrite) throw new ArgumentException("Stream cannot be written to.", nameof(stream));
        if (length is not null && length < 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than zero if specified.");
        if (length is null && !stream.CanSeek) throw new ArgumentException("Stream must be seekable if length is not specified.", nameof(stream));
        
        if(length is null)
        {
            length = (int)(stream.Length - stream.Position);
            if (length < 0) length = 0;
        }

        int bytesWritten = 0;

        for (int i = 0; i < length; i++)
        {
            stream.WriteByte(randomizer.NextByte());
            bytesWritten++;
        }
        
        return bytesWritten;
    }

    /// <summary>
    /// Returns a random enum value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type.</typeparam>
    /// <param name="randomizer">The randomizer. Cannot be <see langword="null"/>.</param>
    /// <returns>A random enum value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="randomizer" /> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentException"><typeparamref name="T"/> is not an enum type.</exception>
    public static T NextEnum<T>(this Random randomizer) where T : Enum
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (!typeof(T).IsEnum) throw new ArgumentException("T must be an enum type.", nameof(T));

        var values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(randomizer.Next(values.Length))!;
    }

    /// <summary>
    /// Returns random <see langword="true" /> or <see langword="false" /> with a 50/50 percent chance.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>.</param>
    /// <returns><see langword="true" /> or <see langword="false" />.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="randomizer" /> is <see langword="null"/>.</exception>
    public static bool NextBoolean(this Random randomizer)
    {
        if (randomizer is null) throw new ArgumentNullException(nameof(randomizer));

        return randomizer.Next(0, 2) == 0;
    }

    /// <summary>
    /// Returns random <see langword="true" /> or <see langword="false" /> with a specified chance for <see langword="true" />.
    /// </summary>
    /// <param name="randomizer">The randomizer. Cannot be <see langword="null"/>.</param>
    /// <param name="chance">The chance to return <see langword="true" />, between 0.0 (never) and 1.0 (always).</param>
    /// <returns><see langword="true" /> or <see langword="false" /> based on the specified chance.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is <see langword="null"/>. </exception>
    /// <exception cref="NotFiniteNumberException"> <paramref name="chance" /> is not a finite number. </exception>
    public static bool NextChance(this Random randomizer, double chance)
    {
        if (randomizer is null) throw new ArgumentNullException(nameof(randomizer));
        if (!chance.IsFinite()) throw new NotFiniteNumberException(nameof(chance));

        return randomizer.NextDouble() < chance;
    }

    /// <summary>
    /// Gets a random <see langword="byte"/> value.
    /// </summary>
    /// <param name="randomizer">The randomizer to use. Cannot be <see langword="null"/>.</param>
    /// <returns>A random <see langword="byte"/> value between 0 (inclusive) and 255 (inclusive).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="randomizer" /> is <see langword="null"/>.</exception>
    public static byte NextByte(this Random randomizer)
    {
        if (randomizer is null) throw new ArgumentNullException(nameof(randomizer));

        return (byte)randomizer.Next(0, 256);
    }

    /// <summary>
    /// Gets a random <see langword="byte"/> value.
    /// </summary>
    /// <param name="randomizer">The randomizer to use. Cannot be <see langword="null"/>.</param>
    /// <param name="max"> The allowed maximum value (inclusive). </param>
    /// <returns>A random <see langword="byte"/> value between 0 (inclusive) and <paramref name="max"/> (inclusive).</returns>
    /// <exception cref="ArgumentNullException"><paramref name="randomizer" /> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="max" /> is less than zero.</exception>
    public static byte NextByte(this Random randomizer, byte max)
    {
        if (randomizer is null) throw new ArgumentNullException(nameof(randomizer));
        if (max < 0) throw new ArgumentOutOfRangeException(nameof(max), "Maximum value must be greater than or equal to zero.");
        
        return (byte)randomizer.Next(0, max + 1);
    }

    /// <summary>
    /// Gets a random <see langword="byte"/> value.
    /// </summary>
    /// <param name="randomizer">The randomizer to use. Cannot be <see langword="null"/>.</param>
    /// <param name="min"> The allowed minimum value (inclusive). </param>
    /// <param name="max"> The allowed maximum value (inclusive). </param>
    /// <returns>A random <see langword="byte"/> value between <paramref name="min"/> (inclusive) and <paramref name="max"/> (inclusive).</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="min" /> is less than zero or <paramref name="max" /> is less than zero. </exception>
    public static byte NextByte(this Random randomizer, byte min, byte max)
    {
        if (randomizer is null) throw new ArgumentNullException(nameof(randomizer));
        if (min < 0) throw new ArgumentOutOfRangeException(nameof(min), "Minimum value must be greater than or equal to zero.");
        if (max < 0) throw new ArgumentOutOfRangeException(nameof(max), "Maximum value must be greater than or equal to zero.");
        return (byte)randomizer.Next(min, max + 1);
    }

    /// <summary>
    /// Fills a byte array with random values.
    /// </summary>
    /// <param name="randomizer"> The randomizer to use. Cannot be <see langword="null"/>. </param>
    /// <param name="buffer"> The byte array to fill. Cannot be <see langword="null"/>. </param>
    /// <param name="offset"> The offset in the byte array at which the random fill starts. </param>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset" /> is less than zero or outside the length of the array. </exception>
    public static void NextBytes(this Random randomizer, byte[] buffer, int offset)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be greater than or equal to zero.");
        if (buffer.Length < offset) throw new ArgumentOutOfRangeException(nameof(buffer), "Offset is outside the length of the array.");

        randomizer.NextBytes(buffer, offset, buffer.Length - offset);
    }

    /// <summary>
    /// Fills a byte array with random values.
    /// </summary>
    /// <param name="randomizer"> The randomizer to use. Cannot be <see langword="null"/>. </param>
    /// <param name="buffer"> The byte array to fill. Cannot be <see langword="null"/>. </param>
    /// <param name="offset"> The offset in the byte array at which the random fill starts. </param>
    /// <param name="count"> The number of bytes to fill with random values, beginning at <paramref name="offset" />. </param>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> or <paramref name="buffer"/> is null. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="offset" /> or <paramref name="count" /> is less than zero or the range specified by <paramref name="offset" /> and <paramref name="count" /> is outside the length of the array. </exception>
    public static void NextBytes(this Random randomizer, byte[] buffer, int offset, int count)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (buffer == null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be greater than or equal to zero.");
        if (buffer.Length < offset) throw new ArgumentOutOfRangeException(nameof(buffer), "Offset is outside the length of the array.");
        if (count < 0) throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than or equal to zero.");
        if (buffer.Length < (offset + count)) throw new ArgumentOutOfRangeException(nameof(buffer), "Offset and count exceed the length of the array.");

        for (int i1 = offset; i1 < (offset + count); i1++)
        {
            buffer[i1] = (byte)randomizer.Next(0, 256);
        }
    }

    /// <summary>
    /// Gets a byte array of a specified length filled with random bytes.
    /// </summary>
    /// <param name="randomizer"> The randomizer to use. Cannot be <see langword="null"/>. </param>
    /// <param name="length"> The number of randomized bytes in the array. Cannot be <see langword="null"/>. </param>
    /// <returns> The byte array which contains the specified number of randomized bytes. </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="length" /> is less than zero. </exception>
    public static byte[] NextBytes(this Random randomizer, int length)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length), "Length must be greater than or equal to zero.");

        byte[] array = new byte[length];
        randomizer.NextBytes(array);
        return array;
    }

    /// <summary>
    ///     Gets a random date and time value.
    /// </summary>
    /// <param name="randomizer"> The randomizer to use. Cannot be <see langword="null"/>.</param>
    /// <returns>A random date and time value between 0001-01-01 00:00:00 (inclusive) and 9999-12-31 23:59:59 (exclusive).</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is <see langword="null"/>. </exception>
    public static DateTime NextDateTime(this Random randomizer)
        => randomizer.NextDateTime(DateTime.MinValue, DateTime.MaxValue);

    /// <summary>
    ///     Gets a random date and time value.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="min"> The allowed minimum date and time (inclusive). </param>
    /// <param name="max"> The allowed maximum date and time (exclusive). </param>
    /// <returns>
    ///     A random date and time value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive).
    /// </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is <see langword="null"/>. </exception>
    public static DateTime NextDateTime(this Random randomizer, DateTime min, DateTime max)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));

        double ticks = randomizer.NextDouble(min.Ticks, max.Ticks);
        return new DateTime((long)ticks);
    }

    /// <summary>
    /// Gets a random time span value.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <returns> A random time span value between 0 (inclusive) and approx. 10'675'199 days (exclusive). </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    public static TimeSpan NextTimeSpan(this Random randomizer)
        => randomizer.NextTimeSpan(TimeSpan.MinValue, TimeSpan.MaxValue);

    /// <summary>
    /// Gets a random time span value.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="min"> The allowed minimum time span (inclusive). </param>
    /// <param name="max"> The allowed maximum time span (exclusive). </param>
    /// <returns> A random time span value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive). </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    public static TimeSpan NextTimeSpan(this Random randomizer, TimeSpan min, TimeSpan max)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));

        double ticks = randomizer.NextDouble(min.Ticks, max.Ticks);
        return new TimeSpan((long)ticks);
    }

    /// <summary>
    /// Gets a random double precision floating point value.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="max"> The allowed maximum value (exclusive). </param>
    /// <returns> A random double precision floating point value between 0.0 (inclusive) and <paramref name="max" /> (exclusive). </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    public static double NextDouble(this Random randomizer, double max)
        => randomizer.NextDouble(0.0, max);

    /// <summary>
    /// Gets a random double precision floating point value.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="min"> The allowed minimum value (inclusive). </param>
    /// <param name="max"> The allowed maximum value (exclusive). </param>
    /// <returns> A random double precision floating point value between <paramref name="min" /> (inclusive) and <paramref name="max" /> (exclusive). </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="NotFiniteNumberException"> <paramref name="min" /> or <paramref name="max" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
    public static double NextDouble(this Random randomizer, double min, double max)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (!min.IsFinite()) throw new NotFiniteNumberException(nameof(min));
        if (!max.IsFinite()) throw new NotFiniteNumberException(nameof(max));

        return min + (randomizer.NextDouble() * (max - min));
    }

    /// <summary>
    /// Returns a sequence of n unique random integers in the range [min, max).
    /// </summary>
    /// <param name="randomizer">The randomizer. Cannot be <see langword="null"/>.</param>
    /// <param name="min">Inclusive minimum value.</param>
    /// <param name="max">Exclusive maximum value.</param>
    /// <param name="count">Number of integers to generate.</param>
    /// <returns>Sequence of unique random integers.</returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="min" />, <paramref name="max" />, or <paramref name="count" /> is invalid. </exception>
    public static List<int> NextUniqueInts(this Random randomizer, int min, int max, int count)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (min > max) throw new ArgumentOutOfRangeException(nameof(min), "Min Must be less than or equal to max.");
        if (count < 0 || count > max - min) throw new ArgumentOutOfRangeException(nameof(count), "Count Must be between 0 and the range of unique values.");

        var pool = Enumerable
            .Range(min, max - min)
            .ToList();

        for (int i = pool.Count - 1; i > 0; i--)
        {
            int j = randomizer.Next(0, i + 1);
            int temp = pool[i];
            pool[i] = pool[j];
            pool[j] = temp;
        }

        return pool
            .Take(count)
            .ToList();
    }

    /// <summary>
    /// Gets a normally distributed random number.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="mu"> The distributions mean. </param>
    /// <param name="sigma"> The standard deviation of the distribution. </param>
    /// <returns> A normally distributed random double precision floating point value. </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="NotFiniteNumberException"> <paramref name="mu" /> or <paramref name="sigma" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
    public static double NextGaussian(this Random randomizer, double mu, double sigma)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (!mu.IsFinite()) throw new NotFiniteNumberException(nameof(mu));
        if (!sigma.IsFinite()) throw new NotFiniteNumberException(nameof(sigma));

        double u1 = randomizer.NextDouble();
        double u2 = randomizer.NextDouble();
        double distribution = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mu + (sigma * distribution);
    }

    /// <summary>
    /// Gets a triangular distributed random number.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="min"> The allowed minimum value. </param>
    /// <param name="max"> The allowed maximum value. </param>
    /// <param name="mode"> The most frequent value. </param>
    /// <returns> A triangular distributed random double precision floating point value. </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="NotFiniteNumberException"> <paramref name="min" />, <paramref name="max" />, or <paramref name="mode" /> is either "NaN"/"Not-a-Number" or infinity (positive or negative). </exception>
    public static double NextTriangular(this Random randomizer, double min, double max, double mode)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (!min.IsFinite()) throw new NotFiniteNumberException(nameof(min));
        if (!max.IsFinite()) throw new NotFiniteNumberException(nameof(max));
        if (!mode.IsFinite()) throw new NotFiniteNumberException(nameof(mode));

        double u = randomizer.NextDouble();

        if (u < ((mode - min) / (max - min)))
        {
            return min + Math.Sqrt(u * (max - min) * (mode - min));
        }

        return max - Math.Sqrt((1 - u) * (max - min) * (max - mode));
    }

    /// <summary>
    /// Returns a random number from the exponential distribution with specified rate (lambda).
    /// </summary>
    /// <param name="random">The randomizer. Cannot be <see langword="null"/>.</param>
    /// <param name="lambda">The rate parameter (lambda &gt; 0).</param>
    /// <returns>An exponentially distributed random number.</returns>
    public static double NextExponential(this Random random, double lambda)
    {
        if (random == null) throw new ArgumentNullException(nameof(random));
        if (lambda <= 0) throw new ArgumentOutOfRangeException(nameof(lambda), "Lambda must be positive.");
        return -Math.Log(1.0 - random.NextDouble()) / lambda;
    }

    /// <summary>
    /// Returns a random number from the log-normal distribution with specified mu and sigma.
    /// </summary>
    /// <param name="random">The randomizer. Cannot be <see langword="null"/>.</param>
    /// <param name="mu">The mean of the underlying normal distribution.</param>
    /// <param name="sigma">The standard deviation of the underlying normal distribution.</param>
    /// <returns>A log-normally distributed random number.</returns>
    public static double NextLogNormal(this Random random, double mu, double sigma)
    {
        if (random == null) throw new ArgumentNullException(nameof(random));
        if (!mu.IsFinite()) throw new NotFiniteNumberException(nameof(mu));
        if (!sigma.IsFinite()) throw new NotFiniteNumberException(nameof(sigma));

        return Math.Exp(random.NextGaussian(mu, sigma));
    }

    /// <summary>
    /// Returns a random string of the specified length, using the given set of allowed characters.
    /// </summary>
    /// <param name="random">The Random instance. Cannot be <see langword="null"/>.</param>
    /// <param name="length">Length of the random string.</param>
    /// <param name="allowedChars">String containing allowed characters.</param>
    /// <returns>A random string.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="random"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="length"/> is negative.</exception>
    /// <exception cref="ArgumentException"><paramref name="allowedChars"/> is empty.</exception>
    public static string NextString(this Random random, int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
    {
        if (random == null) throw new ArgumentNullException(nameof(random));
        if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
        if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("Allowed characters must not be empty.", nameof(allowedChars));

        var sb = new StringBuilder(length);

        for (int i = 0; i < length; i++)
        {
            sb.Append(allowedChars[random.Next(allowedChars.Length)]);
        }

        return sb.ToString();
    }

    /// <summary>
    /// Generates a random sentence of readable text.
    /// </summary>
    /// <param name="randomizer"> The randomizer. Cannot be <see langword="null"/>. </param>
    /// <param name="words"> The number of words in the sentence. </param>
    /// <param name="startWithLoremIpsum"> Indicates whether the sentence should start with &quot;lorem ipsum dolor sit amet&quot;. </param>
    /// <param name="startWithCapital"> Indicates whether the first letter of the sentence should be a capital letter. </param>
    /// <param name="endWithPeriod"> Indicates whether the sentence should end with a period. </param>
    /// <returns> The string with the amount of specified words. </returns>
    /// <exception cref="ArgumentNullException"> <paramref name="randomizer" /> is null. </exception>
    /// <exception cref="ArgumentOutOfRangeException"> <paramref name="words" /> is less than zero or <paramref name="startWithLoremIpsum" /> is true and <paramref name="words" /> is less than five. </exception>
    public static string NextLoremIpsum(this Random randomizer, int words, bool startWithLoremIpsum, bool startWithCapital, bool endWithPeriod)
    {
        if (randomizer == null) throw new ArgumentNullException(nameof(randomizer));
        if (words < 0) throw new ArgumentOutOfRangeException(nameof(words), "Number of words must be zero or more.");
        if (startWithLoremIpsum && (words < 5)) throw new ArgumentOutOfRangeException(nameof(words), "If starting with 'lorem ipsum', at least five words are required.");

        if (words == 0)
        {
            return string.Empty;
        }

        lock (RandomExtensions._loremIpsumGenerateLock)
        {
            if (RandomExtensions._loremIpsumWords.Count == 0)
            {
                StringBuilder currentWord = new StringBuilder();

                for (int i1 = 0; i1 < RandomExtensions._loremIpsumRaw.Length; i1++)
                {
                    char currentChar = RandomExtensions._loremIpsumRaw[i1];

                    if (char.IsLetter(currentChar))
                    {
                        currentWord.Append(char.ToLowerInvariant(currentChar));
                    }
                    else
                    {
                        if (currentWord.Length >= 2)
                        {
                            RandomExtensions._loremIpsumWords.Add(currentWord.ToString());
                        }

                        currentWord.Remove(0, currentWord.Length);
                    }
                }
            }
        }

        StringBuilder result = new StringBuilder();
        int remainingWords = words;
        bool started = false;

        if (startWithLoremIpsum)
        {
            string start = "lorem ipsum dolor sit amet";

            if (startWithCapital)
            {
                start = char.ToUpperInvariant(start[0]) + start.Substring(1);
            }

            result.Append(start);
            remainingWords -= 5;
            started = true;
        }

        for (int i1 = 0; i1 < remainingWords; i1++)
        {
            string word;

            lock (RandomExtensions._loremIpsumGenerateLock)
            {
                word = RandomExtensions._loremIpsumWords[randomizer.Next(0, RandomExtensions._loremIpsumWords.Count)];
            }

            if (!started && startWithCapital)
            {
                word = char.ToUpperInvariant(word[0]) + word.Substring(1);
            }

            if (started)
            {
                result.Append(" ");
            }

            result.Append(word);
            started = true;
        }

        if (endWithPeriod)
        {
            result.Append(".");
        }

        return result.ToString();
    }
}
