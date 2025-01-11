namespace AoC2015.Days;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input.Length - 2 * _input.Count(c => c is ')');

    private int Solve2()
    {
        var floor = 0;

        for(int i = 1; i <= _input.Length; i++)
        {
            floor += _input[i - 1] == '(' ? 1 : -1;

            if (floor == -1) return i;
        }

        throw new InvalidOperationException("No solution found.");
    }
}