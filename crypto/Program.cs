// See https://aka.ms/new-console-template for more information

using crypto;


//Console.WriteLine(Encryption.Shamir(83862086));
Console.WriteLine(Encryption.ElGamal(10));
//Console.WriteLine(Encryption.RSA(6));


// var encryptor = new Encryption.Xor();
// encryptor.Encrypt("t.jpg");
// encryptor.Decrypt("XorEnc_t.jpg");


var m = 124124;
var encryptorRsaA = new Encryption.Rsa();
var encryptorRsaB = new Encryption.Rsa();

var e = encryptorRsaA.Encrypt(m, encryptorRsaB.D, encryptorRsaB.N);
var deEnc = encryptorRsaB.Decrypt(e,encryptorRsaB.N);

Console.WriteLine(e);
Console.WriteLine(deEnc);