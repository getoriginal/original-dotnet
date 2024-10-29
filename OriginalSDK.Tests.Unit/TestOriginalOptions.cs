using OriginalSDK.Tests.Unit.Helpers;

namespace OriginalSDK.Tests.Unit
{
  public class TestOriginalOptions : TestBase
  {
    [Fact]
    public void DefaultEnvironment_ShouldBeProduction()
    {
      var options = new OriginalOptions();
      Assert.Equal(OriginalEnvironment.Production, options.Environment);
    }

    [Fact]
    public void CanSetEnvironmentToDevelopment()
    {
      var options = new OriginalOptions
      {
        Environment = OriginalEnvironment.Development
      };

      Assert.Equal(OriginalEnvironment.Development, options.Environment);
    }

    [Fact]
    public void CanSetEnvironmentToProduction()
    {
      var options = new OriginalOptions
      {
        Environment = OriginalEnvironment.Production
      };
      Assert.Equal(OriginalEnvironment.Production, options.Environment);
    }

    [Fact]
    public void BaseUrl_ShouldBeNullByDefault()
    {
      var options = new OriginalOptions();
      Assert.Null(options.BaseUrl);
    }

    [Fact]
    public void CanSetBaseUrl()
    {
      var options = new OriginalOptions();
      var testUrl = "https://test-url.com/v1/";

      options.BaseUrl = testUrl;

      Assert.Equal(testUrl, options.BaseUrl);
    }

    [Fact]
    public void BaseUrl_ShouldBeOverwritten()
    {
      var options = new OriginalOptions();
      var initialUrl = "https://initial-url.com/v1/";
      var updatedUrl = "https://updated-url.com/v1/";

      options.BaseUrl = initialUrl;
      options.BaseUrl = updatedUrl;

      Assert.Equal(updatedUrl, options.BaseUrl);
    }
  }
}
