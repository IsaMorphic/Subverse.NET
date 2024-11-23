namespace Subverse.Calculator
{
    internal static class EnumerableExtensions
    {
        public static IList<T> Pick<T>(
            this IEnumerable<T> source, Random rng, int k
            ) where T : unmanaged
        {
            SortedList<double, T> H = new();
            using IEnumerator<T> S = source.GetEnumerator();

            while (S.MoveNext())
            {
                double r = rng.NextDouble();
                if (H.Count < k)
                {
                    H.Add(r, S.Current);
                }
                else if (r > H.Keys[0]) 
                {
                    H.RemoveAt(0);
                    H.Add(r, S.Current);
                }
            }

            return H.Values;
        }
    }
}
