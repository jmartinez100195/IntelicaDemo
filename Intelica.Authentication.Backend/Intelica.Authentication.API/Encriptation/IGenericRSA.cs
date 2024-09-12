namespace Intelica.Authentication.API.Encriptation
{
    public interface IGenericRSA
    {
        string GetPublicKey();
        string Decript(string privateKey, string value);
        string Encript(string publicKey, string value);
    }
}