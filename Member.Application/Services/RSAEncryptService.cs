using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Member.Application.Services
{
    // RSA encryption service
    public class RSAEncryptService
    {
        private RSA _rsa;

        public RSAEncryptService()
        {
            //Generate a new RSA key pair
            _rsa = RSA.Create(2048); // Use a key length of 2048 bits for security
        }
        
        public string getPublicKey()
        {
            return _rsa.ToXmlString(false);
        }

        public string Encrypt(string plainText)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] encryptedBytes = _rsa.Encrypt(plainBytes, RSAEncryptionPadding.Pkcs1);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string Decrypt(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] decryptedBytes = _rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}
