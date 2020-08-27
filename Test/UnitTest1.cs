using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using NthItemLib;
using NUnit.Framework;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var random = new Random();

            for (int j = 0; j < 100; j++)
            {
                var randomSource = Enumerable.Range(0, 200).Select(_ => random.NextDouble() * 50).ToArray();

                var order = randomSource.OrderBy(x => x).ToArray();

                int n = random.Next(200);

                Assert.IsTrue(order[n] == randomSource.AsSpan().NthSmallest(n).Item); // always true.
            }
        }

        [Test]
        public void Test2()
        {
            var random = new Random();

            for (int j = 0; j < 100; j++)
            {
                var randomSource = Enumerable.Range(0, 200).Select(_ => random.NextDouble() * 50).ToArray();

                var pool = ArrayPool<int>.Shared.Rent(randomSource.Length);

                var indices = pool.AsSpan(0, randomSource.Length);
                int n = random.Next(200);

                QuickSelect.Iota(indices);
                QuickSelect.Execute(randomSource, indices, n);

                var pivot = randomSource[indices[n]];
                for (int i = 0; i < n; i++)
                {
                    var result = Comparer<double>.Default.Compare(randomSource[indices[i]], pivot);

                    Assert.IsTrue(result < 0);
                }


                for (int i = n; i < randomSource.Length; i++)
                {
                    var result = Comparer<double>.Default.Compare(randomSource[indices[i]], pivot);

                    Assert.IsTrue(0 <= result);
                }

                ArrayPool<int>.Shared.Return(pool);
            }
        }

        [Test]
        public void Test3()
        {
            var random = new Random();

            for (int j = 0; j < 100; j++)
            {
                var randomSource = Enumerable.Range(0, 200).Select(_ => random.Next(0, 100)).ToArray();

                var pool = ArrayPool<int>.Shared.Rent(randomSource.Length);

                var indices = pool.AsSpan(0, randomSource.Length);
                int n = random.Next(200);

                QuickSelect.Iota(indices);
                QuickSelect.Execute(randomSource, indices, n);

                var pivot = randomSource[indices[n]];
                for (int i = 0; i < n; i++)
                {
                    var result = Comparer<double>.Default.Compare(randomSource[indices[i]], pivot);

                    Assert.IsTrue(result <= 0);
                }

                for (int i = n; i < randomSource.Length; i++)
                {
                    var result = Comparer<double>.Default.Compare(randomSource[indices[i]], pivot);

                    Assert.IsTrue(0 <= result);
                }

                ArrayPool<int>.Shared.Return(pool);
            }
        }
        private static void DebugView<T>(ReadOnlySpan<T> vectors, Span<int> indices, int partition) where T : IComparable<T>
        {
            Console.WriteLine("==========Debug View=========");
            for (int i = 0; i < partition; i++)
            {
                Console.WriteLine(vectors[indices[i]]);
            }

            Console.WriteLine($"{vectors[indices[partition]]} <= partition");

            for (int i = partition + 1; i < indices.Length; i++)
            {
                Console.WriteLine(vectors[indices[i]]);
            }

            Console.WriteLine("=============================");
        }

        [Test]
        public void Test4()
        {
            var random = new Random();


            //var randomSource = new int[] { 1, 2, 3, 4, 4, 4, 4, 4, 5, 6, 7, 8 };
            Span<int> randomSource = new int[] { 4, 4, 8, 1, 2, 5, 4, 4, 4, 6, 7, 3 }.AsSpan();

            var pool = ArrayPool<int>.Shared.Rent(randomSource.Length);

            var indices = pool.AsSpan(0, randomSource.Length);
            //int n = random.Next(randomSource.Length);
            int n = 6;

            QuickSelect.Iota(indices);
            QuickSelect.Execute<int>(randomSource, indices, n);
            //or
            //QuickSelect.Execute((ReadOnlySpan<int>)randomSource, indices, n);
            //or
            //QuickSelect.Execute(randomSource, indices, n, Comparer<int>.Default);

            DebugView<int>(randomSource, indices, n);

            var pivot = randomSource[indices[n]];
            for (int i = 0; i < n; i++)
            {
                var result = Comparer<double>.Default.Compare(randomSource[indices[i]], pivot);

                Assert.IsTrue(result <= 0);
            }


            for (int i = n + 1; i < randomSource.Length; i++)
            {
                var result = Comparer<double>.Default.Compare(randomSource[indices[i]], pivot);

                Assert.IsTrue(0 <= result);
            }

            ArrayPool<int>.Shared.Return(pool);
        }


        [Test]
        public void Test5()
        {
            var random = new Random();

            for (int j = 0; j < 100; j++)
            {
                var randomSource = Enumerable.Range(0, 200).Select(_ => random.NextDouble() * 50).ToArray();

                Assert.IsTrue(randomSource.Max() == randomSource.MaxWithIndex().Item);
            }
        }

        [Test]
        public void Test6()
        {
            var random = new Random();

            for (int j = 0; j < 100; j++)
            {
                var randomSource = Enumerable.Range(0, 200).Select(_ => random.NextDouble() * 50).ToArray();

                Assert.IsTrue(randomSource.Min() == randomSource.MinWithIndex().Item);
            }
        }
    }
}