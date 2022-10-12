namespace crypto;

public interface IEncryptable
{
    public long Encrypt(long m);
    public long Decrypt(long m);
    public void Encrypt(string filepath);
    public void Decrypt(string filepath);
}