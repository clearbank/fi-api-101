using System;
using System.Security.Cryptography;
using System.Text;

namespace WebhooksReceiver
{
    public class DigitalSignature
    {
        public static string Generate(string text, string privateKey)
        {
            var hash = HashString(text);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey);
            var signatureBytes = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        public static bool Verify(string digitalSignature, string text, string publicKey)
        {
            var hash = HashString(text);

            var signatureBytes = Convert.FromBase64String(digitalSignature);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);
            return rsa.VerifyHash(hash, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }

        private static byte[] HashString(string text)
        {
            var preHash = Encoding.UTF8.GetBytes(text);

            using var provider = SHA256.Create();
            return provider.ComputeHash(preHash);
        }
    }
}
