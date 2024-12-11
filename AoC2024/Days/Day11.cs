

namespace AoC2024.Days;

public class Day11 : BaseDay
{
    private readonly long[] _input;

    private readonly Dictionary<(long, int), long> _stoneDictionary = [];

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath).Split(" ").Select(long.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1() => _input.Sum(n => Blink(n, 25));

    private long Solve2() => _input.Sum(n => Blink(n, 75));

    private long Blink(long number, int blinksLeft)
    {
        if (blinksLeft == 0) return 1;

        if (_stoneDictionary.TryGetValue((number, blinksLeft), out var result))
        {
            return result;
        }

        long futureBlinkCount;

        if (number == 0)
        {
            futureBlinkCount = Blink(1, blinksLeft - 1);
            return SaveIfNotExists(number, blinksLeft, futureBlinkCount);
        }

        if (number.ToString().Length % 2 == 0)
        {
            var (left, right) = SplitNumber(number.ToString());
            futureBlinkCount = Blink(left, blinksLeft - 1) + Blink(right, blinksLeft - 1);
            return SaveIfNotExists(number, blinksLeft, futureBlinkCount);
        }

        futureBlinkCount = Blink(number * 2024, blinksLeft - 1);
        return SaveIfNotExists(number, blinksLeft, futureBlinkCount);
    }

    private static (long left, long right) SplitNumber(string number)
    {
        var mid = number.Length / 2;
        return (long.Parse(number[..mid]), long.Parse(number[mid..]));
    }

    private long SaveIfNotExists(long number, int blinks, long value)
    {
        if (_stoneDictionary.ContainsKey((number, blinks))) return value;

        _stoneDictionary.Add((number, blinks), value);
        return value;
    }

    #region original solutions part1
    private int Solve1Original()
    {
        LinkedList<long> stones = new(_input);

        const int blinks = 25;

        for (var i = 0; i < blinks; i++)
        {
            for (var current = stones.First; current != null; current = current!.Next)
            {
                var number = current.Value;

                if (number == 0)
                {
                    current.Value = 1;
                    continue;
                }

                if (number.ToString().Length % 2 == 0)
                {
                    var (left, right) = SplitNumber(number.ToString());
                    current.Value = left;
                    stones.AddAfter(current, right);
                    current = current.Next;
                    continue;
                }

                current.Value *= 2024;
            }
        }

        return stones.Count;
    }
    #endregion
}
