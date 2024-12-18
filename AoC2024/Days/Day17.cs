using System.Reflection;

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

    private string Solve1(long regA)
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

        return string.Join(',', result);
    }

    private int Solve2() => 0;

    private static int JumpIfANotZero(long registerA, long literalOperand, int pointer)
        => registerA == 0 ? pointer : (int)literalOperand - 2;

    static int Take3Bit(long comboOperand) => (int)(comboOperand % 8);

    static long DivideRegister(long registerA, long comboOperand)
    {
        var numerator = registerA;
        var denominator = Math.Pow(2, comboOperand);
        return (long)Math.Truncate(numerator / denominator);
    }

    static long GetValueFrom(long[] register, long operand)
        => operand switch
            {
                <= 3 => operand,
                <= 6 => register[operand - 4],
                7 => 7,
                _ => throw new NotSupportedException()
            };
}
