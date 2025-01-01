namespace AoC2024.Days;

public class Day24 : BaseDay
{
    private readonly Dictionary<string, bool> _values;
    private readonly Dictionary<string, (string left, Func<bool, bool, bool> op, string right)> _logics;

    public Day24()
    {
        var input = File.ReadAllLines(InputFilePath);
        var separatorIndex = Array.FindIndex(input, string.IsNullOrWhiteSpace);
        var valueLines = input.Take(separatorIndex).ToArray();
        var logicLines = input.Skip(separatorIndex + 1).ToArray();

        _values = ParseValues(valueLines);
        _logics = ParseLogics(logicLines);
    }

    public override ValueTask<string> Solve_1() => new($"{Solve1()}");

    public override ValueTask<string> Solve_2() => new($"{Solve2()}");

    private long Solve1()
        => _logics.Keys
            .Where(key => key.StartsWith('z'))
            .OrderDescending()
            .Select(GetValueFor)
            .Aggregate(0L, (acc, value) => (acc << 1) | (value? 1L : 0L));

    private static string Solve2()
    {
    /*
        Had no plan how to start so i just printed all the used operations which looked like this:
        
        z02
            tdp ^ ccn -> z02
            bcr | jcr -> tdp
            tss & rvp -> bcr
            x01 & y01 -> jcr
            y02 ^ x02 -> ccn
        z03
            bsj ^ hgq -> z03
            y03 ^ x03 -> bsj
            tkr | hhm -> hgq
            ccn & tdp -> tkr
            y02 & x02 -> hhm

        I found a pattern in this sequences.
        Most of them had the pattern   ^|&&^   or   ^^|&&   (like z02 and z03 above).

        But not all of them. If you ignore the first 2 and the last one, there were 8(!) without this pattern.
        Also all of them were consecutive.
        In my case: z09 - z10, z13 - z14, z19 - z20, z33 - z34

        So i tried to fix those sequences to match the others and it worked.

        z09
           pcd ^ gws -> z09     pcd ^ gws -> z09   
           mdr | dvh -> pcd     mdr | dvh -> pcd
           kvv & kbn -> mdr     kvv & kbn -> mdr
           y08 & x08 -> dvh     y08 & x08 -> dvh
           x09 & y09 -> gws <-  y09 ^ x09 -> gws
        z10
           hcb ^ bkq -> z10     hcb ^ bkq -> z10
           nnt | tqw -> hcb     nnt | tqw -> hcb
           y09 ^ x09 -> nnt <-  x09 & y09 -> nnt
           gws & pcd -> tqw     gws & pcd -> tqw
           x10 ^ y10 -> bkq     x10 ^ y10 -> bkq

        z13
           fmh | tqs -> z13 <-  hgw ^ kvr -> z13
           x13 & y13 -> fmh     svf | bgf -> hgw
           hgw & kvr -> tqs     y12 & x12 -> svf
           svf | bgf -> hgw     crc & vhk -> bgf
           y12 & x12 -> svf     x13 ^ y13 -> kvr
           crc & vhk -> bgf
           x13 ^ y13 -> kvr
        z14
           npf ^ tbd -> z14     npf ^ tbd -> z14
           hgw ^ kvr -> npf <-  fmh | tqs -> npf
           y14 ^ x14 -> tbd     x13 & y13 -> fmh
                                hgw & kvr -> tqs
                                y14 ^ x14 -> tbd

        z19
           y19 & x19 -> z19 <-  rsm ^ fnq -> z19
                                fbp | mdc -> rsm
                                y18 & x18 -> fbp
                                sqn & tkw -> mdc
                                x19 ^ y19 -> fnq
        z20
           crr ^ jgw -> z20     crr ^ jgw -> z20
           dgm | cph -> crr     dgm | cph -> crr
           rsm & fnq -> dgm     rsm & fnq -> dgm
           fbp | mdc -> rsm     y19 & x19 -> cph
           y18 & x18 -> fbp     x20 ^ y20 -> jgw
           sqn & tkw -> mdc
           x19 ^ y19 -> fnq
           rsm ^ fnq -> cph <-  
           x20 ^ y20 -> jgw

        z33
           wgq & wtm -> z33 <-  wtm ^ wgq -> z33
           mhn | mmg -> wgq     x33 ^ y33 -> wtm
           y32 & x32 -> mhn     mhn | mmg -> wgq
           mtk & gmj -> mmg     y32 & x32 -> mhn
           x33 ^ y33 -> wtm     mtk & gmj -> mmg
        z34
           cnd ^ wvn -> z34     cnd ^ wvn -> z34
           x34 ^ y34 -> cnd     x34 ^ y34 -> cnd
           fvk | hgj -> wvn     fvk | hgj -> wvn
           x33 & y33 -> fvk     x33 & y33 -> fvk
           wtm ^ wgq -> hgj <-  wgq & wtm -> hgj

        Yes. Kinda hacky. But a star is a start ;)
    */

        string[] swappedWires = ["gws", "nnt", "z13", "npf", "z19", "cph", "z33", "hgj"];
        
        return string.Join(',', swappedWires.Order());
    }

    private bool GetValueFor(string name)
    {
        if (_values.TryGetValue(name, out var recordedValue)) return recordedValue;

        if (_logics.TryGetValue(name, out var gate))
        {
            var value = gate.op(GetValueFor(gate.left), GetValueFor(gate.right));
            _values[name] = value;
            return value;
        }

        throw new ArgumentException($"Unknown gate {name}.");
    }

    private static Dictionary<string, bool> ParseValues(string[] valueLines)
        => valueLines
            .Select(line => line.Split(": "))
            .ToDictionary(split => split[0], split => split[1] == "1");

    private Dictionary<string, (string left, Func<bool, bool, bool> op, string right)> ParseLogics(string[] logicLines)
        => logicLines
            .Select(ParseLogicLine)
            .ToDictionary(x => x.output, x => (x.left, x.op, x.right));

    private (string output, string left, Func<bool, bool, bool> op, string right) ParseLogicLine(string line)
        => line.Split(' ') switch
            {
                [var left, "AND", var right, _, var output] => (output, left, (a, b) => a & b, right),
                [var left, "OR", var right, _, var output] => (output, left, (a, b) => a || b, right),
                [var left, "XOR", var right, _, var output] => (output, left, (a, b) => a ^ b, right),
                    _ => throw new ArgumentException($"Unsupported operator in {line}.")
            };
}