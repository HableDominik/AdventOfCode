namespace AoC2015.Days;

public class Day02 : BaseDay
{
    private readonly IEnumerable<(int l, int w, int h)> _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.Split('x'))
            .Select(split => 
                (int.Parse(split[0]), int.Parse(split[1]), int.Parse(split[2])));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input
        .Sum(box =>
            2 * box.l * box.w +
            2 * box.w * box.h +
            2 * box.h * box.l +
            GetSmallestSide(box.l, box.w, box.h));

    private int Solve2() => _input
        .Sum(box =>
            box.l * box.w * box.h +
            2 * SumOfTwoLowest(box.l, box.w, box.h));

    private static int GetSmallestSide(int l, int w, int h) 
        => Math.Min(Math.Min(l * w, w * h), h * l);

    private static int SumOfTwoLowest(int a, int b, int c)
        => a + b + c - Math.Max(a, Math.Max(b, c));
}
