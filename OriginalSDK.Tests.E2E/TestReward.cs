using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestReward : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testRewardUid;
    private readonly string _testUserUid;

    public TestReward()
    {
      _client = new OriginalClient();
      _testRewardUid = Environment.GetEnvironmentVariable("TEST_REWARD_UID")
          ?? throw new InvalidOperationException("TEST_REWARD_UID is not set");
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
    }

    [Fact]
    public async Task GetReward_ReturnsCorrectReward()
    {
      var response = await _client.GetRewardAsync(_testRewardUid);
      Assert.Equal(_testRewardUid, response.Data.Uid);
    }

    [Fact]
    public async Task GetReward_NotFound_Throws404()
    {
      var exception = await Assert.ThrowsAsync<ClientException>(async () => await _client.GetRewardAsync("not_found"));
      Assert.Equal(404, exception.Status);
    }

    [Fact]
    public async Task GetBalance_ReturnsCorrectBalance()
    {
      var response = await _client.GetBalanceAsync(_testRewardUid, _testUserUid);
      Assert.Equal(_testRewardUid, response.Data.RewardUid);
      Assert.Equal(_testUserUid, response.Data.UserUid);
    }
  }
}
