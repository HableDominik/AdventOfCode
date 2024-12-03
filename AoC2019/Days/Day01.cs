namespace AoC2019.Days;

public class Day01 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => _input.Select(int.Parse)
            .Select(CalculateFuel)
            .Sum();

    private int Solve2()
        => _input.Select(int.Parse)
            .Select(CalculateFuelRec)
            .Sum();

    private int CalculateFuel(int mass) => (int)Math.Floor(mass / 3.0) - 2;

    private int CalculateFuelRec(int mass)
    {
        if (mass < 6) return 0;
        var fuel = CalculateFuel(mass);
        return fuel + CalculateFuelRec(fuel);
    }
}

