namespace AoC2024.Days;

public class Day13 : BaseDay
{
    private readonly IEnumerable<(int ax, int ay, int bx, int by, int px, int py)> _input;

    public Day13()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Chunk(3)
            .Select(chunk =>
            {
                var aParts = chunk[0].Split([ "X+", ", Y+" ], StringSplitOptions.RemoveEmptyEntries);
                var bParts = chunk[1].Split([ "X+", ", Y+" ], StringSplitOptions.RemoveEmptyEntries);
                var pParts = chunk[2].Split([ "X=", ", Y=" ], StringSplitOptions.RemoveEmptyEntries);

                return (
                    ax: int.Parse(aParts[1]),
                    ay: int.Parse(aParts[2]),
                    bx: int.Parse(bParts[1]),
                    by: int.Parse(bParts[2]),
                    px: int.Parse(pParts[1]),
                    py: int.Parse(pParts[2])
                );
            }).ToList();
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1() => _input
        .Select(m => CalculateResult(m.ax, m.ay, m.bx, m.by, m.px, m.py, true)).Sum();

    private long Solve2() => _input
        .Select(m => CalculateResult(m.ax, m.ay, m.bx, m.by, m.px + 1e13, m.py + 1e13)).Sum();

    public static long CalculateResult(
        double ax, double ay,
        double bx, double by,
        double px, double py,
        bool stopAt100 = false)
    {
        long det = (long)(ax * by - ay * bx);
        long numA = (long)(px * by - py * bx);
        long numB = (long)(py * ax - px * ay);

        if (numA % det != 0 || numB % det != 0) return 0; 

        long a = numA / det;
        long b = numB / det;

        if (stopAt100 && (a > 100 || b > 100)) return 0;

        return a * 3 + b;
    }
}