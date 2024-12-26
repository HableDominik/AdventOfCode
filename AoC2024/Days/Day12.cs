using Spectre.Console;

namespace AoC2024.Days;

public class Day12 : BaseDay
{
    private readonly char[][] _map;
    private readonly int _rows;
    private readonly int _cols;

    public Day12()
    {
        _map = File.ReadAllLines(InputFilePath).Select(line => line.ToArray()).ToArray();

        _rows = _map.Length;
        _cols = _map[0].Length;
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var visited = new HashSet<(int y, int x)>();

        var result1 = 0;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _cols; x++)
            {
                if (visited.Contains((y, x))) continue;

                var (area, perimeter) = ScanRegion(y, x, null, visited);

                result1 += area * perimeter;
            }
        }

        return result1;
    }

    private int Solve2() => 0; // 30765 < x < 836387

    (int area, int perimeter) ScanRegion(int y, int x, char? prev, HashSet<(int y, int x)> visited)
    {
        if (y < 0 || x < 0 || y == _rows || x == _cols) return (0, 1);

        var current = _map[y][x];

        if (prev is not null && current != prev) return (0, 1);

        if (visited.Contains((y, x))) return (0, 0);

        visited.Add((y, x));

        var top = ScanRegion(y - 1, x, current, visited);
        var right = ScanRegion(y, x + 1, current, visited);
        var bottom = ScanRegion(y + 1, x, current, visited);
        var left = ScanRegion(y, x - 1, current, visited);

        var area = 1 + top.area + right.area + bottom.area + left.area;
        var perimeter = top.perimeter + right.perimeter + bottom.perimeter + left.perimeter;

        return (area, perimeter);
    }
}
