using System;
using System.Drawing;

namespace AoC2024.Days;

public class Day06 : BaseDay
{
    private readonly char[][] _input;

    private readonly List<Point> _originalObstructions = [];

    private readonly Point _guardOrigin;

    private readonly Point _guardOriginDir = new Point(0, -1);

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.ToArray()).ToArray();

        for (int y = 0; y < _input.Length; y++)
        {
            for (int x = 0; x < _input[0].Length; x++)
            {
                if (_input[y][x] == '#') _originalObstructions.Add(new Point(x, y));
                if (_input[y][x] == '^') _guardOrigin = new Point(x, y);
            }
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1().Count}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private HashSet<Point> Solve1()
    {
        Point guard = _guardOrigin;
        Point dir = _guardOriginDir;

        var visited = new HashSet<Point>();

        while (IsOnMap(guard))
        {
            visited.Add(guard);
            var next = new Point(guard.X + dir.X, guard.Y + dir.Y);

            if (_originalObstructions.Contains(next))
            {
                dir = new Point(-dir.Y, dir.X);
                continue;
            }

            guard = next;
        }

        return visited;
    }

    private bool IsOnMap(Point pos)
        => pos.X > 0 && pos.Y > 0 && pos.Y < _input.Length && pos.X < _input[0].Length;

    /// <summary>
    /// Brute forced.
    /// Idea: Don't start from beginning, but from checkpoints (last original obstructions?).
    /// </summary>
    /// <returns>Correct result.</returns>
    private int Solve2()
    {
        var possibleNewObstructions = Solve1();
        var visited = new List<(Point pos, Point dir)>();
        var result = 0;
        
        foreach(var newObstrcution in possibleNewObstructions.Skip(1))
        {
            visited.Clear();
            var guard = _guardOrigin;
            var dir = _guardOriginDir;
            var currentObstructions = _originalObstructions.ToList();
            currentObstructions.Add(newObstrcution);

            while (IsOnMap(guard))
            {
                visited.Add((guard, dir));
                var next = new Point(guard.X + dir.X, guard.Y + dir.Y);

                if (visited.Contains((next, dir)))
                {
                    result++;
                    break;
                }

                if (currentObstructions.Contains(next))
                {
                    dir = new Point(-dir.Y, dir.X);
                    continue;
                }

                guard = next;
            }
        }

        return result;
    }
}
