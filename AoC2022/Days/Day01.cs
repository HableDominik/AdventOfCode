namespace AoC2022.Days;

public class Day01 : BaseDay
{
    private readonly IEnumerable<IEnumerable<int>> _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split([$"{Environment.NewLine}{Environment.NewLine}"], StringSplitOptions.RemoveEmptyEntries)
            .Select(chunk => chunk.Split($"{Environment.NewLine}", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input.Max(chunk => chunk.Sum());

    private int Solve2() => _input.Select(chunk => chunk.Sum()).OrderDescending().Take(3).Sum();
}
