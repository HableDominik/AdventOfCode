namespace AoC2017.Days;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{SolveCommon(1)}");

    public override ValueTask<string> Solve_2() => new($"{SolveCommon(_input.Length / 2)}");

    private int SolveCommon(int offset)
    {
        var input = _input + _input[..offset];
        return input
            .Zip(input.Skip(offset), (current, next) => current == next ? current - '0' : 0)
            .Sum();
    }

    private int Solve1()
    {
        var input = _input + _input[0];
        return input
            .Zip(input.Skip(1), (current, next) => current == next ? current - '0' : 0)
            .Sum();
    }

    private int Solve2()
    {
        var halfLen = _input.Length / 2;
        var input = _input + _input[..halfLen];
        return input
            .Zip(input.Skip(halfLen), (current, next) => current == next ? current - '0' : 0)
            .Sum();
    }
}
