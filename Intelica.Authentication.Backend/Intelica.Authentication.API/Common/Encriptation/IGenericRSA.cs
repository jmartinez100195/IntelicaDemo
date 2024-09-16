namespace Intelica.Authentication.API.Common.Encriptation
{
    public interface IGenericRSA
    {
        KeyValuePair<string, string> GetPublicKey();
        string GetPublicKeyXML();
        string Decript(string privateKey, string value);
        string Encript(string publicKey, string value);
        string DecriptXML(string privateKey, string value);
        string EncriptXML(string publicKey, string value);
    }
}