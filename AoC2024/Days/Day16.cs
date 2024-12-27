
using Spectre.Console;

namespace AoC2024.Days;

public class Day16 : BaseDay
{
    private readonly char[][] _maze;
    private readonly int[,,] _costMap;
    private readonly int _rows, _cols;
    private readonly (int X, int Y) _start, _end;
    private readonly (int dx, int dy)[] _directions;
    private readonly Comparer<(int x, int y, int cost, int dir)> _comparer;

    public Day16()
    {
        _maze = File.ReadAllLines(InputFilePath).Select(line => line.ToArray()).ToArray();

        _rows = _maze.Length;
        _cols = _maze[0].Length;

        _costMap = new int[_cols, _rows, 4];

        for (int y = 0; y < _rows; y++)
            for (int x = 0; x < _cols; x++)
                for (int dir = 0; dir < 4; dir++)
                    _costMap[x, y, dir] = int.MaxValue;

        _start = GetSingleCharPosition(_maze, 'S');
        _end = GetSingleCharPosition(_maze, 'E');

        _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        _comparer = Comparer<(int x, int y, int cost, int dir)>.Create((a, b) =>
            a.cost != b.cost ? a.cost.CompareTo(b.cost) :
            a.x != b.x ? a.x.CompareTo(b.x) :
            a.y != b.y ? a.y.CompareTo(b.y) :
            a.dir.CompareTo(b.dir));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => FindCheapestPath(_maze, _start, _end);

    private int Solve2()
    {
        var visited = new HashSet<(int X, int Y)>();

        Backtrack(_end, GetMinCosts(_end.X, _end.Y).Min() + 1, visited);

        return visited.Count;
    }

    private int FindCheapestPath(char[][] maze, (int X, int Y) start, (int X, int Y) end)
    {
        var prioQueue = new SortedSet<(int x, int y, int cost, int dir)>(_comparer)
        {
            (start.X, start.Y, 0, 1)
        };

        _costMap[start.X, start.Y, 1] = 0;

        while(prioQueue.Count > 0)
        {
            var (x, y, currentCost, dir) = prioQueue.Min;
            prioQueue.Remove(prioQueue.Min);

            if ((x, y) == end) return currentCost;

            if (currentCost > _costMap[x, y, dir]) continue;

            for (int dirOffset = -1; dirOffset <= 1; dirOffset++)
            {
                int turnCost = dirOffset * dirOffset * 1000;
                var newDir = (dir + dirOffset + 4) % 4;
                int newX = x + _directions[newDir].dx;
                int newY = y + _directions[newDir].dy;

                if (maze[newY][newX] == '#') continue;
                
                int newCost = currentCost + 1 + turnCost;

                if (newCost >= _costMap[newX, newY, newDir]) continue;

                _costMap[newX, newY, newDir] = newCost;
                prioQueue.Add((newX, newY, newCost, newDir));
            }
        }

        return -1;
    }

    private void Backtrack((int X, int Y) current, int prevCost, HashSet<(int X, int Y)> visited)
    {
        if (visited.Contains(current)) return;

        var currentCosts = GetMinCosts(current.X, current.Y);
        if (currentCosts.Length == 0) return;

        foreach (var currentCost in currentCosts)
        {
            var cost = prevCost - currentCost;
            if (cost is not 1 && cost is not 1001) continue;

            visited.Add(current);

            foreach (var (dx, dy) in _directions)
            {
                Backtrack((current.X + dx, current.Y + dy), currentCost, visited);
            }
        }
    }

    private int[] GetMinCosts(int x, int y)
        => Enumerable.Range(0, 4)
            .Select(d => _costMap[x, y, d])
            .Where(cost => cost is not int.MaxValue)
            .ToArray();

    private static (int, int) GetSingleCharPosition(char[][] map, char term)
    => map
        .SelectMany((row, rowIndex) => row
            .Select((value, colIndex) => new { value, rowIndex, colIndex }))
        .Where(c => c.value == term)
        .Select(c => (c.colIndex, c.rowIndex))
        .Single();
}
