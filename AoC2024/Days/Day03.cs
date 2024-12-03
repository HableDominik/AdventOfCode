using System.Text.RegularExpressions;

namespace AoC2024.Days;

public partial class Day03 : BaseDay
{
    private readonly string _input;

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => Regex1().Matches(_input)
               .Cast<Match>()
               .Select(MultiplyMatch)
               .Sum();

    private int Solve2()
        => Regex2().Matches(_input)
                .Cast<Match>()
                .Aggregate((true, value: 0), CalculateState).value;

    private int MultiplyMatch(Match match)
        => int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);

    private (bool, int) CalculateState((bool enabled, int value) state, Match match)
        => match.Value switch
        {
            "do()" => (true, state.value),
            "don't()" => (false, state.value),
            _ => state.enabled 
                ? (state.enabled, state.value + MultiplyMatch(match)) 
                : state
        };

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)")]
    private static partial Regex Regex1();

    [GeneratedRegex(@"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)")]
    private static partial Regex Regex2();
}
