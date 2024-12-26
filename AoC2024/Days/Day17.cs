using Spectre.Console;

namespace AoC2024.Days;

public class Day17 : BaseDay
{
    private readonly int[] _program;

    public Day17()
    {
        _program = File.ReadAllText(InputFilePath).Split(',').Select(int.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1(56256477)}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private string Solve1(long regA) => string.Join(',', Solve(regA));

    private long Solve2() => Enumerable.Range(0, 8).Select(n => RecursivelySolve(1, n)).First(x => x > 0);

    private List<int> Solve(long regA)
    {
        long[] register = [regA, 0, 0];
        int pointer = 0;

        var result = new List<int>();

        while (pointer < _program.Length)
        {
            var opcode = _program[pointer];
            var literalOperand = _program[pointer + 1];
            var comboOperand = GetValueFrom(register, literalOperand);

            var registerA = register[0];

            switch (opcode)
            {
                case 0: register[0] = DivideRegister(registerA, comboOperand);
                    break;
                case 1: register[1] ^= literalOperand;
                    break;
                case 2: register[1] = Take3Bit(comboOperand);
                    break;
                case 3: pointer = JumpIfANotZero(registerA, literalOperand, pointer);
                    break;
                case 4: register[1] ^= register[2];
                    break;
                case 5: result.Add(Take3Bit(comboOperand));
                    break;
                case 6: register[1] = DivideRegister(registerA, comboOperand);
                    break;
                case 7: register[2] = DivideRegister(registerA, comboOperand);
                    break;
                default:
                    throw new NotSupportedException($"Opcode {opcode} is not supported.");
            }

            pointer += 2;
        }

        return result;
    }

    private long RecursivelySolve(int depth, long previousValue)
    {
        if (depth > _program.Length) return previousValue;

        var sequence = _program[^depth..];

        for (int currentDigit = 0; currentDigit < 8; currentDigit++)
        {
            long currentValue = previousValue * 8 + currentDigit;

            var solvedSequence = Solve(currentValue);

            if (!solvedSequence.SequenceEqual(sequence)) continue;

            var result = RecursivelySolve(depth + 1, currentValue);

            if (result > 0) return result;
        }

        return -1;
    }

    private static int JumpIfANotZero(long registerA, long literalOperand, int pointer)
        => registerA == 0 ? pointer : (int)literalOperand - 2;

    static int Take3Bit(long comboOperand) 
        => (int)(comboOperand % 8);

    static long DivideRegister(long registerA, long comboOperand) 
        => registerA / (1L << (int)comboOperand);

    static long GetValueFrom(long[] register, long operand)
        => operand switch
            {
                <= 3 => operand,
                <= 6 => register[operand - 4],
                7 => 7,
                _ => throw new NotSupportedException()
            };
}
