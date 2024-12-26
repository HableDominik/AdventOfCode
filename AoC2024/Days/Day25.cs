namespace AoC2024.Days;

public class Day25 : BaseDay
{
    private readonly List<string[]> _input;
    private readonly int _width;
    private readonly int _depth;

    public Day25()
    {
        var input = File.ReadAllLines(InputFilePath);

        _depth = Array.FindIndex(input, string.IsNullOrWhiteSpace);
        _width = input[0].Length;

        _input = File.ReadAllLines(InputFilePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Chunk(_depth)
            .ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new(string.Empty);

    private int Solve1()
    {
        var (locks, keys) = ParseLocksAndKeys();

        return locks.Sum(@lock => keys.Count(key => CheckFit(@lock, key)));
    }

    private bool CheckFit(int[] locks, int[] key)
        => !locks.Where((lockValue, keyIndex) => key[keyIndex] + lockValue > (_depth - 2)).Any();

    private int[] ParseLockOrKey(string[] input, bool isKey = true)
    {
        if (!isKey) input = input.Reverse().ToArray();

        return Enumerable.Range(0, _width)
            .Select(x => Enumerable.Range(1, _depth - 1)
                .FirstOrDefault(y => input[y][x] == '.', _depth - 1) - 1)
            .ToArray();
    }

    private (List<int[]>, List<int[]>) ParseLocksAndKeys()
    {
        var locks = new List<int[]>();
        var keys = new List<int[]>();

        foreach (var input in _input)
        {
            if (input[0][0] == '#') 
                locks.Add(ParseLockOrKey(input));
            else 
                keys.Add(ParseLockOrKey(input, isKey: false));
        }

        return (locks, keys);
    }
}
