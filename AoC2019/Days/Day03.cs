using System.Drawing;

namespace AoC2019.Days;

public class Day03 : BaseDay
{
    private readonly string[][] _input;

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(lines => lines.Split(',')).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var wire1 = TraceWire(_input[0]);
        var wire2 = TraceWire(_input[1]);

        var intersections = wire1.Intersect(wire2);

        return intersections.Min(point => Math.Abs(point.X) + Math.Abs(point.Y));
    }

    private int Solve2()
    {
        var wire1 = TraceWireWithDistance(_input[0]);
        var wire2 = TraceWireWithDistance(_input[1]);

        var intersections = wire1.Keys.Intersect(wire2.Keys);

        return intersections.Min(point => wire1[point] + wire2[point]);
    }

    private static HashSet<Point> TraceWire(IEnumerable<string> wirePaths)
    {
        var wire = new HashSet<Point>();
        var curr = new Point(0, 0);

        foreach (var path in wirePaths)
        {
            var dir = GetDirection(path[0]);
            var steps = int.Parse(path[1..]);
            for (int i = 0; i < steps; i++)
            {
                curr.Offset(dir);
                wire.Add(curr);
            }
        }

        return wire;
    }

    private static Dictionary<Point, int> TraceWireWithDistance(IEnumerable<string> wirePaths)
    {
        var wire = new Dictionary<Point, int>();
        var curr = new Point(0, 0);
        int totalSteps = 0;

        foreach (var path in wirePaths)
        {
            var dir = GetDirection(path[0]);
            var steps = int.Parse(path[1..]);
            for (int i = 0; i < steps; i++)
            {
                curr.Offset(dir);
                totalSteps++;

                if (!wire.ContainsKey(curr))
                {
                    wire[curr] = totalSteps;
                }
            }
        }

        return wire;
    }

    private static Point GetDirection(char ch)
        => ch switch
        {
            'R' => new Point(1, 0),
            'L' => new Point(-1, 0),
            'D' => new Point(0, 1),
            'U' => new Point(0, -1),
            _ => throw new ArgumentException()
        };
}
