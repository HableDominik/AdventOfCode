using System.Reflection;

namespace AoC2019.Days;

public class Day05 : BaseDay
{
    private readonly int[] _input;

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath).Split(',').Select(int.Parse).ToArray();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var code = (int[])_input.Clone();
        const int input = 1;
        var pntr = 0;

        while (code[pntr] != 99)
        {
            switch(code[pntr])
            {
                case 1: pntr +=
                        Add(code, code[pntr + 1], false, code[pntr + 2], false, code[pntr + 3]); 
                    break;
                case 2:
                    pntr +=
                        Multiply(code, code[pntr + 1], false, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 3: pntr += Input(code, code[pntr + 1], input);
                    break; 
                case 4: var (jump, result) = Output(code, code[pntr + 1], false);
                    if (result != -1) return result;
                    pntr += jump;
                    break;
                case 104: (jump, result) = Output(code, code[pntr + 1], true);
                    if (result != -1) return result;
                    pntr += jump;
                    break;
                case 101: pntr +=
                        Add(code, code[pntr + 1], true, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 102: pntr +=
                        Multiply(code, code[pntr + 1], true, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 1001: pntr +=
                        Add(code, code[pntr + 1], false, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1002: pntr +=
                        Multiply(code, code[pntr + 1], false, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1101: pntr +=
                        Add(code, code[pntr + 1], true, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1102: pntr +=
                        Multiply(code, code[pntr + 1], true, code[pntr + 2], true, code[pntr + 3]);
                    break;
                default: Console.WriteLine("Missing Instruction: " + code[pntr]); return -1;
            }
        }

        return -1;
    }

    private static int Input(int[] code, int pointer, int value)
    {
        code[pointer] = value;
        return 2;
    }

    private static (int jump, int result) Output(int[] code, int pointer, bool pointerImmediateMode)
    {
        var value = pointerImmediateMode ? pointer : code[pointer];
        
        if (value != 0)
        {
            return (0, value);
        }

        return (2,-1);
    }

    private static int Add(int[] code, 
        int param1Pointer, bool param1ImmediateMode, 
        int param2Pointer, bool param2ImmediateMode, 
        int destinationPointer)
    {
        var value1 = param1ImmediateMode ? param1Pointer : code[param1Pointer];
        var value2 = param2ImmediateMode ? param2Pointer : code[param2Pointer];
        code[destinationPointer] = value1 + value2;

        return 4;
    }

    private static int Multiply(int[] code,
        int param1Pointer, bool param1ImmediateMode,
        int param2Pointer, bool param2ImmediateMode,
        int destinationPointer)
    {
        var value1 = param1ImmediateMode ? param1Pointer : code[param1Pointer];
        var value2 = param2ImmediateMode ? param2Pointer : code[param2Pointer];
        code[destinationPointer] = value1 * value2;

        return 4;
    }

    private int Solve2()
    {
        var code = (int[])_input.Clone();
        const int input = 5;
        var pntr = 0;

        while (code[pntr] != 99)
        {
            switch (code[pntr])
            {
                case 3:
                    pntr += Input(code, code[pntr + 1], input);
                    break;
                case 4:
                    var (jump, result) = Output(code, code[pntr + 1], false);
                    if (result != -1) return result;
                    pntr += jump;
                    break;
                case 104:
                    (jump, result) = Output(code, code[pntr + 1], true);
                    if (result != -1) return result;
                    pntr += jump;
                    break;
                case 1:
                    pntr +=        Add(code, code[pntr + 1], false, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 2:
                    pntr +=   Multiply(code, code[pntr + 1], false, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 5:
                    pntr =  JumpIfTrue(code, code[pntr + 1], false, code[pntr + 2], false, pntr);
                    break;
                case 6:
                    pntr = JumpIfFalse(code, code[pntr + 1], false, code[pntr + 2], false, pntr);
                    break;
                case 7:
                    pntr +=    LessThan(code, code[pntr + 1], false, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 8:
                    pntr +=      Equals(code, code[pntr + 1], false, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 101:
                    pntr +=        Add(code, code[pntr + 1], true, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 102:
                    pntr +=   Multiply(code, code[pntr + 1], true, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 105:
                    pntr =  JumpIfTrue(code, code[pntr + 1], true, code[pntr + 2], false, pntr);
                    break;
                case 106:
                    pntr = JumpIfFalse(code, code[pntr + 1], true, code[pntr + 2], false, pntr);
                    break;
                case 107:
                    pntr +=    LessThan(code, code[pntr + 1], true, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 108:
                    pntr +=      Equals(code, code[pntr + 1], true, code[pntr + 2], false, code[pntr + 3]);
                    break;
                case 1001:
                    pntr +=        Add(code, code[pntr + 1], false, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1002:
                    pntr +=   Multiply(code, code[pntr + 1], false, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1005:
                    pntr =  JumpIfTrue(code, code[pntr + 1], false, code[pntr + 2], true, pntr);
                    break;
                case 1006:
                    pntr = JumpIfFalse(code, code[pntr + 1], false, code[pntr + 2], true, pntr);
                    break;
                case 1007:
                    pntr +=    LessThan(code, code[pntr + 1], false, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1008:
                    pntr +=      Equals(code, code[pntr + 1], false, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1101:
                    pntr +=        Add(code, code[pntr + 1], true, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1102:
                    pntr +=   Multiply(code, code[pntr + 1], true, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1105:
                    pntr =  JumpIfTrue(code, code[pntr + 1], true, code[pntr + 2], true, pntr);
                    break;
                case 1106:
                    pntr = JumpIfFalse(code, code[pntr + 1], true, code[pntr + 2], true, pntr);
                    break;
                case 1107:
                    pntr +=    LessThan(code, code[pntr + 1], true, code[pntr + 2], true, code[pntr + 3]);
                    break;
                case 1108:
                    pntr +=      Equals(code, code[pntr + 1], true, code[pntr + 2], true, code[pntr + 3]);
                    break;
                default: Console.WriteLine("Missing Instruction: " + code[pntr]); return -1;
            }
        }

        return 0;
    }

    private static int JumpIfTrue(int[] code,
        int param1Pointer, bool param1ImmediateMode,
        int param2Pointer, bool param2ImmediateMode,
        int pointer)
    {
        var value1 = param1ImmediateMode ? param1Pointer : code[param1Pointer];
        var value2 = param2ImmediateMode ? param2Pointer : code[param2Pointer];

        if (value1 != 0) return value2;

        return pointer + 3;
    }

    private static int JumpIfFalse(int[] code,
        int param1Pointer, bool param1ImmediateMode,
        int param2Pointer, bool param2ImmediateMode,
        int pointer)
    {
        var value1 = param1ImmediateMode ? param1Pointer : code[param1Pointer];
        var value2 = param2ImmediateMode ? param2Pointer : code[param2Pointer];

        if (value1 == 0) return value2;

        return pointer + 3;
    }

    private static int LessThan(int[] code,
        int param1Pointer, bool param1ImmediateMode,
        int param2Pointer, bool param2ImmediateMode,
        int destinationPointer)
    {
        var value1 = param1ImmediateMode ? param1Pointer : code[param1Pointer];
        var value2 = param2ImmediateMode ? param2Pointer : code[param2Pointer];

        code[destinationPointer] = value1 < value2 ? 1 : 0;

        return 4;
    }

    private static int Equals(int[] code,
        int param1Pointer, bool param1ImmediateMode,
        int param2Pointer, bool param2ImmediateMode,
        int destinationPointer)
    {
        var value1 = param1ImmediateMode ? param1Pointer : code[param1Pointer];
        var value2 = param2ImmediateMode ? param2Pointer : code[param2Pointer];

        code[destinationPointer] = value1 == value2 ? 1 : 0;

        return 4;
    }
}
