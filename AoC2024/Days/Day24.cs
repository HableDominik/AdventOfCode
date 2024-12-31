namespace AoC2024.Days;

public class Day24 : BaseDay
{
    private readonly Dictionary<string, bool> _values;
    private readonly Dictionary<string, (string left, Func<bool, bool, bool> op, string right)> _logics;

    public Day24()
    {
        var input = File.ReadAllLines(InputFilePath);
        var separatorIndex = Array.FindIndex(input, string.IsNullOrWhiteSpace);
        var valueLines = input.Take(separatorIndex).ToArray();
        var logicLines = input.Skip(separatorIndex + 1).ToArray();

        _values = ParseValues(valueLines);
        _logics = ParseLogics(logicLines);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1()
        => _logics.Keys
            .Where(key => key.StartsWith('z'))
            .OrderDescending()
            .Select(GetValueFor)
            .Aggregate(0L, (acc, value) => (acc << 1) | (value ? 1L : 0L));

    private string Solve2() => string.Empty;

    private bool GetValueFor(string name)
    {
        if (_values.TryGetValue(name, out var recordedValue)) return recordedValue;

        if (_logics.TryGetValue(name, out var gate))
        {
            var value = gate.op(GetValueFor(gate.left), GetValueFor(gate.right));
            _values[name] = value;
            return value;
        }

        throw new ArgumentException($"Unknown gate {name}.");
    }

    private static Dictionary<string, bool> ParseValues(string[] valueLines)
        => valueLines
            .Select(line => line.Split(": "))
            .ToDictionary(split => split[0], split => split[1] == "1");

    private Dictionary<string, (string left, Func<bool, bool, bool> op, string right)> ParseLogics(string[] logicLines)
        => logicLines
            .Select(ParseLogicLine)
            .ToDictionary(x => x.output, x => (x.left, x.op, x.right));

    private (string output, string left, Func<bool, bool, bool> op, string right) ParseLogicLine(string line)
        => line.Split(' ') switch
            {
                [var left, "AND", var right, _, var output] => (output, left, (a, b) => a & b, right),
                [var left, "OR", var right, _, var output] => (output, left, (a, b) => a || b, right),
                [var left, "XOR", var right, _, var output] => (output, left, (a, b) => a ^ b, right),
                    _ => throw new ArgumentException($"Unsupported operator in {line}.")
            };
}