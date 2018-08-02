using System;
using System.IO;
using System.Text;

namespace GitHubJwt
{
    public class StringPrivateKeySource : IPrivateKeySource
    {
        protected readonly string Key;

        public StringPrivateKeySource(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            Key = key;
        }

        public TextReader GetPrivateKeyReader()
        {
            return new StringReader(Key.HydrateRsaVariable());
        }
    }
}
