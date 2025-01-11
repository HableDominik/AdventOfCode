namespace AoC2016.Days;

public class Day02 : BaseDay
{
    private readonly string[] _input;

    private readonly char?[] _layout =
    [
        null, null, null, null, null, null, null,
        null, null, null,  '1', null, null, null,
        null, null,  '2',  '3',  '4', null, null,
        null,  '5',  '6',  '7',  '8',  '9', null,
        null, null,  'A',  'B',  'C', null, null,
        null, null, null,  'D', null, null, null,
        null, null, null, null, null, null, null 
    ];

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private string Solve1()
    {
        int pos = 5;
        string result = string.Empty;

        foreach (var line in _input)
        {
            foreach(var button in line)
            {
                pos += Move1(button, pos);
            }
            result += pos;
        }

        return result;
    }

    private string Solve2()
    {
        int pos = 22;
        string result = string.Empty;

        foreach (var line in _input)
        {
            foreach(var button in line)
            {
                pos += Move2(button, pos);
            }
            result += _layout[pos];
        }

        return result;
    }

    private static int Move1(char button, int pos)
        => button switch
            {
                'U' => pos > 3 ? -3 : 0,
                'R' => (pos % 3) != 0 ? 1 : 0,
                'D' => pos < 7 ? 3 : 0,
                'L' => (pos % 3) != 1 ? -1 : 0,
                _ => throw new ArgumentException($"Invalid button: {button}")
            };

    private int Move2(char button, int pos)
        => button switch
            {
                'U' => _layout[pos - 7] is not null ? -7 : 0,
                'R' => _layout[pos + 1] is not null ? 1 : 0,
                'D' => _layout[pos + 7] is not null ? 7 : 0,
                'L' => _layout[pos - 1] is not null ? -1 : 0,
                _ => throw new ArgumentException($"Invalid button: {button}")
            };
}