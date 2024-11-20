using System.Linq;

namespace AoC2017.Days;

public class Day10 : BaseDay
{
    private readonly string _input;

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var lengths = _input.Split(',').Select(int.Parse);
        const int max = 256;
        var list = Enumerable.Range(0, max).ToList();
        var skip = 0;
        var offset = 0;

        foreach(var length in lengths)
        {
            var rotated = list.Skip(offset).Concat(list.Take(offset));
            var reversed = rotated.Take(length).Reverse().Concat(rotated.Skip(length));
            list = reversed.Skip(max-offset).Concat(reversed.Take(max-offset)).ToList();
            offset = (offset + length + skip) % max;
            skip++;
        }

        return list[0] * list[1];
    }

    private string Solve2()
    {
        var lengths = _input.Select(c => (int)c).ToList();
        lengths.AddRange([17, 31, 73, 47, 23]);

        const int max = 256;
        var list = Enumerable.Range(0, max).ToList();
        var skip = 0;
        var offset = 0;

        for (var i = 0; i < 64; i++)
        {
            foreach (var length in lengths)
            {
                var rotated = list.Skip(offset).Concat(list.Take(offset));
                var reversed = rotated.Take(length).Reverse().Concat(rotated.Skip(length));
                list = reversed.Skip(max - offset).Concat(reversed.Take(max - offset)).ToList();
                offset = (offset + length + skip) % max;
                skip++;
            }
        }

        var batches = Enumerable.Range(0, list.Count / 16)
            .Select(i => list.Skip(i * 16).Take(16));

        return string.Join("", batches
            .Select(batch => batch.Aggregate((x, y) => x ^ y))
            .Select(hash => hash.ToString("x2")));
    }
}
