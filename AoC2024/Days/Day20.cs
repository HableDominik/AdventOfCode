using Spectre.Console;
using System.ComponentModel;

namespace AoC2024.Days;

public class Day20 : BaseDay
{
    private readonly char[][] _maze;
    private readonly int _rows, _cols;
    private readonly (int dx, int dy)[] _directions;
    private readonly List<(int X, int Y)> _track = [];

    public Day20()
    {
        _maze = File.ReadAllLines(InputFilePath).Select(line => line.ToArray()).ToArray();

        _rows = _maze.Length;
        _cols = _maze[0].Length;

        _directions = [ (0, -1), (1, 0), (0, 1), (-1, 0) ];

        ParseTrack(GetSingleCharPosition(_maze, 'S'));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => GetCheats(GetNeighbors1());

    private int Solve2() => GetCheats(GetNeighbors2());

    private int GetCheats((int dx, int dy, int ds)[] neighbors)
    {
        int currentIndex = 0;
        var result = 0;

        foreach (var (tx, ty) in _track)
        {
            foreach (var (dx, dy, ds) in neighbors)
            {
                var x = tx + dx;
                var y = ty + dy;

                if (x < 0 || y < 0 || x >= _cols || y >= _rows) continue;

                var index = _track.IndexOf((x, y));

                if ((index - currentIndex - ds) >= 100) result++;
            }

            currentIndex++;
        }

        return result;
    }

    private static (int dx, int dy, int ds)[] GetNeighbors1()
        => [(0, -2, 2), (2, 0, 2), (0, 2, 2), (-2, 0, 2)];

    private static (int dx, int dy, int ds)[] GetNeighbors2()
    {
        HashSet<(int, int, int)> neighbors = [];

        for (int y = 0; y <= 20; y++)
        {
            for (int x = 0; x <= 20-y; x++)
            {
                neighbors.Add(( x,  y, x + y));
                neighbors.Add((-x,  y, x + y));
                neighbors.Add(( x, -y, x + y));
                neighbors.Add((-x, -y, x + y));
            }
        }

        return [..neighbors];
    }

    private void ParseTrack((int X, int Y) position)
    {
        if (_maze[position.Y][position.X] is '#') return;

        if (_track.Contains(position)) return;

        _track.Add(position);

        foreach (var (dx, dy) in _directions)
        {
            ParseTrack((position.X + dx, position.Y + dy));
        }
    }

    private static (int, int) GetSingleCharPosition(char[][] map, char term)
        => map.SelectMany((row, rowIndex) => row
                .Select((value, colIndex) => new { value, rowIndex, colIndex }))
            .Where(c => c.value == term)
            .Select(c => (c.colIndex, c.rowIndex))
            .Single();
}