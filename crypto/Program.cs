// See https://aka.ms/new-console-template for more information

using System.Numerics;
using crypto;

//CryptoLib.DiffieHellman();

// var n = 1000000000;
// var p = CryptoLib.GenerateSimpleNumber(n);
// var y = CryptoLib.GenerateSimpleNumber(Convert.ToInt32(p - 1));
// var a = CryptoLib.GenerateSimpleNumber(Convert.ToInt32(p - 1));
//
// var res = CryptoLib.Shanks(a, p, y);
// foreach (var i in res)
// {
//     Console.WriteLine("x = {2} {0} == {1}", y, CryptoLib.Mod(a, i, p), i);
// }

//CryptoLib.Shamir(83862086);
    //CryptoLib.ElGamal(10);

Console.WriteLine(Encryption.RSA(6));