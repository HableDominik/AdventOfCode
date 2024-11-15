using System.Runtime.ExceptionServices;

namespace AoC2017.Days;

public class Day06 : BaseDay
{
    private readonly IEnumerable<int> _input;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split('\t')
            .Select(int.Parse);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => Solve().Count -1 ;

    private int Solve2()
    {
        var knownBanks = Solve();
        return knownBanks!.Count - 1 - knownBanks.IndexOf(knownBanks.Last());
    }

    private List<string> Solve()
    {
        var banks = _input.ToArray();
        var knownBanks = new List<string>() { string.Join('-', banks) };
        var cycles = 0;
        var size = banks.Length;

        while (true)
        {
            var pos = Array.FindIndex(banks, bank => bank == banks.Max());
            var blocks = banks[pos];
            banks[pos] = 0;

            // This could be optimized by adding blocks/size but idc right now.
            while (blocks > 0)
            {
                pos = (pos + 1) % size;
                banks[pos]++;
                blocks--;
            }

            cycles++;

            var bankState = string.Join('-', banks);
            if (knownBanks.Contains(bankState))
            {
                knownBanks.Add(bankState);
                return knownBanks;
            }
            knownBanks.Add(bankState);         
        }
    }
}
