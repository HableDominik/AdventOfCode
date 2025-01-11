using System.Text.RegularExpressions;

namespace AoC2023.Days;

public class Day01 : BaseDay
{
    private readonly string[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input.Sum(input => GetFirstAndLastDigitAsNumber(input));

    private int Solve2() => _input.Sum(input => GetFirstAndLastDigitAsNumber(ReplaceNumbers(input)));

    private static int GetFirstAndLastDigitAsNumber(string s)
           => int.Parse(Regex.Match(s, @"\d").Value + Regex.Match(s, @"\d(?=\D*$)").Value);

    private static string ReplaceNumbers(string s)
           => s.Replace("one", "o1e")
               .Replace("two", "t2o")
               .Replace("three", "t3e")
               .Replace("four", "f4r")
               .Replace("five", "f5e")
               .Replace("six", "s6x")
               .Replace("seven", "s7n")
               .Replace("eight", "e8t")
               .Replace("nine", "n9e");
}
