using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace NthItemLib
{
    public readonly struct ItemWithIndex<T>
    {
        public readonly T Item;
        public readonly int Index;

        public ItemWithIndex(T item, int index)
        {
            Item = item;
            Index = index;
        }
    }

    public static class NthExtensions
    {
        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthLargest<T>(this IReadOnlyList<T> source, int n) where T : IComparable<T>
        {
            return source.NthSmallest(source.Count - 1 - n);
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthSmallest<T>(this IReadOnlyList<T> source, int n) where T : IComparable<T>
        {
            var pool = ArrayPool<int>.Shared.Rent(source.Count);

            try
            {
                var indices = pool.AsSpan(0, source.Count);
                QuickSelect.Iota(indices);
                QuickSelect.Execute(source, indices, n);

                return new ItemWithIndex<T>(source[indices[n]], indices[n]);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(pool);
            }
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthLargest<T>(this Span<T> source, int n) where T : IComparable<T>
        {
            return source.NthSmallest(source.Length - 1 - n);
        }

        /// <param name="n">0 ~ source.Length - 1</param>
        public static ItemWithIndex<T> NthSmallest<T>(this Span<T> source, int n) where T : IComparable<T>
        {
            var pool = ArrayPool<int>.Shared.Rent(source.Length);

            try
            {
                var indices = pool.AsSpan(0, source.Length);
                QuickSelect.Iota(indices);
                QuickSelect.Execute<T>(source, indices, n);

                return new ItemWithIndex<T>(source[indices[n]], indices[n]);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(pool);
            }
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthLargest<T>(this ReadOnlySpan<T> source, int n) where T : IComparable<T>
        {
            return source.NthSmallest(source.Length - 1 - n);
        }

        /// <param name="n">0 ~ source.Length - 1</param>
        public static ItemWithIndex<T> NthSmallest<T>(this ReadOnlySpan<T> source, int n) where T : IComparable<T>
        {
            var pool = ArrayPool<int>.Shared.Rent(source.Length);

            try
            {
                var indices = pool.AsSpan(0, source.Length);
                QuickSelect.Iota(indices);
                QuickSelect.Execute(source, indices, n);

                return new ItemWithIndex<T>(source[indices[n]], indices[n]);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(pool);
            }
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthLargest<T>(this IReadOnlyList<T> source, int n, Comparer<T> comparer)
        {
            return source.NthSmallest(source.Count - 1 - n, comparer);
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthSmallest<T>(this IReadOnlyList<T> source, int n, Comparer<T> comparer) 
        {
            var pool = ArrayPool<int>.Shared.Rent(source.Count);

            try
            {
                var indices = pool.AsSpan(0, source.Count);
                QuickSelect.Iota(indices);
                QuickSelect.Execute(source, indices, n, comparer);

                return new ItemWithIndex<T>(source[indices[n]], indices[n]);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(pool);
            }
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthLargest<T>(this Span<T> source, int n, Comparer<T> comparer) 
        {
            return source.NthSmallest(source.Length - 1 - n, comparer);
        }

        /// <param name="n">0 ~ source.Length - 1</param>
        public static ItemWithIndex<T> NthSmallest<T>(this Span<T> source, int n, Comparer<T> comparer)
        {
            var pool = ArrayPool<int>.Shared.Rent(source.Length);

            try
            {
                var indices = pool.AsSpan(0, source.Length);
                QuickSelect.Iota(indices);
                QuickSelect.Execute<T>(source, indices, n, comparer);

                return new ItemWithIndex<T>(source[indices[n]], indices[n]);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(pool);
            }
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static ItemWithIndex<T> NthLargest<T>(this ReadOnlySpan<T> source, int n, Comparer<T> comparer) 
        {
            return source.NthSmallest(source.Length - 1 - n, comparer);
        }

        /// <param name="n">0 ~ source.Length - 1</param>
        public static ItemWithIndex<T> NthSmallest<T>(this ReadOnlySpan<T> source, int n, Comparer<T> comparer)
        {
            var pool = ArrayPool<int>.Shared.Rent(source.Length);

            try
            {
                var indices = pool.AsSpan(0, source.Length);
                QuickSelect.Iota(indices);
                QuickSelect.Execute(source, indices, n, comparer);

                return new ItemWithIndex<T>(source[indices[n]], indices[n]);
            }
            finally
            {
                ArrayPool<int>.Shared.Return(pool);
            }
        }

        #region Max

        public static ItemWithIndex<T> MaxWithIndex<T>(this IReadOnlyList<T> source) where T : IComparable<T>
        {
            T maxValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Count; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], maxValue) > 0)
                {
                    maxValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(maxValue, index);
        }

        public static ItemWithIndex<T> MaxWithIndex<T>(this Span<T> source) where T : IComparable<T>
        {
            T maxValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], maxValue) > 0)
                {
                    maxValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(maxValue, index);
        }

        public static ItemWithIndex<T> MaxWithIndex<T>(this ReadOnlySpan<T> source) where T : IComparable<T>
        {
            T maxValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], maxValue) > 0)
                {
                    maxValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(maxValue, index);
        }

        public static ItemWithIndex<T> MaxWithIndex<T>(this IReadOnlyList<T> source, Comparer<T> comparer)
        {
            T maxValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Count; i++)
            {
                if (comparer.Compare(source[i], maxValue) > 0)
                {
                    maxValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(maxValue, index);
        }

        public static ItemWithIndex<T> MaxWithIndex<T>(this Span<T> source, Comparer<T> comparer)
        {
            T maxValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (comparer.Compare(source[i], maxValue) > 0)
                {
                    maxValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(maxValue, index);
        }

        public static ItemWithIndex<T> MaxWithIndex<T>(this ReadOnlySpan<T> source, Comparer<T> comparer)
        {
            T maxValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (comparer.Compare(source[i], maxValue) > 0)
                {
                    maxValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(maxValue, index);
        }

        #endregion //Max

        #region Min
        public static ItemWithIndex<T> MinWithIndex<T>(this IReadOnlyList<T> source) where T : IComparable<T>
        {
            T minValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Count; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], minValue) < 0)
                {
                    minValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(minValue, index);
        }

        public static ItemWithIndex<T> MinWithIndex<T>(this Span<T> source) where T : IComparable<T>
        {
            T minValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], minValue) < 0)
                {
                    minValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(minValue, index);
        }

        public static ItemWithIndex<T> MinWithIndex<T>(this ReadOnlySpan<T> source) where T : IComparable<T>
        {
            T minValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (Comparer<T>.Default.Compare(source[i], minValue) < 0)
                {
                    minValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(minValue, index);
        }

        public static ItemWithIndex<T> MinWithIndex<T>(this IReadOnlyList<T> source, Comparer<T> comparer)
        {
            T minValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Count; i++)
            {
                if (comparer.Compare(source[i], minValue) < 0)
                {
                    minValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(minValue, index);
        }

        public static ItemWithIndex<T> MinWithIndex<T>(this Span<T> source, Comparer<T> comparer)
        {
            T minValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (comparer.Compare(source[i], minValue) < 0)
                {
                    minValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(minValue, index);
        }

        public static ItemWithIndex<T> MinWithIndex<T>(this ReadOnlySpan<T> source, Comparer<T> comparer)
        {
            T minValue = source[0];
            int index = 0;

            for (int i = 1; i < source.Length; i++)
            {
                if (comparer.Compare(source[i], minValue) < 0)
                {
                    minValue = source[i];
                    index = i;
                }
            }

            return new ItemWithIndex<T>(minValue, index);
        }

        #endregion // Min
    }

    public static class QuickSelect
    {
        /// <param name="indices"></param>
        public static void Iota(Span<int> indices)
        {
            for (int i = 0; i < indices.Length; i++)
            {
                indices[i] = i;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Swap(Span<int> indices, int i, int j)
        {
            if (i == j) return;

            var tmp = indices[i];
            indices[i] = indices[j];
            indices[j] = tmp;
        }

        private static int Partition<T>(ReadOnlySpan<T> source, Span<int> indices, int begin, int count, Comparer<T> comparer)
        {
            if (count <= 1)
            {
                return begin;
            }

            int l = begin;
            int r = begin + count - 2;
            int endIndex = begin + count - 1;

            T pivot = source[indices[endIndex]];

            while (l <= r)
            {
                while (l < endIndex && comparer.Compare(source[indices[l]], pivot) < 0)
                {
                    l++;
                }

                while (begin <= r && comparer.Compare(source[indices[r]], pivot) >= 0)
                {
                    r--;
                }

                if (r < l)
                {
                    break;
                }

                Swap(indices, l, r);

                l++;
                r--;
            }

            Swap(indices, l, endIndex);

            return l;
        }

        private static int Partition<T>(IReadOnlyList<T> source, Span<int> indices, int begin, int count, Comparer<T> comparer)
        {
            if (count <= 1)
            {
                return begin;
            }

            int l = begin;
            int r = begin + count - 2;
            int endIndex = begin + count - 1;

            T pivot = source[indices[endIndex]];

            while (l <= r)
            {
                while (l < endIndex && comparer.Compare(source[indices[l]], pivot) < 0)
                {
                    l++;
                }

                while (begin <= r && comparer.Compare(source[indices[r]], pivot) >= 0)
                {
                    r--;
                }

                if (r < l)
                {
                    break;
                }

                Swap(indices, l, r);

                l++;
                r--;
            }

            Swap(indices, l, endIndex);

            return l;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Validation<T>(IReadOnlyList<T> source, Span<int> indices, int n)
        {
            if (source.Count <= n)
            {
                throw new ArgumentException("n is bigger than source.Count");
            }

            if (indices.Length < source.Count)
            {
                throw new ArgumentException("source.Count is bigger than indices.Length");
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void Validation<T>(ReadOnlySpan<T> source, Span<int> indices, int n)
        {
            if (source.Length <= n)
            {
                throw new ArgumentException("n is bigger than source.Length");
            }

            if (indices.Length < source.Length)
            {
                throw new ArgumentException("source.Length is bigger than indices.Length");
            }
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static void Execute<T>(IReadOnlyList<T> source, Span<int> indices, int n) where T : IComparable<T>
        {
            Validation(source, indices, n);
            ExecuteCore(source, indices, n, 0, source.Count, Comparer<T>.Default);
        }

        /// <param name="n">0 ~ source.Count - 1</param>
        public static void Execute<T>(IReadOnlyList<T> source, Span<int> indices, int n, Comparer<T> comparer)
        {
            Validation(source, indices, n);
            ExecuteCore(source, indices, n, 0, source.Count, comparer);
        }

        /// <param name="n">0 ~ source.Length - 1</param>
        public static void Execute<T>(ReadOnlySpan<T> source, Span<int> indices, int n) where T : IComparable<T>
        {
            Validation(source, indices, n);
            ExecuteCore(source, indices, n, 0, source.Length, Comparer<T>.Default);
        }

        /// <param name="n">0 ~ source.Length - 1</param>
        public static void Execute<T>(ReadOnlySpan<T> source, Span<int> indices, int n, Comparer<T> comparer)
        {
            Validation(source, indices, n);
            ExecuteCore(source, indices, n, 0, source.Length, comparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ExecuteCore<T>(IReadOnlyList<T> source, Span<int> indices, int n, int begin, int count, Comparer<T> comparer)
        {
            int partition = Partition(source, indices, begin, count, comparer);

            while (partition != n)
            {
                if (partition < n)
                {
                    count = begin + count - partition - 1;
                    begin = partition + 1;
                }
                else if (n < partition)
                {
                    count = partition - begin;
                }

                partition = Partition(source, indices, begin, count, comparer);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void ExecuteCore<T>(ReadOnlySpan<T> source, Span<int> indices, int n, int begin, int count, Comparer<T> comparer)
        {
            int partition = Partition(source, indices, begin, count, comparer);

            while (partition != n)
            {
                if (partition < n)
                {
                    count = begin + count - partition - 1;
                    begin = partition + 1;
                }
                else if (n < partition)
                {
                    count = partition - begin;
                }

                partition = Partition(source, indices, begin, count, comparer);
            }
        }
    }
}