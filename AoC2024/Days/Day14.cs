using System.Drawing;
using System.Text;

namespace AoC2024.Days;

public class Day14 : BaseDay
{
    private readonly IEnumerable<(Point Pos, Point Vel)> _robots;

    private const int Width = 101;
    private const int Height = 103;

    public Day14()
    {
        _robots = File.ReadAllLines(InputFilePath)
            .Select(line =>
            {
                var parts = line.Split('=', ',', ' ');
                var pos = new Point(int.Parse(parts[1]), int.Parse(parts[2]));
                var vel = new Point(int.Parse(parts[4]), int.Parse(parts[5]));
                return (pos, vel);
            });
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var endPos = _robots.Select(robo => CalculatePositionAfter(robo, 100)).ToList();

        int vertical = Width / 2;
        int horizontal = Height / 2;

        var northeast = endPos.Count(p => p.X > vertical && p.Y < horizontal);
        var southeast = endPos.Count(p => p.X > vertical && p.Y > horizontal);
        var southwest = endPos.Count(p => p.X < vertical && p.Y > horizontal);
        var northwest = endPos.Count(p => p.X < vertical && p.Y < horizontal);

        return northeast * southeast * southwest * northwest;
    }

    private static Point CalculatePositionAfter((Point Pos, Point Vel) robot, int time)
    {
        var x = robot.Pos.X + robot.Vel.X * time;
        var y = robot.Pos.Y + robot.Vel.Y * time;

        x = (x + Width * time) % Width;
        y = (y + Height * time) % Height;

        return new Point(x, y);
    }

    private static int Solve2()
    {
        // EXPLANATION
        // 
        // I noticed 2 patterns, a vertical and a horizontal one.
        // Those patterns reappeared in a constant cycle (width & height).
        // I thought that they might overlap at a point and calculated that.
        // 
        // linear function : y = k * x + d
         
        const int k1 = Width;  // = vertical pattern cycle
        const int d1 = 22;     // = first appearance of vertical pattern

        const int k2 = Height; // = horizontal pattern cycle
        const int d2 = 98;     // = first appearance of horizontal pattern

        // intersection formula
        var x = (d2 - d1) / (k1 - k2);

        // calcualte y with linear function
        var y = k1 * x + d1;

        // y is negative, so add a full cycle to get the first positive.
        var fullCycle = k1 * k2;

        return y + fullCycle;
    }
}
