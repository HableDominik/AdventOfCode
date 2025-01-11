namespace AoC2015.Days;

public class Day : BaseDay
{
    private readonly string _input;

    public Day()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        return 0;
    }

    private int Solve2()
    {
        return 0;
    }
}
