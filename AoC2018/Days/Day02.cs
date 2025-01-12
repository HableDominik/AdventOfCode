namespace AoC2018.Days;

public class Day02 : BaseDay
{
    private readonly string[] _input;

    public Day02()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => _input.Count(s => HasLetterCount(s, 2))
            * _input.Count(s => HasLetterCount(s, 3));

    private string Solve2()
    {
        string boxId1 = string.Empty, boxId2 = string.Empty;
        int minHammingDistance = int.MaxValue;

        for (int i = 0; i < _input.Length; i++)
        {
            for (int j = i + 1; j < _input.Length; j++)
            {
                var hammingDistance = GetHammingDistance(_input[i], _input[j]);
                if (hammingDistance > minHammingDistance) continue;
                
                minHammingDistance = hammingDistance;
                boxId1 = _input[i];
                boxId2 = _input[j];  
            }
        }

        return GetCommonLetters(boxId1, boxId2);
    }

    private static bool HasLetterCount(string str, int count) 
        => str.GroupBy(c => c).Any(g => g.Count() == count);

    private static int GetHammingDistance(string s1, string s2)
        => s1.Zip(s2, (c1, c2) => c1 != c2 ? 1 : 0).Sum();

    private static string GetCommonLetters(string s1, string s2)
        => new (s1.Zip(s2, (c1, c2) => c1 == c2 ? c1 : '\0')
            .Where(c => c is not '\0')
            .ToArray());
}