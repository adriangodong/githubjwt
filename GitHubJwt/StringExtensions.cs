using System.Text;

namespace GitHubJwt
{
    internal static class StringExtensions
    {
        public static string HydrateRsaVariable(this string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("-----BEGIN RSA PRIVATE KEY-----");
            stringBuilder.AppendLine(input);
            stringBuilder.AppendLine("-----END RSA PRIVATE KEY-----");
            return stringBuilder.ToString();
        }
    }
}
