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

    private int Solve2()
    {
        return 0;
    }
}
