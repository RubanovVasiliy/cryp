// See https://aka.ms/new-console-template for more information

using crypto;

Console.WriteLine(Encryption.ElGamal(234234));



 // var encryptor = new Encryption.Xor();
 // encryptor.Encrypt("t.jpg");
 // encryptor.Decrypt("XorEnc_t.jpg");


// var encryptorRsaA = new Encryption.Rsa();
// var encryptorRsaB = new Encryption.Rsa();
//encryptorRsaA.Encrypt("t.jpg", encryptorRsaB.D, encryptorRsaB.N);
//encryptorRsaB.Decrypt("RsaEnc_t.jpg",encryptorRsaB.N);

//var p = CryptoLib.GenerateSimpleNumber(1000000000);
//var encryptorShamilA = new Encryption.Shamir(p);
//var encryptorShamilB = new Encryption.Shamir(encryptorShamilA.P);
//encryptorShamilA.Encrypt("t.jpg",encryptorShamilB.C,encryptorShamilB.D);
//encryptorShamilB.Decrypt("ShamirEnc_t.jpg",encryptorShamilA.C,encryptorShamilA.D);
