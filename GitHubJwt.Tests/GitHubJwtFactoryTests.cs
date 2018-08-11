using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SilverGiggle;

namespace GitHubJwt.Tests
{
    [TestClass]
    public class GitHubJwtFactoryTests
    {

        [TestMethod]
        public void CreateEncodedJwtToken_FromFileSource_ShouldNotFail()
        {
            // Arrange
            var privateKeySource = new FilePrivateKeySource("private.pem");
            var options = new GitHubJwtFactoryOptions
            {
                AppIntegrationId = 6837,
                ExpirationSeconds = 600 // 10 minutes maximum
            };
            var factory = new GitHubJwtFactory(privateKeySource, options);

            // Act
            var token = factory.CreateEncodedJwtToken();

            // Assert
            Assert.IsNotNull(token);
            Console.WriteLine(token);
        }

        [TestMethod]
        public void CreateEncodedJwtToken_FromEnvVar_ShouldNotFail()
        {
            // Arrange
            var privateKeyName = Guid.NewGuid().ToString("N");
            var privateKeySource = new EnvironmentVariablePrivateKeySource(privateKeyName);
            var options = new GitHubJwtFactoryOptions
            {
                AppIntegrationId = 6837,
                ExpirationSeconds = 600 // 10 minutes maximum
            };
            var factory = new GitHubJwtFactory(privateKeySource, options);

            using (new EnvironmentVariableScope(privateKeyName))
            {
                Environment.SetEnvironmentVariable(privateKeyName, File.ReadAllText("envvar.pem"));

                // Act
                var token = factory.CreateEncodedJwtToken();

                // Assert
                Assert.IsNotNull(token);
                Console.WriteLine(token);
            }
        }

        [TestMethod]
        public void CreateEncodedJwtToken_FromString_ShouldNotFail()
        {
            // Arrange
            var privateKeySource = new StringPrivateKeySource(File.ReadAllText("envvar.pem"));
            var options = new GitHubJwtFactoryOptions
            {
                AppIntegrationId = 6837,
                ExpirationSeconds = 600 // 10 minutes maximum
            };
            var factory = new GitHubJwtFactory(privateKeySource, options);

            // Act
            var token = factory.CreateEncodedJwtToken();

            // Assert
            Assert.IsNotNull(token);
            Console.WriteLine(token);
        }

    }
}
