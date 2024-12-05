namespace AoC2024.Days;

public class Day05 : BaseDay
{
    private readonly (int left, int right)[] _rules;
    private readonly int[][] _updates;
    private readonly Comparison<int> _comparer;

    public Day05()
    {
        var input = File.ReadAllLines(InputFilePath);

        var separatorIndex = Array.FindIndex(input, string.IsNullOrWhiteSpace);
        var ruleLines = input.Take(separatorIndex).ToArray();
        var updateLines = input.Skip(separatorIndex + 1).ToArray();

        _rules = ruleLines
            .Select(line => line.Split('|'))
            .Select(parts => (left: int.Parse(parts[0]), right: int.Parse(parts[1])))
            .ToArray();

        _updates = updateLines
            .Select(line => line
                .Split(',')
                .Select(int.Parse)
                .ToArray())
            .ToArray();

        var rulesDictionary = _rules
            .GroupBy(rule => rule.left)
            .ToDictionary(
                group => group.Key,
                group => group.Select(rule => rule.right).ToHashSet());

        _comparer = (x, y) =>
        {
            if (rulesDictionary.ContainsKey(x) && rulesDictionary[x].Contains(y)) return -1;
            if (rulesDictionary.ContainsKey(y) && rulesDictionary[y].Contains(x)) return 1;
            return 0;
        };
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _updates
        .Select(pages => GetMiddlePage(pages, correctIfInvalid: false))
        .Sum();

    private int Solve2() => _updates
        .Select(pages => GetMiddlePage(pages, correctIfInvalid: true))
        .Sum();

    private int GetMiddlePage(int[] pages, bool correctIfInvalid)
    {
        var indexMap = pages
            .Select((value, index) => (value, index))
            .ToDictionary(item => item.value, item => item.index);

        var isValid = _rules.All(rule =>
            !indexMap.TryGetValue(rule.left, out var leftIndex) ||
            !indexMap.TryGetValue(rule.right, out var rightIndex) ||
            leftIndex < rightIndex);

        if (isValid)
        {
            return correctIfInvalid ? 0 : pages[pages.Length / 2];
        }

        if (!correctIfInvalid) return 0;

        var correctedPages = CorrectUpdate(pages);
        return correctedPages[correctedPages.Length / 2];
    }

    private int[] CorrectUpdate(int[] pages)
    {
        Array.Sort(pages, _comparer);
        return pages;
    }
}
