using Intelica.Infrastructure.Library.Cache.Interface;
using System.Security.Cryptography;
using System.Text;
namespace Intelica.Authentication.API.Encriptation
{
    public class GenericRSA(IGenericCache genericCache) : IGenericRSA
    {
        public string Decript(string privateKey, string value)
        {
            var privateKeyBytes = Convert.FromBase64String(privateKey);
            using var rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            var encryptedTextBytes = Convert.FromBase64String(value);
            var decryptedTextBytes = rsa.Decrypt(encryptedTextBytes, RSAEncryptionPadding.Pkcs1);
            var decryptedText = Encoding.UTF8.GetString(decryptedTextBytes);
            return decryptedText;


        }
        public string DecriptXML(string privateKey, string value)
        {
            var privateKeyBytes = Convert.FromBase64String(privateKey);
            using var rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
            var encryptedTextBytes = Convert.FromBase64String(value);
            var decryptedTextBytes = rsa.Decrypt(encryptedTextBytes, RSAEncryptionPadding.Pkcs1);
            var decryptedText = Encoding.UTF8.GetString(decryptedTextBytes);
            return decryptedText;


        }
        public string Encript(string publicKey, string value)
        {
           // var publickKeyBytes = Convert.FromBase64String(publicKey);
            using var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            var textBytes = Encoding.ASCII.GetBytes(value);
            var encryptedTextBytes = rsa.Encrypt(textBytes, false);
            var encryptedText = Convert.ToBase64String(encryptedTextBytes) ;
            return encryptedText;
        }
        public string GetPublicKey()
        {
            using var rsa = RSA.Create(2048);

            string publicKey = rsa.ToXmlString(false); // Solo la clave pública
            string privateKey = rsa.ToXmlString(true);


            //var privateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
            //var publicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
            genericCache.Set(privateKey, publicKey);
            return publicKey;
        }
    }
}