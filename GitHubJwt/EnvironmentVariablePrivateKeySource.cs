using System;
using System.IO;

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
            var privateKeyPem = Environment.GetEnvironmentVariable(environmentVariableName).HydrateRsaVariable();
            return new StringReader(privateKeyPem);
        }

    }
}
