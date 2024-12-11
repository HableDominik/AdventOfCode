
using System.Drawing;

namespace AoC2024.Days;

public class Day10 : BaseDay
{
    private readonly int[][] _input;

    private readonly int _rows, _cols;

    private readonly (int dy, int dx)[] _neighborOffsets =
        [ ( 0, -1), ( 0, 1), ( 1, 0), (-1, 0) ];

    public Day10()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.Select(ch => ch - '0').ToArray())
            .ToArray();

        _rows = _input.Length;
        _cols = _input[0].Length;
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => Enumerable.Range(0, _rows)
            .SelectMany(y => Enumerable.Range(0, _cols)
                .Where(x => _input[y][x] == 0)
                .Select(x =>
                {
                    var trailheads = new HashSet<Point>();
                    HikeRec(y, x, 0, trailheads);
                    return trailheads.Count;
                }))
            .Sum();

    private int Solve2()
        => Enumerable.Range(0, _rows)
            .SelectMany(y => Enumerable.Range(0, _cols)
                .Where(x => _input[y][x] == 0)
                .Select(x => HikeRec(y, x, 0, [])))
            .Sum();

    private int HikeRec(int y, int x, int wantedHeight, HashSet<Point> trailheads)
    {
        if (y < 0 || x < 0 || y >= _rows || x >= _cols) return 0;

        if (_input[y][x] != wantedHeight) return 0;

        if (_input[y][x] == 9)
        {
            trailheads.Add(new Point(x, y));
            return 1;
        }

        wantedHeight++;

        return _neighborOffsets
            .Sum(offset => HikeRec(y + offset.dy, x + offset.dx, wantedHeight, trailheads));
    }
}
