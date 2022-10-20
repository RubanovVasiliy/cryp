// See https://aka.ms/new-console-template for more information

using crypto;



    var encryptorRsaA = new Encryption.Rsa();
    encryptorRsaA.EncryptDigitalSignature("t.jpg");
    var res = Encryption.Rsa.CheckDigitalSignature("t.jpg", encryptorRsaA.D, encryptorRsaA.N);
    Console.WriteLine(res);


     var el = new Encryption.ElGamal();
     var y  = el.EncryptDigitalSignature("t.jpg");
     Console.WriteLine(Encryption.ElGamal.CheckDigitalSignature("t.jpg", y, el.P, el.G));
