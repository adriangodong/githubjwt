namespace GitHubJwt
{
    public interface IGitHubJwtFactory
    {
        string CreateEncodedJwtToken(int iatOffset = 0);
    }
}
