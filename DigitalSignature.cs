using System;
using System.Security.Cryptography;
using System.Text;

namespace WebhooksReceiver
{
    public class DigitalSignature
    {
        public static string Generate(string text, string privateKey)
        {
            var preHash = Encoding.UTF8.GetBytes(text);

            using var provider = SHA256.Create();
            var hash = provider.ComputeHash(preHash);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(privateKey);
            var signatureBytes = rsa.SignHash(hash, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            return Convert.ToBase64String(signatureBytes);
        }

        public static bool Verify(string digitalSignature, string text, string publicKey)
        {
            var preHash = Encoding.UTF8.GetBytes(text);

            using var provider = SHA256.Create();
            var hash = provider.ComputeHash(preHash);

            var signatureBytes = Convert.FromBase64String(digitalSignature);

            using var rsa = RSA.Create();
            rsa.ImportFromPem(publicKey);
            return rsa.VerifyHash(hash, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        }
    }
}
