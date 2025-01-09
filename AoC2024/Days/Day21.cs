using System.Linq;
using System.Net.WebSockets;

namespace AoC2024.Days;

public class Day21 : BaseDay
{
    private readonly string[] _codes;
    private readonly Dictionary<(string code, int keypads), long> _cache = [];

    public Day21()
    {
        _codes = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1() => _codes.Sum(code => CalculateComplexity(code, 3));

    private long Solve2() => _codes.Sum(code => CalculateComplexity(code, 26));

    private long CalculateComplexity(string code, int keypads) 
        => GetMinLenRec(code, GetNumpadMovements, keypads) * ExtractNumbers(code);

    private long GetMinLenRec(string code, Func<string, List<string>> getMovements, int keypads)
    {
        if (_cache.TryGetValue((code, keypads), out var cachedResult)) return cachedResult;

        if (keypads == 0) return code.Length;

        var aCode = $"A{code}";
        var pairs = aCode.Zip(aCode.Skip(1), (current, next) => $"{current}{next}").ToList();

        long minLen = 0;

        foreach (var pair in pairs)
        {
            minLen += getMovements(pair).Min(m => GetMinLenRec(m, GetDirpadMovements, keypads - 1));
        }

        _cache[(code, keypads)] = minLen;

        return minLen;
    }

    private static int ExtractNumbers(string input)
    {
        string numbersOnly = new(input.Where(char.IsDigit).ToArray());
        return int.TryParse(numbersOnly, out int number) ? number : 0;
    }

    private static List<string> GetDirpadMovements(string movement)
        => movement[0] == movement[1]
            ? ["A"]
            : movement switch
        {
            "A^" => ["<A"],
            "A<" => ["v<<A"],
            "Av" => ["<vA", "v<A"],
            "A>" => ["vA"],
            "^A" => [">A"],
            "^<" => ["v<A"],
            "^v" => ["vA"],
            "^>" => ["v>A", ">vA"],
            "<^" => [">^A"],
            "<A" => [">>^A"],
            "<v" => [">A"],
            "<>" => [">>A"],
            "v^" => ["^A"],
            "v<" => ["<A"],
            "vA" => ["^>A", ">^A"],
            "v>" => [">A"],
            ">^" => ["<^A", "^<A"],
            "><" => ["<<A"],
            ">v" => ["<A"],
            ">A" => ["^A"],
            _ => throw new NotImplementedException($"Movement '{movement}' not implemented.")
        };

    private static List<string> GetNumpadMovements(string movement)
        => movement switch
        {
            "A0" => ["<A"],
            "02" => ["^A"],
            "29" => [">^^A", "^^>A"],
            "9A" => ["vvvA"],
            "A9" => ["^^^A"],
            "98" => ["<A"],
            "80" => ["vvvA"],
            "0A" => [">A"],
            "A1" => ["^<<A"],
            "17" => ["^^A"],
            "79" => [">>A"],
            "A4" => ["^^<<A"],
            "45" => [">A"],
            "56" => [">A"],
            "6A" => ["vvA"],
            "A3" => ["^A"],
            "37" => ["<<^^A", "^^<<A"],
            "31" => ["<<A"],
            "19" => [">>^^A", "^^>>A"],
            "08" => ["^^^A"],
            "85" => ["vA"],
            "5A" => ["vv>A", ">vvA"],
            "14" => ["^A"],
            "43" => [">>vA", "v>>A"],
            "3A" => ["vA"],
            "A2" => ["^<A", "<^A"],
            "28" => ["^^A"],
            "86" => [">vA", "v>A"],
            "A7" => ["^^^<<A"],
            "78" => [">A"],
            "89" => [">A"],
            _ => throw new NotImplementedException($"Movement '{movement}' not implemented.")
        };
}
