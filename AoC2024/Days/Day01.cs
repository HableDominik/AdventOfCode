namespace AoC2024.Days;

public class Day01 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1()
    {
        var pairs = _input.Select(line => line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray());

        var sortedIds1 = pairs.Select(pair => pair[0]).OrderBy(id => id).ToList();
        var sortedIds2 = pairs.Select(pair => pair[1]).OrderBy(id => id).ToList();

        return sortedIds1.Zip(sortedIds2, (a, b) => Math.Abs(a - b)).Sum();
    }

    private long Solve2()
    {
        var pairs = _input.Select(line => line
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToArray());

        var ids1 = pairs.Select(line => line[0]).ToList();
        var ids2 = pairs.Select(line => line[1]).ToList();

        var app = ids2
            .GroupBy(id => id)
            .ToDictionary(group => group.Key, group => group.LongCount());

        return ids1.Sum(id => app.TryGetValue(id, out var count) ? count * id : 0);
    }
}
