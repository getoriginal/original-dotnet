using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestTransfer : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;

    public TestTransfer()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
    }

    [Fact]
    public async Task GetTransfersByUserUid_ReturnsList()
    {
      var response = await _client.GetTransfersByUserUidAsync(_testUserUid);
      Assert.IsType<List<Transfer>>(response.Data);
    }
  }
}
