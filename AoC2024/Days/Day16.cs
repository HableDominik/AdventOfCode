
using Spectre.Console;

namespace AoC2024.Days;

public class Day16 : BaseDay
{
    private readonly char[][] _maze;
    private readonly (int X, int Y) _start;
    private readonly (int X, int Y) _end;
    private readonly (int dx, int dy)[] _directions;
    private readonly Comparer<(int x, int y, int cost, int dir)> _comparer;

    public Day16()
    {
        _maze = File.ReadAllLines(InputFilePath).Select(line => line.ToArray()).ToArray();

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

    private int Solve2() => -1;

    private int FindCheapestPath(char[][] maze, (int X, int Y) start, (int X, int Y) end)
    {
        int rows = _maze.Length;
        int cols = _maze[0].Length;

        var prioQueue = new SortedSet<(int x, int y, int cost, int dir)>(_comparer);

        var costMap = new int[cols, rows, 4];
        for (int y = 0; y < rows; y++)
            for (int x = 0; x < cols; x++)
                for (int dir = 0; dir < 4; dir++)
                    costMap[x, y, dir] = int.MaxValue;

        prioQueue.Add((start.X, start.Y, 0, 1));
        costMap[start.X, start.Y, 1] = 0;

        while(prioQueue.Count > 0)
        {
            var (x, y, currentCost, dir) = prioQueue.Min;
            prioQueue.Remove(prioQueue.Min);

            if ((x, y) == end) return currentCost;

            if (currentCost > costMap[x, y, dir]) continue;

            for (int dirOffset = -1; dirOffset <= 1; dirOffset++)
            {
                int turnCost = dirOffset * dirOffset * 1000;
                var newDir = (dir + dirOffset + 4) % 4;
                int newX = x + _directions[newDir].dx;
                int newY = y + _directions[newDir].dy;

                if (maze[newY][newX] == '#') continue;
                
                int newCost = currentCost + 1 + turnCost;

                if (newCost >= costMap[newX, newY, newDir]) continue;

                costMap[newX, newY, newDir] = newCost;
                prioQueue.Add((newX, newY, newCost, newDir));
            }
        }

        return -1;
    }

    private static (int, int) GetSingleCharPosition(char[][] map, char term)
    => map
        .SelectMany((row, rowIndex) => row
            .Select((value, colIndex) => new { value, rowIndex, colIndex }))
        .Where(c => c.value == term)
        .Select(c => (c.colIndex, c.rowIndex))
        .Single();
}
