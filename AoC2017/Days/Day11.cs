namespace AoC2017.Days;

public class Day11 : BaseDay
{
    private readonly string _input;

    public Day11()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var dirs = _input.Split(',');
        int x = 0, y = 0, z = 0;

        foreach( var dir in dirs )
        {
            var vals = GetDirValues( dir );
            x += vals.dx;
            y += vals.dy;
            z += vals.dz;
        };

        return (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2;
    }

    private double Solve2()
    {
        var dirs = _input.Split(',');
        int x = 0, y = 0, z = 0;
        var max = 0;

        foreach (var dir in dirs)
        {
            var vals = GetDirValues(dir);
            x += vals.dx;
            y += vals.dy;
            z += vals.dz;
            var dist = (Math.Abs(x) + Math.Abs(y) + Math.Abs(z)) / 2;
            max = Math.Max(max, dist);
        };

        return max;
    }

    private (int dx, int dy, int dz) GetDirValues(string dir)
        => dir switch
        {
            "n" =>  (0, 1, -1),
            "ne" => (1, 0, -1),
            "se" => (1, -1, 0),
            "s" =>  (0, -1, 1),
            "sw" => (-1, 0, 1),
            "nw" => (-1, 1, 0),
            _ => throw new ArgumentException($"Invalid direction: {dir}")
        };
}
