namespace AoC2019.Days;

public class Day06 : BaseDay
{
    private readonly Dictionary<string, string[]> _input;

    public Day06()
    {
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.Split(')'))
            .GroupBy(parts => parts[0], parts => parts[1])
            .ToDictionary(group => group.Key, group => group.ToArray());
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => CountOrbits("COM", 0);

    private int Solve2() => CalculateOrbitalTransfers("COM", 0).result;

    private int CountOrbits(string name, int depth)
        => _input.TryGetValue(name, out var orbits)
            ? depth + orbits.Sum(orbit => CountOrbits(orbit, depth + 1))
            : depth;

    private (int result, int you, int san) CalculateOrbitalTransfers(string name, int depth)
    {
        if (name == "YOU") return (-1, depth - 1, -1);

        if (name == "SAN") return (-1, -1, depth - 1);

        if (_input.TryGetValue(name, out var orbits))
        {
            int you = -1, san = -1;

            foreach (var orbit in orbits)
            {
                var (result, orbitYou, orbitSan) = CalculateOrbitalTransfers(orbit, depth + 1);

                if (result != -1) return (result, orbitYou, orbitSan);

                you = orbitYou != -1 ? orbitYou : you;
                san = orbitSan != -1 ? orbitSan : san;
            }

            var res = (you != -1 && san != -1) 
                ? you + san - depth * 2
                : -1;

            return (res, you, san);
        }

        return (-1, -1, -1);
    }
}
