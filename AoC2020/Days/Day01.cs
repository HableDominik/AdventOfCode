namespace AoC2020.Days;

public class Day01 : BaseDay
{
    private readonly int[] _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath).Select(int.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = i + 1; j < _input.Length; j++)
            {
                if ((_input[i] + _input[j]) == 2020)
                {
                    return _input[i] * _input[j];
                }
            }
        }

        throw new InvalidOperationException("No solution found.");
    }

    private int Solve2()
    {
        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = i + 1; j < _input.Length; j++)
            {
                for (int k = j + 1; k < _input.Length; k++)
                {
                    if ((_input[i] + _input[j] + _input[k]) == 2020)
                    {
                        return _input[i] * _input[j] * _input[k];
                    }
                }
            }
        }

        throw new InvalidOperationException("No solution found.");
    }
}
