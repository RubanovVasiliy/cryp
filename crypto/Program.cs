// See https://aka.ms/new-console-template for more information

using crypto;


//Console.WriteLine(Encryption.Shamir(83862086));
Console.WriteLine(Encryption.ElGamal(10));
//Console.WriteLine(Encryption.RSA(6));


 // var encryptor = new Encryption.Xor();
 // encryptor.Encrypt("t.jpg");
 // encryptor.Decrypt("XorEnc_t.jpg");


var m = new Random().Next();
var encryptorRsaA = new Encryption.Rsa();
var encryptorRsaB = new Encryption.Rsa();

encryptorRsaA.Encrypt("t.jpg", encryptorRsaB.D, encryptorRsaB.N);
encryptorRsaB.Decrypt("RsaEnc_t.jpg",encryptorRsaB.N);
