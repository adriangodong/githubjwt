namespace GitHubJwt
{
    public interface IGitHubJwtFactory
    {
        string CreateEncodedJwtToken(System.TimeSpan? iatOffset = null);
    }
}
