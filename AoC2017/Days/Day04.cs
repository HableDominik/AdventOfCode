namespace AoC2017.Days;

public class Day04 : BaseDay
{
    private readonly IEnumerable<IEnumerable<string>> _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split('\n')
            .Select(line => line
                .Trim()
                .Split(' ', '\t'));

    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => _input.Count(line => line
            .Distinct().Count() == line.Count());

    private int Solve2()
         => _input
            .Count(line => line
                .Select(word => new string(word.OrderBy(c => c).ToArray()))
                .Distinct().Count() == line.Count());
}
