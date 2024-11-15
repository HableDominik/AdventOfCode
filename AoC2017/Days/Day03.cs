using System.Drawing;

namespace AoC2017.Days;

public class Day03 : BaseDay
{
    private readonly int _input;

    private static readonly (int dx, int dy)[] Neighbors =
    {
        (-1,  1), (0, -1), (1,  1),
        (-1,  0),          (1,  0),
        (-1, -1), (0,  1), (1, -1)
    };

    public Day03()
    {
        _input = int.Parse(File.ReadAllText(InputFilePath));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var current = new Point(0, 0);
        var dir = new Point(1, 0);
        int steps = 1;
        int currentSteps = 0;
        bool first = true;

        foreach (var i in Enumerable.Range(2, _input - 1))
        {
            current.Offset(dir);
            currentSteps++;
            if (currentSteps == steps)
            {
                dir = new Point(-dir.Y, dir.X);
                currentSteps = 0;
                if (!first) steps++;
                first = !first;
            }
        }

        return Math.Abs(current.X) + Math.Abs(current.Y);
    }

    private int Solve2()
    {
        var current = new Point(0, 0);
        var dir = new Point(1, 0);
        int steps = 1;
        int currentSteps = 0;
        bool first = true;
        var values = new Dictionary<Point, int> { { current, 1 } };

        foreach (var i in Enumerable.Range(2, _input - 1))
        {
            current.Offset(dir);
            var currentValue = GetNeighborValuesSum(values, current);
            if (currentValue > _input) return currentValue;
            values.Add(current, currentValue);
            currentSteps++;
            if (currentSteps == steps)
            {
                dir = new Point(-dir.Y, dir.X);
                currentSteps = 0;
                if (!first) steps++;
                first = !first;
            }
        }

        return -1;
    }

    private static int GetNeighborValuesSum(Dictionary<Point, int> values, Point current)
        => Neighbors
            .Select(nb => new Point(current.X + nb.dx, current.Y + nb.dy))
            .Where(nb => values.ContainsKey(nb))
            .Sum(nb => values[nb]);
}
