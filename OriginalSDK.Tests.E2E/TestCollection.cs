using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestCollection : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testCollectionUid;

    public TestCollection()
    {
      _client = new OriginalClient();
      _testCollectionUid = Environment.GetEnvironmentVariable("TEST_COLLECTION_UID")
          ?? throw new InvalidOperationException("TEST_COLLECTION_UID is not set");
    }

    [Fact]
    public async Task GetCollection_ReturnsCorrectCollection()
    {
      var response = await _client.GetCollectionAsync(_testCollectionUid);
      Assert.Equal(_testCollectionUid, response.Data.Uid);
    }

    [Fact]
    public async Task GetCollection_NotFound_Throws404()
    {
      var exception = await Assert.ThrowsAsync<ClientException>(
          async () => await _client.GetCollectionAsync("not_found"));

      Assert.Equal(404, exception.Status);
    }
  }
}
