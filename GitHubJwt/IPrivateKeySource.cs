using System.IO;

namespace GitHubJwt
{
    public interface IPrivateKeySource
    {
        TextReader GetPrivateKeyReader();
    }
}
