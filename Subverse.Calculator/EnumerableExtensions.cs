namespace Subverse.Calculator
{
    internal static class EnumerableExtensions
    {
        public static T[] PickN<T>(
            this IEnumerable<T> source, Random rng, int N
            ) where T : unmanaged
        {
            T[] R = new T[N]; long i; bool loopFlag = true;
            using IEnumerator<T> S_i = source.GetEnumerator();

            for (i = 0; i < N && loopFlag; i++)
            {
                R[i] = S_i.Current;
                loopFlag = S_i.MoveNext();
            }

            if (!loopFlag) return R[..(int)--i];

            double W = Math.Exp(Math.Log(rng.NextDouble()) / N);
            while (loopFlag)
            {
                long j = i + (int)Math.Floor(Math.Log(rng.NextDouble()) / Math.Log(1 - W)) + 1;
                for (; i < j && loopFlag; i++)
                { loopFlag = S_i.MoveNext(); }

                if (loopFlag)
                {
                    R[rng.Next(N)] = S_i.Current;
                    W *= Math.Exp(Math.Log(rng.NextDouble()) / N);
                }
            }

            return R;
        }
    }
}
