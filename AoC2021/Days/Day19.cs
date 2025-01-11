using Spectre.Console;

namespace AoC2021.Days;

public class Day19 : BaseDay
{
    private readonly string[] _input;

    public Day19()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => 0;

    private int Solve2() => 0;
}
