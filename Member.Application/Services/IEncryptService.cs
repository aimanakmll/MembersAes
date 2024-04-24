using System;
using System.Security.Cryptography;
using System.Text;

namespace Member.Application
{
    public interface IEncryptService
    {
        string EncryptPassword(string password);
        string DecryptPassword(string encryptedPassword);
    }

    public class EncryptService : IEncryptService
    {
        private readonly byte[] _iv = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        private readonly int _blockSize = 128;
        private readonly string _encryptionKey;

        public EncryptService(string encryptionKey)
        {
            _encryptionKey = encryptionKey;
        }

        public string EncryptPassword(string password)
        {
            return EncryptText(password, _encryptionKey);
        }

        public string DecryptPassword(string encryptedPassword)
        {
            return DecryptText(encryptedPassword, _encryptionKey);
        }

        private string EncryptText(string plainText, string password)
        {
            using (SymmetricAlgorithm crypt = Aes.Create())
            {
                using (HashAlgorithm hash = MD5.Create())
                {
                    crypt.BlockSize = _blockSize;
                    crypt.Key = hash.ComputeHash(Encoding.UTF8.GetBytes(password)); // Change encoding to UTF8
                    crypt.IV = _iv;

                    byte[] bytes = Encoding.UTF8.GetBytes(plainText); // Change encoding to UTF8
                    using (var memoryStream = new System.IO.MemoryStream())
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, crypt.CreateEncryptor(), CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(bytes, 0, bytes.Length);
                        }
                        return Convert.ToBase64String(memoryStream.ToArray());
                    }
                }
            }
        }

        private string DecryptText(string encryptedText, string password)
        {
            byte[] bytes = Convert.FromBase64String(encryptedText);
            using (SymmetricAlgorithm crypt = Aes.Create())
            {
                using (HashAlgorithm hash = MD5.Create())
                {
                    crypt.Key = hash.ComputeHash(Encoding.UTF8.GetBytes(password)); // Change encoding to UTF8
                    crypt.IV = _iv;

                    using (var memoryStream = new System.IO.MemoryStream(bytes))
                    {
                        using (var cryptoStream = new CryptoStream(memoryStream, crypt.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            byte[] decryptedBytes = new byte[bytes.Length];
                            int decryptedLength = cryptoStream.Read(decryptedBytes, 0, decryptedBytes.Length);
                            // Trim null characters from the decrypted string
                            string decryptedString = Encoding.UTF8.GetString(decryptedBytes, 0, decryptedLength);
                            return decryptedString;
                        }
                    }
                }
            }
        }
    }
}
