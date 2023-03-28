using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Jose;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.OpenSsl;

namespace GitHubJwt
{
    public class GitHubJwtFactory : IGitHubJwtFactory
    {
        private static readonly long TicksSince197011 = new DateTime(1970, 1, 1).Ticks;
        private readonly IPrivateKeySource privateKeySource;
        private readonly GitHubJwtFactoryOptions options;

        public GitHubJwtFactory(IPrivateKeySource privateKeySource, GitHubJwtFactoryOptions options)
        {
            this.privateKeySource = privateKeySource;
            this.options = options;
        }

        public string CreateEncodedJwtToken(TimeSpan? iatOffset = null)
        {
            var utcNow = DateTime.UtcNow.Add(iatOffset ?? TimeSpan.Zero);

            var payload = new Dictionary<string, object>
            {
                {"iat", ToUtcSeconds(utcNow)},
                {"exp", ToUtcSeconds(utcNow.AddSeconds(options.ExpirationSeconds))},
                {"iss", options.AppIntegrationId}
            };

            // Generate JWT
            using (var rsa = new RSACryptoServiceProvider())
            {
                var rsaParams = ToRSAParameters(GetPrivateKey());
                rsa.ImportParameters(rsaParams);
                return JWT.Encode(payload, rsa, JwsAlgorithm.RS256);
            }
        }

        private RsaPrivateCrtKeyParameters GetPrivateKey()
        {
            using (var privateKeyReader = privateKeySource.GetPrivateKeyReader())
            {
                var pemReader = new PemReader(privateKeyReader);
                var keyPair = (AsymmetricCipherKeyPair)pemReader.ReadObject();
                return (RsaPrivateCrtKeyParameters)keyPair.Private;
            }
        }

        private static RSAParameters ToRSAParameters(RsaPrivateCrtKeyParameters privKey)
        {
            var rp = new RSAParameters();
            rp.Modulus = privKey.Modulus.ToByteArrayUnsigned();
            rp.Exponent = privKey.PublicExponent.ToByteArrayUnsigned();
            rp.P = privKey.P.ToByteArrayUnsigned();
            rp.Q = privKey.Q.ToByteArrayUnsigned();
            rp.D = ConvertRSAParametersField(privKey.Exponent, rp.Modulus.Length);
            rp.DP = ConvertRSAParametersField(privKey.DP, rp.P.Length);
            rp.DQ = ConvertRSAParametersField(privKey.DQ, rp.Q.Length);
            rp.InverseQ = ConvertRSAParametersField(privKey.QInv, rp.Q.Length);
            return rp;
        }

        private static byte[] ConvertRSAParametersField(BigInteger n, int size)
        {
            byte[] bs = n.ToByteArrayUnsigned();

            if (bs.Length == size)
                return bs;

            if (bs.Length > size)
                throw new ArgumentException("Specified size too small", nameof(size));

            byte[] padded = new byte[size];
            Array.Copy(bs, 0, padded, size - bs.Length, bs.Length);
            return padded;
        }

        private static long ToUtcSeconds(DateTime dt)
        {
            return (dt.ToUniversalTime().Ticks - TicksSince197011) / TimeSpan.TicksPerSecond;
        }
    }
}
