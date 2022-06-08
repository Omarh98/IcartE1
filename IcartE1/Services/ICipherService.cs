namespace IcartE1.Services
{
    public interface ICipherService
    {
        string Decrypt(string cipherText);
        string Encrypt(string input);
    }
}