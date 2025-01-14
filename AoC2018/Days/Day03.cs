namespace AoC2018.Days;

public class Day03 : BaseDay
{
    private readonly List<(int index, int x, int y, int w, int h)> _input;
    private readonly Dictionary<(int, int), List<int>> _claimedInches = [];

    public Day03()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.Split(['#', ' ', '@', ',', ':', 'x'], StringSplitOptions.RemoveEmptyEntries))
            .Select(x => x.Select(int.Parse).ToArray())
            .Select(x => (x[0], x[1], x[2], x[3], x[4]))
            .ToList();

        _input.ForEach(CheckClaims);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _claimedInches.Values.Count(list =>  list.Count > 1);
    
    private int Solve2()
        => _claimedInches
            .Where(kv => kv.Value.Count == 1)
            .GroupBy(kv => kv.Value.First())
            .Select(g => (index: g.Key, amount: g.Count()))
            .Intersect(_input.Select(x => (x.index, x.w * x.h)))
            .Single()
            .index;

    private void CheckClaims((int index, int x, int y, int w, int h) claim)
    {
        for (int y = claim.y; y < (claim.y + claim.h); y++)
        {
            for (int x = claim.x; x < (claim.x + claim.w); x++)
            {
                if (!_claimedInches.ContainsKey((x, y)))
                {
                    _claimedInches[(x, y)] = [claim.index];
                    continue;
                }
                
                _claimedInches[(x, y)].Add(claim.index);
            }
        }
    }
}
