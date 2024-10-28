using OriginalSDK.Tests.Helpers;

namespace OriginalSDK.Tests
{
    public class TestTokenManager : TestBase
    {
        [Fact]
        public void TokenManagerConstructor_ThrowsErrorIfSecretIsNull()
        {
            Assert.Throws<ArgumentException>(() => new TokenManager(null!));
        }

        [Fact]
        public void TokenManagerConstructor_ThrowsErrorIfSecretIsTooShort()
        {
            Assert.Throws<ArgumentException>(() => new TokenManager("smallsecret"));
        }

        [Fact]
        public void GenerateToken_ReturnsToken()
        {
            var tokenManager = new TokenManager("asuperveryverylongsecretkeythatis32chars");
            var token = tokenManager.GenerateToken();
            Assert.NotNull(token);
        }
    }
}