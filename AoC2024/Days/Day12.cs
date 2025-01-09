using Spectre.Console;

namespace AoC2024.Days;

public class Day12 : BaseDay
{
    private readonly char[][] _map;
    private readonly int[][] _edges;
    private readonly int[][] _corners;
    private readonly int _rows;
    private readonly int _cols;
    private readonly (int dy, int dx)[] _neighbors;

    public Day12()
    {
        _map = File.ReadAllLines(InputFilePath).Select(line => line.ToArray()).ToArray();

        _rows = _map.Length;
        _cols = _map[0].Length;

        _edges = Enumerable.Range(0, _rows).Select(_ => new int[_cols]).ToArray();
        _corners = Enumerable.Range(0, _rows).Select(_ => new int[_cols]).ToArray();

        _neighbors = [ (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1) ];

        FindEdgesAndCorners();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    public int Solve1() => Solve((y, x) => _edges[y][x]);

    public int Solve2() => Solve((y, x) => _corners[y][x]);   

    private int Solve(Func<int, int, int> metricSelector)
    {
        var visited = new HashSet<(int y, int x)>();
        int result = 0;

        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _cols; x++)
            {
                if (visited.Contains((y, x))) continue;

                var (area, metric) = ScanRegion(y, x, null, visited, metricSelector);

                result += area * metric;
            }
        }

        return result;
    }

    private (int area, int metric) ScanRegion(
        int y, int x,
        char? prev,
        HashSet<(int y, int x)> visited,
        Func<int, int, int> metricSelector)
    {
        if (y < 0 || x < 0 || y == _rows || x == _cols) return (0, 0);

        var current = _map[y][x];

        if (prev is not null && current != prev) return (0, 0);

        if (visited.Contains((y, x))) return (0, 0);

        visited.Add((y, x));

        var top = ScanRegion(y - 1, x, current, visited, metricSelector);
        var right = ScanRegion(y, x + 1, current, visited, metricSelector);
        var bottom = ScanRegion(y + 1, x, current, visited, metricSelector);
        var left = ScanRegion(y, x - 1, current, visited, metricSelector);

        var area = 1 + top.area + right.area + bottom.area + left.area;
        var metric = top.metric + right.metric + bottom.metric + left.metric + metricSelector(y, x);

        return (area, metric);
    }

    private void FindEdgesAndCorners()
    {
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _cols; x++)
            {
                var (edges, corners) = GetEdgesAndCorners(y, x);
                _edges[y][x] = edges;
                _corners[y][x] = corners;
            }
        }
    }

    private (int edges, int corners) GetEdgesAndCorners(int y, int x)
    {
        char center = _map[y][x];
        var surrounding = _neighbors.Select(offset =>
        {
            int nbY = y + offset.dy;
            int nbX = x + offset.dx;
            return (nbY >= 0 && nbY < _rows && nbX >= 0 && nbX < _cols) ? _map[nbY][nbX] : (char?)null;
        }).ToArray();

        return (GetEdgeCount(center, surrounding), GetCornerCount(center, surrounding));

    }

    private static int GetEdgeCount(char center, char?[] surrounding)
        => surrounding
            .Where((neighbor, index) => index % 2 == 0 && neighbor != center)
            .Count();

    private static int GetCornerCount(char center, char?[] surrounding)
        => Enumerable.Range(1, 4)
            .Select(i => i * 2 - 1)
            .Count(i =>
            {
                int prev = (i + 7) % 8;
                int next = (i + 9) % 8;
                return (surrounding[i] != center && surrounding[prev] == center && surrounding[next] == center)
                       || (surrounding[prev] != center && surrounding[next] != center);
            });
}