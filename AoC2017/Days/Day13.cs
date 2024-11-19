namespace AoC2017.Days;

public class Day13 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day13()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => ParseLayers()
            .Where(l => IsCaught(l.Depth, l.Range, 0))
            .Sum(l => l.Depth * l.Range);

    private int Solve2()
    {
        var layers = ParseLayers();
        return Enumerable.Range(0, int.MaxValue)
            .First(delay => layers
                .All(l => !IsCaught(l.Depth, l.Range, delay)));
    }

    private static bool IsCaught(int depth, int range, int delay = 0)
        => (depth + delay) % (2 * range - 2) == 0;

    private List<(int Depth, int Range)> ParseLayers()
        =>_input
            .Select(line =>
            {
                var parts = line.Split(':');
                return (int.Parse(parts[0]), int.Parse(parts[1]));
            })
            .ToList();
}
