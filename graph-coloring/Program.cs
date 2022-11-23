using System.Numerics;
using System.Text;
using graph_coloring;

const string path = "file.txt";
const int checksCount = 5;


var lines = File.ReadAllLines(path, Encoding.UTF8);

var subs = lines[0].Split(' ');
var n = Convert.ToInt32(subs[0]);
var count = Convert.ToInt32(subs[0]);

var edges = new List<Tuple<int, int>> { };

for (var i = 1; i < count + 1; i++)
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

Console.WriteLine(new string(CreateTransposition(colors)));

Console.WriteLine(Convert.ToInt32('r'));
Console.WriteLine(Convert.ToInt32('g'));
Console.WriteLine(Convert.ToInt32('b'));

var nodes = new Dictionary<int, Node>();

for (var i = 1; i < n + 1; i++)
{
    nodes.Add(i,new Node(i));
}

for (var i = 1; i < checksCount; i++)
{
    var rnd = new Random().Next(0, n);
    var edge = edges[rnd];
    
    nodes.TryGetValue(edge.Item1, out var item1);
    nodes.TryGetValue(edge.Item2, out var item2);
    
    var newTransposition = new string(CreateTransposition(colors));
    
    var z1 = item1!.CreateZ(newTransposition);
    var z2 = item2!.CreateZ(newTransposition);

    var z11 = BigInteger.ModPow(z1, item1.C, item1.N);
    var z22 = BigInteger.ModPow(z2, item2.C, item2.N).ToByteArray();
    
    Console.WriteLine("{0} {1}", BitConverter.ToInt32(z11.ToByteArray(), 61), 255);

}




