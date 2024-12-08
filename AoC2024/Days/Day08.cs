using System.Collections.Generic;
using System.Drawing;

namespace AoC2024.Days;

public class Day08 : BaseDay
{
    private readonly char[][] _map;

    private readonly Dictionary<char, List<Point>> _antennas = [];


    public Day08()
    {
        _map = File.ReadAllLines(InputFilePath)
            .Select(line => line.ToArray()).ToArray();

        _antennas = _map
            .SelectMany((row, y) => row.Select((cell, x) => (cell, point: new Point(x, y))))
            .Where(item => item.cell != '.')
            .GroupBy(item => item.cell, item => item.point)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => _antennas.Values
            .SelectMany(GetCombinations)
            .SelectMany(combination =>
                {
                    var distance = CalculateDistance(combination.First, combination.Second);

                    return new[]
                    {
                        AddDistance(combination.First, distance),
                        AddDistance(combination.Second, distance, -1),
                    };
                })
            .ToHashSet()
            .Count(IsOnMap);

    private int Solve2()
        => _antennas.Values
            .SelectMany(GetCombinations)
            .SelectMany(combination =>
                {
                    var distance = CalculateDistance(combination.First, combination.Second);

                    return GenerateAntinodes(combination.First, distance)
                        .Concat(GenerateAntinodes(combination.Second, distance, -1));
                })
            .ToHashSet()
            .Count;

    private IEnumerable<Point> GenerateAntinodes(Point start, Point distance, int direction = 1)
    {
        var current = start;
        while (IsOnMap(current))
        {
            yield return current;
            current = AddDistance(current, distance, direction);
        }
    }

    private bool IsOnMap(Point pos)
        => pos.X >= 0 && pos.Y >= 0 && pos.Y < _map.Length && pos.X < _map[0].Length;

    private static Point CalculateDistance(Point first, Point second)
        => new (first.X - second.X, first.Y - second.Y);

    private static Point AddDistance(Point antenna, Point distance, int dir = 1)
        => new (antenna.X + dir * distance.X, antenna.Y + dir * distance.Y);

    private static List<(Point First, Point Second)> GetCombinations(List<Point> antennas)
    {
        return antennas
            .SelectMany((antenna1, i) => antennas
                .Skip(i + 1)
                .Select(antenna2 => (antenna1, antenna2)))
            .ToList();
    }
}
