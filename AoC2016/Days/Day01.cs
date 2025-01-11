
namespace AoC2016.Days;

public class Day01 : BaseDay
{
    private readonly IEnumerable<(char, int)> _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split(", ")
            .Select(x => (x[0], int.Parse(x[1..])));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var pos = (0, 0);
        var dir = (0, -1);

        foreach (var (turn, steps) in _input)
        {
            dir = Turn(turn, dir);
            pos = Move(pos, dir, steps);
        }

        return GetManhattanDistance(pos);
    }
    private int Solve2()
    {
        var pos = (0, 0);
        var dir = (0, -1);
        var visited = new HashSet<(int, int)>();

        foreach (var (turn, steps) in _input)
        {
            dir = Turn(turn, dir);

            for (int step = 0; step < steps; step++)
            {
                pos = Move(pos, dir, 1);

                if (!visited.Add(pos))
                {
                    return GetManhattanDistance(pos);
                }
            }
        }

        throw new InvalidOperationException("No solution found.");
    }

    private static int GetManhattanDistance((int x, int y) pos) => Math.Abs(pos.x) + Math.Abs(pos.y);

    private static (int, int) Move((int x, int y) pos, (int x, int y) dir, int steps)
        => (pos.x + dir.x * steps, pos.y + dir.y * steps);

    private static (int, int) Turn(char turn, (int x, int y) dir)
        => turn switch
        {
            'R' => (dir.y, -dir.x),
            'L' => (-dir.y, dir.x),
            _ => throw new NotSupportedException()
        };
}
