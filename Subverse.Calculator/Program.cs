// See https://aka.ms/new-console-template for more information
using Subverse.Calculator;
using System.Numerics;

int N, K;
using StreamWriter writer = File.CreateText("results.csv");
writer.AutoFlush = true;
writer.WriteLine("N,K,P");
for (N = 2; N <= 8; N++)
{
    ulong ident = BitMatrix.Identity(N).Bits;
    IEnumerable<ulong> PermuteBits(int M, int K)
    {
        if (K == 0)
        {
            yield return 0UL;
            yield break;
        }

        for (int i = K - 1; i < M; i++)
        {
            foreach (ulong v in PermuteBits(i, K - 1))
            {
                ulong r = (1UL << i) | v;
                if ((r & ident) == 0)
                {
                    yield return r;
                }
            }
        }
    }

    for (K = 1; K <= 12; K++)
    {
        long totalPopCount = 0, totalPermCount = 0;
        Parallel.ForEach(PermuteBits(N * N, K),
            (v, ct) =>
            {
                BitMatrix A = new(N, v);
                BitMatrix R_n = BitMatrix.Warshall(A);

                Interlocked.Add(ref totalPopCount, BitOperations.PopCount(R_n.Bits & ~ident));
                Interlocked.Increment(ref totalPermCount);
            });
        writer.WriteLine($"{N},{K},{totalPopCount / (double)(totalPermCount * N * (N - 1))}");
    }
}
