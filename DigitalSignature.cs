using System;
using System.Security.Cryptography;
using System.Text;

namespace WebhooksReceiver
{
    public static class DigitalSignature
    {
        public static string Generate(string text, string privateKey)
        {
            using RSACryptoServiceProvider signingAlgorithm = new();

            signingAlgorithm.ImportFromPem(privateKey);

            using var digestAlgorithm = SHA256.Create();

            var signatureBytes = signingAlgorithm.SignData(Encoding.UTF8.GetBytes(text), digestAlgorithm);

            return Convert.ToBase64String(signatureBytes);
        }

        public static bool Verify(string digitalSignature, string text, string publicKey)
        {
            var signatureBytes = Convert.FromBase64String(digitalSignature);

            using RSACryptoServiceProvider signingAlgorithm = new();

            signingAlgorithm.ImportFromPem(publicKey);

            using var digestAlgorithm = SHA256.Create();

            return signingAlgorithm.VerifyData(Encoding.UTF8.GetBytes(text), digestAlgorithm, signatureBytes);
        }
    }
}
