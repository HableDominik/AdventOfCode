namespace AoC2019.Days;

public class Day02 : BaseDay
{
    private readonly int[] _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath).Split(',').Select(int.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => Solve(12, 2);

    private int Solve2()
    {
        const int wanted = 19690720;

        for (int noun = 0; noun < 100; noun++)
        {
            for (int verb = 0; verb < 100; verb++)
            {
                if (Solve(noun, verb) == wanted)
                {
                    return 100 * noun + verb;
                }
            }
        }

        return -1;
    }

    private int Solve(int noun, int verb)
    {
        var intcode = (int[])_input.Clone();
        intcode[1] = noun;
        intcode[2] = verb;
        var pointer = 0;

        while (intcode[pointer] != 99)
        {
            intcode[intcode[pointer + 3]] = intcode[pointer] == 1
                ? intcode[intcode[pointer + 1]] + intcode[intcode[pointer + 2]]
                : intcode[intcode[pointer + 1]] * intcode[intcode[pointer + 2]];

            pointer += 4;
        }

        return intcode[0];
    }
}
