using System.Collections.Generic;

namespace AoC2017.Days;

public class Day08 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day08()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var registers = new Dictionary<string, int>();

        foreach (var line in _input)
        {
            var split = line.Split(' ');

            if (!registers.TryGetValue(split[4], out int compVal)) compVal = 0;

            if (Compare(compVal, split[5], int.Parse(split[6])))
            {
                var sign = split[1] == "inc" ? 1 : -1;

                var value = sign * int.Parse(split[2]);

                if (registers.TryGetValue(split[0], out int val)) value += val;

                registers[split[0]] = value;
            }
        }

        return registers.Values.Max();
    }

    private int Solve2()
    {
        var registers = new Dictionary<string, int>();
        var max = 0;

        foreach (var line in _input)
        {
            var split = line.Split(' ');

            if (!registers.TryGetValue(split[4], out int compVal)) compVal = 0;

            if (Compare(compVal, split[5], int.Parse(split[6])))
            {
                var sign = split[1] == "inc" ? 1 : -1;

                var value = sign * int.Parse(split[2]);

                if (registers.TryGetValue(split[0], out int val)) value += val;

                if (value > max) max = value;

                registers[split[0]] = value;
            }
        }

        return max;
    }

    private static bool Compare(int left, string comp, int right)
        => comp switch
        {
            ">" => left > right,
            ">=" => left >= right,
            "==" => left == right,
            "<=" => left <= right,
            "<" => left < right,
            "!=" => left != right,
            _ => throw new ArgumentException($"Invalid comparison operator: {comp}")
        };
}
