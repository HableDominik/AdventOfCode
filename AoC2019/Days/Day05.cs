using System.Reflection;

namespace AoC2019.Days;

public class Day05 : BaseDay
{
    private readonly int[] _input;

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath).Split(',').Select(int.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve(1)}");

    public override ValueTask<string> Solve_2() => new($"{Solve(5)}");

    private int Solve(int input)
    {
        var code = (int[])_input.Clone();
        var pointer = 0;
        var result = 0;

        var operations = new Dictionary<int, Action>
        {
            [1] = () => pointer += BinaryOperation(code, pointer, (a,b) => a + b),
            [2] = () => pointer += BinaryOperation(code, pointer, (a, b) => a * b),
            [3] = () => pointer += InputOperation(code, pointer, input),
            [4] = () => pointer += OutputOperation(code, pointer, ref result),
            [5] = () => pointer = JumpOperation(code, pointer, true),
            [6] = () => pointer = JumpOperation(code, pointer, false),
            [7] = () => pointer += BinaryOperation(code, pointer, (a, b) => a < b ? 1 : 0),
            [8] = () => pointer += BinaryOperation(code, pointer, (a, b) => a == b ? 1 : 0),
        };

        while (code[pointer] != 99)
        {
            var op = code[pointer] % 10;

            if (operations.TryGetValue(op, out var operation))
            {
                operation();
            }
            else
            {
                Console.WriteLine("Unknown instruction: " + code[pointer]);
                return -1;
            }
        }

        return result;
    }

    private static (int value1, int value2, int dest) ParseParameters(int[] code, int pointer, bool hasDest)
    {
        var op = code[pointer];

        var mode1 = (op % 1000) / 100 == 1;
        var mode2 = (op % 10000) / 1000 == 1;

        var param1 = code[pointer + 1];
        var param2 = code[pointer + 2];

        var value1 = mode1 ? param1 : code[param1];
        var value2 = mode2 ? param2 : code[param2];

        var dest = hasDest ? code[pointer + 3] : -1;

        return (value1, value2, dest);
    }

    private static int BinaryOperation(int[] code, int pointer, Func<int, int, int> operation)
    {
        var (value1, value2, dest) = ParseParameters(code, pointer, hasDest: true);

        code[dest] = operation(value1, value2);

        return 4;
    }

    private static int JumpOperation(int[] code, int pointer, bool condition)
    {
        var (value1, value2, _) = ParseParameters(code, pointer, hasDest: false);

        return (value1 > 0) == condition ? value2 : pointer + 3;
    }

    private static int InputOperation(int[] code, int pointer, int value)
    {
        var dest = code[pointer + 1];

        code[dest] = value;

        return 2;
    }

    private static int OutputOperation(int[] code, int pointer, ref int result)
    {
        var op = code[pointer];

        var mode = op / 100 == 1;

        var dest = mode ? pointer + 1 : code[pointer + 1];

        result = code[dest];

        return 2;
    }
}
