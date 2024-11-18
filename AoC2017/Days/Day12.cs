using System.Xml.Linq;
using static AoC2017.Days.Day07;

namespace AoC2017.Days;

public class Day12 : BaseDay
{
    private readonly IEnumerable<string> _input;

    public Day12()
    {
        _input = File.ReadAllLines(InputFilePath);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private int Solve1()
    {
        var vertices = _input.Select(line => new Vertex(line)).ToList();
        vertices.ForEach(vertex => vertex.AddConnectedVertex(vertices));

        return vertices[0].CountConnectedRec();
    }

    private int Solve2()
    {
        var vertices = _input.Select(line => new Vertex(line)).ToList();
        vertices.ForEach(vertex => vertex.AddConnectedVertex(vertices));

        return vertices.Count(vertex => vertex.CountConnectedRec() != 0);
    }

    private class Vertex
    {
        public int Id { get; private set; }

        public List<Vertex>? ConnectedVertices { get; private set; }

        public List<int> ConnectedIds { get; private set; }

        public bool IsCounted { get; private set; }

        public Vertex(string line)
        {
            var split = line.Split(" <-> ");
            Id = int.Parse(split[0]);
            ConnectedIds = split[1].Split(", ").Select(int.Parse).ToList();
        }

        public void AddConnectedVertex(IEnumerable<Vertex> vertices)
            => ConnectedVertices = ConnectedIds
                .Select(id => vertices
                .First(vertex => vertex.Id == id))
                .ToList();

        public int CountConnectedRec()
        {
            if (IsCounted) return 0;

            IsCounted = true;

            return 1 + ConnectedVertices!
                .Where(vertex => !vertex.IsCounted)
                .Sum(vertex => vertex.CountConnectedRec());
        }
    }
}
