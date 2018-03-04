using System;
using System.IO;
using System.Text;

namespace GitHubJwt
{
    public class EnvironmentVariablePrivateKeySource : IPrivateKeySource
    {
        private readonly string environmentVariableName;

        public EnvironmentVariablePrivateKeySource(string environmentVariableName)
        {
            this.environmentVariableName = environmentVariableName;
        }

        public TextReader GetPrivateKeyReader()
        {
            var privateKeyPem = HydrateEnvVarPem(
                Environment.GetEnvironmentVariable(environmentVariableName));
            return new StringReader(privateKeyPem);
        }

        private static string HydrateEnvVarPem(string input)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("-----BEGIN RSA PRIVATE KEY-----");
            stringBuilder.AppendLine(input);
            stringBuilder.AppendLine("-----END RSA PRIVATE KEY-----");

            return stringBuilder.ToString();
        }
    }
}
