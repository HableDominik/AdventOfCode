namespace AoC2024.Days;

public class Day07 : BaseDay
{
    private readonly IEnumerable<(long key, int[] values)> _input;

    public Day07()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.Split([" ", ":"], StringSplitOptions.RemoveEmptyEntries))
            .Select(split => (
                key: long.Parse(split[0]),
                values: split.Skip(1).Select(int.Parse).ToArray()));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1()
        => _input.Where(x => IsCombinable(x.key, 0, x.values, operation: null, withConcat: false))
            .Sum(x => x.key);

    private long Solve2()
        => _input.Where(x => IsCombinable(x.key, 0, x.values, operation: null, withConcat: true))
            .Sum(x => x.key);

    private static bool IsCombinable(
        long result, 
        long current, 
        int[] values, 
        Func<long, int, long>? operation, 
        bool withConcat)
    {
        if (operation is null)
        {
            return IsCombinable(result, values[0], values[1..], Sum, withConcat)
                || IsCombinable(result, values[0], values[1..], Multiply, withConcat)
                || (withConcat 
                    && IsCombinable(result, values[0], values[1..], Concatenation, withConcat));
        }

        current = operation(current, values[0]);

        if (current == result && values.Length == 1)  return true; 

        if (current > result || values.Length == 1) return false;

        return IsCombinable(result, current, values[1..], Sum, withConcat)
            || IsCombinable(result, current, values[1..], Multiply, withConcat)
            || (withConcat && IsCombinable(result, current, values[1..], Concatenation, withConcat));
    }

    private static long Sum(long a, int b) => a + b;

    private static long Multiply(long a, int b) => a * b;

    private static long Concatenation(long a, int b) => long.Parse($"{a}{b}");
}
