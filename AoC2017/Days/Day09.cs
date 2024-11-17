using System.Reflection.Metadata.Ecma335;

namespace AoC2017.Days;

public class Day09 : BaseDay
{
    private readonly string _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var score = 0;
        var depth = 0;
        var isGarbage = false;
        var skip = false;

        foreach(var ch in _input)
        {
            if (skip) 
            { 
                skip = false; 
                continue; 
            }

            switch (ch)
            {
                case '{' : if (isGarbage) break;
                    depth++;
                    break;
                case '}': if (isGarbage) break;
                    score += depth;
                    depth--;
                    break;
                case '<': isGarbage = true; break;
                case '>': isGarbage = false; break;
                case '!': skip = true; break;
                default: break;
            };
        }

        return score;
    }

    private int Solve2()
    {
        var score = 0;
        var isGarbage = false;
        var skip = false;

        foreach (var ch in _input)
        {
            if (skip)
            {
                skip = false;
                continue;
            }

            switch (ch)
            {
                case '<':
                    if (isGarbage) score++;
                    isGarbage = true; break;
                case '>': isGarbage = false; break;
                case '!': skip = true; break;
                default: if (isGarbage) score++; break;
            };
        }

        return score;
    }
}
