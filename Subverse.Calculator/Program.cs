// See https://aka.ms/new-console-template for more information
using Subverse.Calculator;
using System.Numerics;
using System.Security.Cryptography;

using StreamWriter writer = File.CreateText("results.csv");
writer.AutoFlush = true;
writer.WriteLine("N,K,P");
Parallel.For(2, 9, N =>
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

    Parallel.For(1, 13, K =>
    {
        long totalPopCount = 0, totalPermCount = 0;
        foreach (ulong v in PermuteBits(N * N, K)
            .Where(x => N < 6 || K < 8 || RandomNumberGenerator.GetInt32(1 << (K + 9)) == 0))
        {
            BitMatrix A = new(N, v);
            BitMatrix R_n = BitMatrix.Warshall(A);

            totalPopCount += BitOperations.PopCount(R_n.Bits & ~ident);
            ++totalPermCount;
        }

        string line = $"{N},{K},{totalPopCount / (double)(totalPermCount * N * (N - 1))}";
        Console.WriteLine(line);

        lock (writer)
        {
            writer.WriteLine(line);
        }
    });
});
