using System.IO;

namespace GitHubJwt
{
    public class FilePrivateKeySource : IPrivateKeySource
    {
        private readonly string filePath;

        public FilePrivateKeySource(string filePath)
        {
            this.filePath = filePath;
        }

        public TextReader GetPrivateKeyReader()
        {
            return new StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read));
        }
    }
}
