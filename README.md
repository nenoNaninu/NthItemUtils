# NthItemLib

A library for getting the nth smallest value, the nth largest value, etc. from a randomly accessible data structure like `Span<T>`,`ReadOnlySpan<T>`, `IReadOnlyList<T>`.
Internally, it uses QuickSelect as well as C++ std::nth_element(), so the average performance is O(n).

The source code consists of only one file, so you can easily use it by copy and past.

[source code](https://github.com/nenoNaninu/NthItemLib/blob/master/NthItemLib/NthSearch.cs)


# API
The following extension methods are defined in `Span<T>`,`ReadOnlySpan<T>`, `IReadOnlyList<T>`.
The range of n is between 0 and source.Length - 1.
All return values are `struct　ItemWithIndex<T>{ T Item; int index }`.
```cs
NthLargest<T>(this Span<T> source, int n)
NthLargest<T>(this Span<T> source, int n, Comparer<T> comparer)

NthSmallest<T>(this Span<T> source, int n)
NthSmallest<T>(this Span<T> source, int n, Comparer<T> comparer)

MaxWithIndex<T>(this Span<T> source)
MaxWithIndex<T>(this Span<T> source, Comparer<T> comparer)

MinWithIndex<T>(this Span<T> source)
MinWithIndex<T>(this Span<T> source, Comparer<T> comparer)
```

Since QuickSelect is used internally, this is also published as an API.


```cs
public static class QuickSelect
{
    public static void Iota(Span<int> indices);
    
    public static void Execute<T>(ReadOnlySpan<T> source, Span<int> indices, int n);
    public static void Execute<T>(ReadOnlySpan<T> source, Span<int> indices, int n, Comparer<T> comparer);
    
    public static void Execute<T>(IReadOnlyList<T> source, Span<int> indices, int n);
    public static void Execute<T>(IReadOnlyList<T> source, Span<int> indices, int n, Comparer<T> comparer);
}

```
# Example
Extension Method.
```cs
var random = new Random();
var randomSource = Enumerable.Range(0, 200).Select(_ => random.NextDouble() * 50).ToArray();

var order = randomSource.OrderBy(x => x).ToArray();

int n = random.Next(200);

Assert.IsTrue(order[n] == randomSource.AsSpan().NthSmallest(n).Item); // always true.
```

QuickSelect

```cs
ReadOnlySpan<int> source = new int[] { 4, 4, 8, 1, 2, 5, 4, 4, 4, 6, 7, 3 }.AsSpan();
var pool = ArrayPool<int>.Shared.Rent(source.Length);
var indices = pool.AsSpan(0, source.Length);

int n = 6;

QuickSelect.Iota(indices);
QuickSelect.Execute(source, indices, n);

var pivot = source[indices[n]];
for (int i = 0; i < n; i++)
{
    var result = Comparer<double>.Default.Compare(source[indices[i]], pivot);

    Assert.IsTrue(result <= 0); // always true.
}

for (int i = n + 1; i < source.Length; i++)
{
    var result = Comparer<double>.Default.Compare(source[indices[i]], pivot);

    Assert.IsTrue(0 <= result); // always true.
}

ArrayPool<int>.Shared.Return(pool);
```
