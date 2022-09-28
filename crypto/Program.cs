// See https://aka.ms/new-console-template for more information

using crypto;

// CryptoLib.Diffie();

var y = 47;
var a =88;
var p = 107;

//Console.WriteLine("\n{0}^x % {1} = {2}", a, p, y);
var res = CryptoLib.Shanks(a, p, y);
foreach (var i in res)
{
    Console.WriteLine("\nx = {2} {0} == {1}", y, CryptoLib.Mod(a, i, p), i);
}
