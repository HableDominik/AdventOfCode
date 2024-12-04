namespace AoC2024.Days;

public class Day04 : BaseDay
{
    private readonly char[][] _input;

    private readonly (int y, int x)[] neighborOffsets =
    [
        (-1, -1), (-1, 0), (-1, 1),
        ( 0, -1),          ( 0, 1),
        ( 1, -1), ( 1, 0), ( 1, 1)
    ];

    public Day04()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.ToArray()).ToArray() ;
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => Enumerable.Range(0, _input.Length)
            .SelectMany(y => Enumerable.Range(0, _input[0].Length)
            .Where(x => IsSpecificChar(y, x, 'X'))
            .Select(x => CheckXMAS(y, x, 0, 0, null)))
            .Sum();

    private int Solve2()
        => Enumerable.Range(0, _input.Length)
            .SelectMany(y => Enumerable.Range(0, _input[0].Length)
            .Where(x => IsSpecificChar(y, x, 'A') && IsXMAS(y, x)))
            .Count();

    int CheckXMAS(int y, int x, int dy, int dx, char? prevChar)
    {
        if (y < 0 || y >= _input.Length || x < 0 || x >= _input[0].Length)
            return 0;

        if (prevChar is null)
        {
            return neighborOffsets
                .Sum(nb => CheckXMAS(y + nb.y, x + nb.x, nb.y, nb.x, 'X'));
        }

        if (prevChar == 'A' && IsSpecificChar(y, x, 'S'))
        {
            return 1;
        }

        if (prevChar == 'X' && IsSpecificChar(y, x, 'M'))
        {
            return CheckXMAS(y + dy, x + dx, dy, dx, 'M');
        }

        if (prevChar == 'M' && IsSpecificChar(y, x, 'A'))
        {
            return CheckXMAS(y + dy, x + dx, dy, dx, 'A');
        }

        return 0;
    }

    private bool IsXMAS(int y, int x)
        => ((IsSpecificChar(y - 1, x - 1, 'M') && IsSpecificChar(y + 1, x + 1, 'S'))
            || (IsSpecificChar(y - 1, x - 1, 'S') && IsSpecificChar(y + 1, x + 1, 'M')))
            && ((IsSpecificChar(y + 1, x - 1, 'M') && IsSpecificChar(y - 1, x + 1, 'S'))
            || (IsSpecificChar(y + 1, x - 1, 'S') && IsSpecificChar(y - 1, x + 1, 'M')));

    private bool IsSpecificChar(int y, int x, char ch)
    {
        if (y < 0 || y >= _input.Length || x < 0 || x >= _input[0].Length)
            return false;

        return _input[y][x] == ch;
    }
}
