namespace AoC2024.Days;

public class Day22 : BaseDay
{
    private readonly long[] _secrets;

    public Day22()
    {
        _secrets = File.ReadAllLines(InputFilePath).Select(long.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1() => _secrets.Select(secret => Evolve(secret, 2000)).Sum();

    private int Solve2()
    {
        var firstSequenceBananas = new Dictionary<int, int>();
        var foundSequences = new HashSet<int>();
        int i, last, prev, change, encodedSequence;

        foreach(var secret in _secrets)
        {
            var current = secret;
            foundSequences.Clear();
            prev = (int)(secret % 10);
            encodedSequence = AppendAndPrune(0, prev);

            for (i = 0; i < 2000; i++)
            {
                current = Evolve(current);
                last = (int)(current % 10);
                change = last - prev;
                prev = last;
                encodedSequence = AppendAndPrune(encodedSequence, change);

                if (HasFourValues(encodedSequence))
                {
                    if (!foundSequences.Add(encodedSequence)) continue;                   

                    if (firstSequenceBananas.TryGetValue(encodedSequence, out int currentValue))
                    {
                        firstSequenceBananas[encodedSequence] = currentValue + last;
                    }
                    else
                    {
                        firstSequenceBananas[encodedSequence] = last;
                    }
                }
            }
        }

        return firstSequenceBananas.Values.Max(); ;
    }

    private static long Evolve(long secret, int cycles)
    {
        while (cycles-- > 0) secret = Evolve(secret);
        return secret;
    }

    private static long Evolve(long secret)
    {
        secret ^= (secret << 6) & 0xFFFFFF;
        secret ^= (secret >> 5) & 0xFFFFFF;
        secret ^= (secret << 11) & 0xFFFFFF;
        return secret;
    }

    private static int AppendAndPrune(int sequence, int number)
        => ((sequence << 5) | (number & 0b11111)) & 0xFFFFF;

    private static bool HasFourValues(int encodedSeqeunce) 
        => (encodedSeqeunce & 0xFFFFF) >= (1 << 15);
}
