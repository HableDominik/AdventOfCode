using Microsoft.VisualBasic;
using Spectre.Console;
using System.Drawing;

namespace AoC2024.Days;

public class Day15 : BaseDay
{
    private readonly string[] _mapLines;
    private readonly char[] _movements;

    public Day15()
    {
        var input = File.ReadAllLines(InputFilePath);
        var separatorIndex = Array.FindIndex(input, string.IsNullOrWhiteSpace);
        _mapLines = input.Take(separatorIndex).ToArray();
        var movementLines = input.Skip(separatorIndex + 1).ToArray();
        _movements = [.. string.Join(string.Empty, movementLines)];
    }

    private char[] DoubleMap(string line)
        => line
            .Replace("#", "##")
            .Replace("O", "[]")
            .Replace(".", "..")
            .Replace("@", "@.")         
            .ToCharArray();

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var map = _mapLines.Select(line => line.ToArray()).ToArray();

        var pos = GetStartingPosition(map);

        foreach(var movement in _movements)
        {
            var dir = GetDirection(movement);

            if (MoveSingle(pos.X + dir.X, pos.Y + dir.Y, dir, '@', map))
            {
                map[pos.Y][pos.X] = '.';
                pos.Offset(dir);
            }
        }

        return CalculateResult(map, 'O');
    }

    private int Solve2()
    {
        var map = _mapLines.Select(DoubleMap).ToArray();

        var pos = GetStartingPosition(map);

        foreach (var movement in _movements)
        {
            var dir = GetDirection(movement);

            if (Move(pos.X + dir.X, pos.Y + dir.Y, dir, '@', map, executeMove: false))
            {
                Move(pos.X + dir.X, pos.Y + dir.Y, dir, '@', map, executeMove: true);
                map[pos.Y][pos.X] = '.';
                pos.Offset(dir);
            }
        }

        return CalculateResult(map, '[');
    }

    private static bool MoveSingle(int x, int y, Point dir, char prev, char[][] map)
    {
        var current = map[y][x];

        if (current == '#') return false;

        if (current == '.')
        {
            map[y][x] = prev;
            return true;
        }

        if (MoveSingle(x + dir.X, y + dir.Y, dir, current, map))
        {
            map[y][x] = prev;
            return true;
        }

        return false;
    }

    private static bool Move(int x, int y, Point dir, char prev, char[][] map, bool executeMove)
    {
        var current = map[y][x];

        if (current == '#') return false;

        if (current == '.')
        {
            if (executeMove) map[y][x] = prev;
            return true;
        }

        if (dir.Y == 0)
        {
            if (Move(x + dir.X, y, dir, current, map, executeMove))
            {
                if (executeMove) map[y][x] = prev;
                return true;
            }
            return false;
        }

        var xOffset = current == '[' ? 1 : -1;
        var neighbor = current == '[' ? ']' : '[';

        if (Move(x, y + dir.Y, dir, current, map, executeMove)
            && Move(x + xOffset, y + dir.Y, dir, neighbor, map, executeMove))
        {
            if (executeMove)
            {
                map[y][x] = prev;
                map[y][x + xOffset] = '.';
            }

            return true;
        }

        return false;
    }

    private static Point GetDirection(char movement)
        => movement switch
            {
                '^' => new Point(0, -1),
                '>' => new Point(1, 0),
                'v' => new Point(0, 1),
                '<' => new Point(-1, 0),
                _ => throw new ArgumentException($"Invalid movement: {movement}")
            };

    private static Point GetStartingPosition(char[][] map)
        => map
            .SelectMany((row, rowIndex) => row
                .Select((value, colIndex) => new { value, rowIndex, colIndex }))
            .Where(c => c.value == '@')
            .Select(c => new Point(c.colIndex, c.rowIndex))
            .Single();

    private static int CalculateResult(char[][] map, char box)
        => map
            .SelectMany((row, rowIndex) => row
                .Select((value, colIndex) => new { value, rowIndex, colIndex }))
            .Where(c => c.value == box)
            .Select(c => 100 * c.rowIndex + c.colIndex)
            .Sum();
}
