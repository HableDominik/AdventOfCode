namespace AoC2021.Days;

public class Day01 : BaseDay
{
    private readonly List<int> _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath).Select(int.Parse).ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input.Zip(_input.Skip(1), (a, b) => a < b).Count(b => b);

    private int Solve2()
    {
        var triples = _input
            .Zip(_input.Skip(1), (a, b) => a + b)
            .Zip(_input.Skip(2), (ab, c) => ab + c);

        return triples.Zip(triples.Skip(1), (a, b) => a < b).Count(b => b);
    }
}