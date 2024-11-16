using System.Runtime.ExceptionServices;

namespace AoC2017.Days;

public class Day07 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day07()
    {
        _input = File.ReadLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private string Solve1()
    {
        var parents = new List<string>();
        var children = new List<string>();

        foreach(var line in _input)
        {
            var split = line.Split(" -> ");
            parents.Add(split[0].Split(' ')[0]);

            if (split.Count() == 1) continue;

            var splitChilren = split[1].Split(", ");
            children.AddRange(splitChilren);
        }

        return parents.Except(children).Single();
    }

    private int Solve2()
    {
        var nodes = _input.Select(line => new Node(line)).ToList();
        nodes.ForEach(node => node.AddChildNodes(nodes));

        var node = nodes.First();
        while (node.Parent != null) node = node.Parent;

        var targetWeight = 0;
        while(!node.IsBalanced())
        {
            (node, targetWeight) = node.GetUnbalancedChild();
        }

        return node.Weight + targetWeight - node.GetTotalWeight();
    }

    public class Node
    {
        public int Weight { get; private set; }
        public string Name { get; private set; }
        public List<string> ChildNames { get; private set; }
        public List<Node>? ChildNodes { get; private set; }
        public Node? Parent { get; private set; }

        public Node(string line)
        {
            var split = line.Split(" -> ");
            Name = split[0].Split(' ')[0];
            Weight = int.Parse(split[0].Split(' ')[1].Trim('(', ')'));
            if (split.Length == 1)
            {
                ChildNames = [];
                return;
            }
            ChildNames = [.. split[1].Split(", ")];
        }

        public void AddChildNodes(IEnumerable<Node> nodes)
        {
            ChildNodes = ChildNames
                .Select(name => nodes
                .First(node => node.Name == name))
                .ToList();
            ChildNodes.ForEach(x => x.Parent = this);
        }

        public int GetTotalWeight() => ChildNodes!.Sum(x => x.GetTotalWeight()) + Weight;

        public bool IsBalanced() => ChildNodes!.GroupBy(x => x.GetTotalWeight()).Count() == 1;

        public (Node node, int targetWeight) GetUnbalancedChild()
        {
            var groups = ChildNodes!.GroupBy(n => n.GetTotalWeight()).OrderBy(n => n.Count());
            var targetWeight = groups.Last().Key;
            var unbalancedChild = groups.First().First();
            return (unbalancedChild, targetWeight);
        }
    }
}
