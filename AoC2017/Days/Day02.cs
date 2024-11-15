namespace AoC2017.Days;

public class Day02 : BaseDay
{
    private readonly IEnumerable<IEnumerable<int>> _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split('\n')
            .Select(line => line
                .Split(' ', '\t')
                .Select(int.Parse)
        );
    }

    public override ValueTask<string> Solve_1() => new($"{SolveCommon(MinMaxDiff)}");

    public override ValueTask<string> Solve_2() => new($"{SolveCommon(EvenlyDivisible)}");

    private int SolveCommon(Func<IEnumerable<int>, int> func)
        => _input
            .Select(line => func(line))
            .Sum();

    private int MinMaxDiff(IEnumerable<int> values)
        => values.Max() - values.Min();

    private static int EvenlyDivisible(IEnumerable<int> values)
        => (from a in values
            from b in values
            where a != b && b % a == 0
            select b / a).FirstOrDefault();

    private int Solve1() => _input
        .Select(line => line.Max() - line.Min())
        .Sum();

    private int Solve2() => _input
        .Select(line => EvenlyDivisible(line))
        .Sum();

}
