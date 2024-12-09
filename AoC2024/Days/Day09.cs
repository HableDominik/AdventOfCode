using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AoC2024.Days;

public class Day09 : BaseDay
{
    private readonly IEnumerable<int> _input;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath).Select(ch => ch - '0'); ;
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1()
    {
        var isFile = true;
        int id = 0;

        var disk = _input.SelectMany(number =>
            {
                var result = Enumerable.Repeat(isFile ? id : -1, number);
                if (isFile) id++;
                isFile = !isFile;
                return result;
            }).ToList();

        var fileCount = disk.Count(d => d != -1);

        for (int i = 0; i < fileCount; i++)
        {
            if (disk[i] == -1)
            {
                var last = disk[^1];
                disk.RemoveAt(disk.Count - 1);

                if (last == -1)
                {
                    i--;
                    continue;
                }

                disk[i] = last;
            }
        }

        return disk.Select((value, index) => index * (long)value).Sum();
    }

    private long Solve2()
    {
        var disk = new LinkedList<(int Id, int Size)>();

        var isFile = true;
        int id = 0;

        foreach (var number in _input)
        {
            disk.AddLast((isFile ? id : -1, number));
            if (isFile) id++;
            isFile = !isFile;
        }

        var current = disk.Last;

        while (current != null)
        {
            var currentValue = current.Value;
            
            if(currentValue.Id == -1)
            {
                current = current.Previous;
                continue;
            }

            var iterator = disk.First;

            for(; iterator != current && iterator != null; iterator = iterator.Next)
            {
                var (Id, Size) = iterator.Value;
                if (Id == -1 && Size >= currentValue.Size) break;
            }

            if (iterator == current || iterator == null)
            {
                current = current.Previous;
                continue;
            }

            current.Value = (-1, current.Value.Size);
            disk.AddBefore(iterator, currentValue);
            iterator.Value = (-1, iterator.Value.Size - currentValue.Size);
        }

        return disk
            .SelectMany(node => Enumerable.Repeat(node.Id == -1 ? 0 : node.Id, node.Size))
            .Select((value, index) => index * (long)value).Sum();
    }
}
