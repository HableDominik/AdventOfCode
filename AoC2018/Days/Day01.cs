namespace AoC2018.Days;

public class Day01 : BaseDay
{
    private readonly IEnumerable<int> _input;

    public Day01()
    {
        _input = File.ReadAllLines(InputFilePath).Select(int.Parse);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => _input.Sum();

    private int Solve2()
    {
        int frequency = 0;
        var frequencies = new HashSet<int>() { frequency };

        while (true)
        {
            foreach (var fq in _input)
            {
                frequency += fq;
                if (!frequencies.Add(frequency)) return frequency;
            }
        }
    }
}
