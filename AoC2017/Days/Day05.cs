namespace AoC2017.Days;

public class Day05 : BaseDay
{
    private readonly IEnumerable<int> _input;

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath)
            .Split('\n')
            .Select(int.Parse);
    }

    public override ValueTask<string> Solve_1() => new($"{SolveCommon(_ => false)}");

    public override ValueTask<string> Solve_2() => new($"{SolveCommon(x => x >= 3)}");

    #region Common solution
    private int SolveCommon(Func<int, bool> func)
    {
        var input = _input.ToArray();
        int pos = 0, steps = 0;
        while (pos >= 0 && pos < input.Length)
        {
            steps++;
            var jump = input[pos];
            input[pos] += func(jump) ? -1 : 1;
            pos += jump;
        }
        return steps;
    }
    #endregion

    #region Original solutions
    private int Solve1()
    {
        var input = _input.ToArray();
        int pos = 0, steps = 0;
        while (pos >= 0 && pos < input.Length)
        {
            steps++;
            input[pos]++;
            pos += input[pos] - 1;
        }
        return steps;
    }

    private int Solve2()
    {
        var input = _input.ToArray();
        int pos = 0, steps = 0;
        while (pos >= 0 && pos < input.Length)
        {
            steps++;
            var jump = input[pos];
            input[pos] += jump >= 3 ? -1 : 1;
            pos += jump;
        }
        return steps;
    }
    #endregion
}
