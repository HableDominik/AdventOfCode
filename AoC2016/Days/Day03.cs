namespace AoC2016.Days;

public class Day03 : BaseDay
{
    private readonly IEnumerable<int[]> _input1;
    private readonly IEnumerable<int[]> _input2;

    public Day03()
    {
        var lines = File.ReadAllLines(InputFilePath);

        _input1 = lines
            .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse).Order().ToArray());

        _input2 = Enumerable.Range(0, 3)
            .SelectMany(colIndex => lines
                .Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[colIndex])
                .Select(int.Parse)
                .Chunk(3))
            .Select(chunk => chunk.Order().ToArray()) 
            .ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input1.Count(t => t[0] + t[1] > t[2]);

    private int Solve2() => _input2.Count(t => t[0] + t[1] > t[2]);
}
