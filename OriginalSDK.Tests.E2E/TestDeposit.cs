using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestDeposit : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testCollectionUid;

    public TestDeposit()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testCollectionUid = Environment.GetEnvironmentVariable("TEST_COLLECTION_UID")
          ?? throw new InvalidOperationException("TEST_COLLECTION_UID is not set");
    }

    [Fact]
    public async Task GetDeposit_ReturnsCorrectDepositData()
    {
      var response = await _client.GetDepositAsync(_testUserUid, _testCollectionUid);

      Assert.True(response.Success);
      Assert.Equal(80002, response.Data.ChainId);
      Assert.Equal("Amoy", response.Data.Network);
    }
  }
}
