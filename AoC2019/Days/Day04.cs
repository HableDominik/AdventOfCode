namespace AoC2019.Days;

public class Day04 : BaseDay
{
    private readonly int _min;
    private readonly int _max;

    public Day04()
    {
        var input = File.ReadAllText(InputFilePath).Split('-');
        _min = int.Parse(input[0]); 
        _max = int.Parse(input[1]); 
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => Enumerable.Range(_min, _max - _min + 1)
            .Select(number => number.ToString())
            .Count(number => DigitsNeverDecrease(number) &&HasAdjacentSameDigits(number));
    
    private int Solve2()
        => Enumerable.Range(_min, _max - _min + 1)
            .Select(number => number.ToString())
            .Count(number => DigitsNeverDecrease(number) 
                && HasAdjacentSameDigits(number)
                && HasADoubleDigit(number));

    public static bool DigitsNeverDecrease(string number)
        => number.Zip(number.Skip(1), (a, b) => a <= b).All(x => x);

    public static bool HasAdjacentSameDigits(string number)
        => number.Zip(number.Skip(1), (a, b) => a == b).Any(x => x);

    public static bool HasADoubleDigit(string number)
        => number.GroupBy(c => c).Any(g => g.Count() == 2);
}
