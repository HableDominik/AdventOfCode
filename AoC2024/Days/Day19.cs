


using System.Collections.ObjectModel;
using System.Linq;

namespace AoC2024.Days;

public class Day19 : BaseDay
{
    private readonly HashSet<string> _towels;
    private readonly string[] _patterns;
    private readonly int _min, _max;
    private readonly HashSet<string> _badPatterns = [];
    private readonly Dictionary<string, long> _goodPatterns = [];

    public Day19()
    {
        var input = File.ReadAllLines(InputFilePath);
        _towels = [.. input[0].Split(", ")];
        _patterns = input.Skip(2).ToArray();
        _min = _towels.Min(t => t.Length);
        _max = _towels.Max(t => t.Length);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _patterns.Count(pattern => CheckPattern(pattern) is > 0);

    private long Solve2() => _patterns.Sum(pattern => CheckPattern(pattern));

    private long CheckPattern(string pattern)
    {
        if (_goodPatterns.TryGetValue(pattern, out long value)) return value;

        if (_badPatterns.Contains(pattern)) return 0;

        if (pattern.Length == 0) return 1;

        long patternCount = 0;

        for (int i = _min; i <= _max && i <= pattern.Length; i++)
        {
            if (!_towels.Contains(pattern[..i])) continue;

            patternCount += CheckPattern(pattern[i..]);
        }

        if (patternCount == 0)
        {
            _badPatterns.Add(pattern);
        }

        _goodPatterns[pattern] = patternCount;

        return patternCount;
    }
}
