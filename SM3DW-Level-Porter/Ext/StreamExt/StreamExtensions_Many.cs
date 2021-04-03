using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Syroot.BinaryData
{
    public partial class StreamExtensions
    {
        // ---- Iterators ----

        /// <summary>
        /// Invokes the <paramref name="action"/> as many times as there are elements in the <paramref name="enumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="enumerable">The <see cref="IEnumerable"/> to iterate over.</param>
        /// <param name="action">The callback invoked for every element, receiving the current index.</param>
        /// <returns>The current stream instance.</returns>
        public static T For<T>(this T stream, IEnumerable enumerable, Action<int> action)
            where T : Stream
        {
            int i = 0;
            IEnumerator enumerator = enumerable.GetEnumerator();
            while (enumerator.MoveNext())
                action(i++);
            return stream;
        }

        /// <summary>
        /// Invokes the <paramref name="action"/> for every element in the <paramref name="enumerable"/>.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <typeparam name="TItem">The type of the elements.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="enumerable">The <see cref="IEnumerable"/> to iterate over.</param>
        /// <param name="action">The callback invoked for every element, receiving the current element.</param>
        /// <returns>The current stream instance.</returns>
        public static T ForEach<T, TItem>(this T stream, IEnumerable<TItem> enumerable, Action<TItem> action)
            where T : Stream
        {
            foreach (TItem item in enumerable)
                action(item);
            return stream;
        }

        // ---- Read ----

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="TItem"/> read from the stream by
        /// calling the <paramref name="readCallback"/> until all elements are read.
        /// </summary>
        /// <typeparam name="TItem">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readCallback">A callback returning the element read.</param>
        /// <returns>The read instances.</returns>
        public static TItem[] Read<TItem>(this Stream stream, int count, Func<TItem> readCallback)
            => ReadMany(stream, count, readCallback);

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="TItem"/> read asynchronously from the
        /// stream by calling the <paramref name="readCallback"/> until all elements are read.
        /// </summary>
        /// <typeparam name="TItem">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readCallback">A callback returning the element read.</param>
        /// <returns>The read instances.</returns>
        /// <param name="ct">The token to monitor for cancellation requests.</param>
        public static async Task<TItem[]> ReadAsync<TItem>(this Stream stream, int count, Func<Task<TItem>> readCallback,
            CancellationToken ct = default)
            => await ReadManyAsync(stream, count, readCallback, ct);

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="TItem"/> continually read from the
        /// stream by calling the <paramref name="readCallback"/>.
        /// </summary>
        /// <typeparam name="TItem">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readCallback">A callback returning the element read.</param>
        /// <returns>The read instances.</returns>
#pragma warning disable IDE0060 // Remove unused parameter, these methods are meant as Stream extensions
        public static TItem[] ReadMany<TItem>(this Stream stream, int count, Func<TItem> readCallback)
        {
            TItem[] values = new TItem[count];
            for (int i = 0; i < values.Length; i++)
                values[i] = readCallback();
            return values;
        }

        /// <summary>
        /// Returns <paramref name="count"/> instances of type <typeparamref name="TItem"/> read asynchronously from the
        /// stream by calling the <paramref name="readCallback"/> until all elements are read.
        /// </summary>
        /// <typeparam name="TItem">The type of the instances to read.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="count">The number of instances to read.</param>
        /// <param name="readCallback">A callback returning the element read.</param>
        /// <returns>The read instances.</returns>
        /// <param name="ct">The token to monitor for cancellation requests.</param>
        public static async Task<TItem[]> ReadManyAsync<TItem>(this Stream stream, int count, Func<Task<TItem>> readCallback,
            CancellationToken ct = default)
        {
            TItem[] values = new TItem[count];
            for (int i = 0; i < count; i++, ct.ThrowIfCancellationRequested())
                values[i] = await readCallback();
            return values;
        }
#pragma warning restore IDE0060

        // ---- Write ----

        /// <summary>
        /// Writes the <paramref name="values"/> to the stream through the <paramref name="writeCallback"/> invoked for
        /// each value.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <typeparam name="TItem">The type of the instances to write.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">A callback receiving the element to write.</param>
        /// <returns>The current stream instance.</returns>
        public static T Write<T, TItem>(this T stream, IEnumerable<TItem> values, Action<TItem> writeCallback)
            where T : Stream
            => WriteMany(stream, values, writeCallback);

        /// <summary>
        /// Writes the <paramref name="values"/> to the stream asynchronously through the
        /// <paramref name="writeCallback"/> invoked for each value.
        /// </summary>
        /// <typeparam name="TItem">The type of the instances to write.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">A callback receiving the element to write.</param>
        /// <param name="ct">The token to monitor for cancellation requests.</param>
        public static async Task WriteAsync<TItem>(this Stream stream, IEnumerable<TItem> values, Func<TItem, Task> writeCallback,
            CancellationToken ct = default)
            => await WriteManyAsync(stream, values, writeCallback, ct);

        /// <summary>
        /// Writes the <paramref name="values"/> to the stream through the <paramref name="writeCallback"/> invoked for
        /// each value.
        /// </summary>
        /// <typeparam name="T">The type of the extended <see cref="Stream"/> instance.</typeparam>
        /// <typeparam name="TItem">The type of the instances to write.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">A callback receiving the element to write.</param>
        /// <returns>The current stream instance.</returns>
        public static T WriteMany<T, TItem>(this T stream, IEnumerable<TItem> values, Action<TItem> writeCallback)
            where T : Stream
        {
            foreach (TItem value in values)
                writeCallback(value);
            return stream;
        }

        /// <summary>
        /// Writes the <paramref name="values"/> to the stream asynchronously through the
        /// <paramref name="writeCallback"/> invoked for each value.
        /// </summary>
        /// <typeparam name="TItem">The type of the instances to write.</typeparam>
        /// <param name="stream">The extended <see cref="Stream"/> instance.</param>
        /// <param name="values">The values to write.</param>
        /// <param name="writeCallback">A callback receiving the element to write.</param>
        /// <param name="ct">The token to monitor for cancellation requests.</param>
#pragma warning disable IDE0060 // Remove unused parameter, these methods are meant as Stream extensions
        public static async Task WriteManyAsync<TItem>(this Stream stream, IEnumerable<TItem> values, Func<TItem, Task> writeCallback,
            CancellationToken ct = default)
        {
            foreach (TItem value in values)
            {
                ct.ThrowIfCancellationRequested();
                await writeCallback(value);
            }
        }
#pragma warning restore IDE0060
    }
}
