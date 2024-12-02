namespace AoC2024.Days;

public class Day02 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => ParseReports().Count(report => IsValid(report).isValid);

    private int Solve2()
        => ParseReports().Count(IsSafeWithSingleError);

    private static bool IsSafeWithSingleError(List<int> levels)
    {
        var (isValid, errorIndex) = IsValid(levels);
        if (isValid) return true;

        if (errorIndex == 1 && IsValid(levels[1..]).isValid) return true;
    
        levels.RemoveAt(errorIndex);
        return IsValid(levels).isValid;
    }

    private static (bool isValid, int errorIndex) IsValid(List<int> values)
    {
        bool isIncreasing = values[0] < values[^1];

        for (int i = 0; i < values.Count - 1; ++i)
        {
            int curr = values[i];
            int next = values[i + 1];

            if (Math.Abs(next - curr) is < 1 or > 3 || (curr < next != isIncreasing))
            {
                return (false, i + 1);
            }
        }
        return (true, -1);
    }

    List<List<int>> ParseReports()
        => _input.Select(line => line.Split(' ').Select(int.Parse).ToList()).ToList();
}
