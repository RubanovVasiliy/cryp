using System.Numerics;
using System.Text;
using graph_coloring;

const string path = "file.txt";
const int checksCount = 7;


var lines = File.ReadAllLines(path, Encoding.UTF8);

var subs = lines[0].Split(' ');
var nodesCount = Convert.ToInt32(subs[0]);
var edgesCount = Convert.ToInt32(subs[1]);

var edges = new List<Tuple<int, int>>();
for (var i = 1; i < edgesCount + 1; i++)
{
    var keys = lines[i].Split(' ');
    edges.Add(new Tuple<int, int>(Convert.ToInt32(keys[0]), Convert.ToInt32(keys[1])));
}

var colors = lines[^1];

char[] Swap(string input)
{
    var rnd = new Random();
    var output = input.ToCharArray();
    for (var i = 0; i < 2; i++)
    {
        var rnd1 = rnd.Next(0, input.Length);
        var rnd2 = rnd.Next(0, input.Length);
        while (rnd1 == rnd2)
        {
            rnd2 = rnd.Next(0, input.Length);
        }

        (output[rnd1], output[rnd2]) = (output[rnd2], output[rnd1]);
    }

    return output;
}
char[] CreateTransposition(string input)
{
    var rgb = Swap("rgb");
    var output = input.ToCharArray();

    for (var i = 0; i < output.Length; i++)
    {
        output[i] = output[i] switch
        {
            'r' => rgb[0],
            'g' => rgb[1],
            'b' => rgb[2],
            _ => output[i]
        };
    }
    return output;
}

var nodes = new Dictionary<int, Node>();
for (var i = 1; i < nodesCount + 1; i++)
{
    nodes.Add(i,new Node(i));
}

for (var i = 0; i < checksCount; i++)
{
    var rnd = new Random().Next(0, edgesCount);
    var edge = edges[rnd];

    nodes.TryGetValue(edge.Item1, out var item1);
    nodes.TryGetValue(edge.Item2, out var item2);

    var newTransposition = new string(CreateTransposition(colors));

    var z1 = item1!.CreateZ(newTransposition);
    var z2 = item2!.CreateZ(newTransposition);

    var z11 = BigInteger.ModPow(z1, item1.C, item1.N).ToByteArray()[^1];
    var z22 = BigInteger.ModPow(z2, item2.C, item2.N).ToByteArray()[^1];

    Console.WriteLine(newTransposition);
    Console.WriteLine("{0} {1}", item1.Id, item2.Id);
    Console.WriteLine("{0} {1}", Convert.ToChar(z11), Convert.ToChar(z22));
    Console.WriteLine();
}




