namespace AoC2024.Days;

public class Day23 : BaseDay
{
    private readonly Dictionary<string, HashSet<string>> _graph = [];

    public Day23()
    {
        var input = File.ReadAllLines(InputFilePath);

        foreach (var line in input)
        {
            var split = line.Split('-');
            AddEdge(_graph, split[0], split[1]);
        }
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
        => FindTriangles(_graph)
            .Where(static triangle =>
                triangle.Item1.StartsWith('t') ||
                triangle.Item2.StartsWith('t') ||
                triangle.Item3.StartsWith('t'))
            .Count();

    private string Solve2()
        => string.Join(',', BronKerbosch([], new HashSet<string>(_graph.Keys), []).Order());

    private List<string> BronKerbosch(
        HashSet<string> R, // Vertices already in the current clique.
        HashSet<string> P, // Vertices that can still join the current clique.
        HashSet<string> X) // Vertices that can't join the current clique.
    {
        if (P.Count is 0 && X.Count is 0) return [.. R];

        var largestClique = new List<string>();

        foreach (var v in P.ToList())
        {
            var neighbors = _graph[v];

            var newClique = BronKerbosch(
                new HashSet<string>(R) { v },
                new HashSet<string>(P.Intersect(neighbors)),
                new HashSet<string>(X.Intersect(neighbors))
            );

            if (newClique.Count > largestClique.Count)
            {
                largestClique = newClique;
            }

            P.Remove(v);
            X.Add(v);
        }

        return largestClique;
    }

    private static HashSet<(string, string, string)> FindTriangles(Dictionary<string, HashSet<string>> graph)
        => graph.Keys
            .SelectMany(a => graph[a]
                .Where(b => string.Compare(b, a) > 0)
                .SelectMany(b => graph[b]
                    .Where(c => string.Compare(c, b) > 0 
                        && c != a 
                        && graph[a].Contains(c))
                    .Select(c => new[] { a, b, c }
                        .OrderBy(node => node)
                        .ToArray())
                )
            )
            .Select(triangle => (triangle[0], triangle[1], triangle[2]))
            .ToHashSet();

    private static void AddEdge(Dictionary<string, HashSet<string>> graph, string left, string right)
    {
        if (!graph.ContainsKey(left)) graph[left] = [];
        if (!graph.ContainsKey(right)) graph[right] = [];

        graph[left].Add(right);
        graph[right].Add(left);
    }
}