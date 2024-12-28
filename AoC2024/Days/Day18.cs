using Spectre.Console;

namespace AoC2024.Days;

public class Day18 : BaseDay
{
    private readonly int _size;
    private readonly HashSet<(int x, int y)> _input;
    private readonly Dictionary<(int x, int y), int> _costMap = [];
    private readonly (int dx, int dy)[] _directions;
    private readonly Comparer<(int x, int y, int cost)> _comparer;

    public Day18()
    {
        _size = 70;
        _input = File.ReadAllLines(InputFilePath)
            .Select(line => line.Split(','))
            .Select(pair => (int.Parse(pair[0]), int.Parse(pair[1])))
            .ToHashSet();

        _directions = [(0, -1), (1, 0), (0, 1), (-1, 0)];

        _comparer = Comparer<(int x, int y, int cost)>.Create((a, b) =>
            a.cost != b.cost ? a.cost.CompareTo(b.cost) :
            a.x != b.x ? a.x.CompareTo(b.x) :
            a.y.CompareTo(b.y));
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1() => FindCheapestPath(_input.Take(1024).ToHashSet());

    private string Solve2()
    {
        int lowerBound = 1024;
        int upperBound = _input.Count;
        int resultIndex = lowerBound;

        while (lowerBound < upperBound)
        {
            resultIndex = (lowerBound + upperBound) / 2;
            if (FindCheapestPath(_input.Take(resultIndex).ToHashSet()) > 0)
                lowerBound = resultIndex + 1;
            else
                upperBound = resultIndex;
        }

        var (x, y) = _input.ElementAt(resultIndex);
        return $"{x},{y}";
    }

    private int FindCheapestPath(HashSet<(int x, int y)> walls)
    {
        _costMap.Clear();

        var prioQueue = new SortedSet<(int x, int y, int cost)>(_comparer) { (0, 0, 0) };

        _costMap[(0, 0)] = 0;

        while (prioQueue.Count > 0)
        {
            var (x, y, currentCost) = prioQueue.Min;
            prioQueue.Remove(prioQueue.Min);

            if ((x, y) == (_size, _size)) return currentCost;

            if (_costMap.TryGetValue((x, y), out int recordedCost) && currentCost > recordedCost) continue;

            foreach (var (dx, dy) in _directions)
            {
                int newX = x + dx;
                int newY = y + dy;

                if (newX < 0 || newX > _size || newY < 0 || newY > _size) continue;
                if (walls.Contains((newX, newY))) continue;

                int newCost = currentCost + 1;

                if (_costMap.TryGetValue((newX, newY), out int existingCost) && newCost >= existingCost) continue;

                _costMap[(newX, newY)] = newCost;
                prioQueue.Add((newX, newY, newCost));
            }
        }

        return -1;
    }
}