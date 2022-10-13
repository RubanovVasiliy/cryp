// See https://aka.ms/new-console-template for more information

using crypto;

//Console.WriteLine(Encryption.ElGamal(10));



 // var encryptor = new Encryption.Xor();
 // encryptor.Encrypt("t.jpg");
 // encryptor.Decrypt("XorEnc_t.jpg");


var m = new Random().Next();
var encryptorRsaA = new Encryption.Rsa();
var encryptorRsaB = new Encryption.Rsa();
encryptorRsaA.Encrypt("t.jpg", encryptorRsaB.D, encryptorRsaB.N);
encryptorRsaB.Decrypt("RsaEnc_t.jpg",encryptorRsaB.N);

var p = CryptoLib.GenerateSimpleNumber(1000000000);
var encryptorShamilA = new Encryption.Shamir(p);
var encryptorShamilB = new Encryption.Shamir(encryptorShamilA.P);

// var m = 123123;
// var x1 = encryptorShamilA.Encrypt(m);
// var x2 = encryptorShamilB.Encrypt(x1);
// var x3 = encryptorShamilA.Decrypt(x2);
// var x4 = encryptorShamilB.Decrypt(x3);
// Console.WriteLine(x4);

encryptorShamilA.Encrypt("t.jpg",encryptorShamilB.C,encryptorShamilB.D);
encryptorShamilB.Decrypt("ShamirEnc_t.jpg",encryptorShamilA.C,encryptorShamilA.D);
