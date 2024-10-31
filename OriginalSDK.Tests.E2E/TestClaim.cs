using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestClaim : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testRewardUid;
    private readonly string _testUserUid;
    private readonly string _testClaimUid;
    private readonly string _testClaimToAddress;
    private readonly int _retryCounter;


    public TestClaim()
    {
      _client = new OriginalClient();
      _testRewardUid = Environment.GetEnvironmentVariable("TEST_REWARD_UID")
          ?? throw new InvalidOperationException("TEST_REWARD_UID is not set");
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testClaimUid = Environment.GetEnvironmentVariable("TEST_CLAIM_UID")
          ?? throw new InvalidOperationException("TEST_CLAIM_UID is not set");
      _testClaimToAddress = Environment.GetEnvironmentVariable("TEST_CLAIM_TO_ADDRESS")
          ?? throw new InvalidOperationException("TEST_CLAIM_TO_ADDRESS is not set");
      _retryCounter = 20;
    }

    [Fact]
    public async Task CreateClaim_ReturnsUid()
    {
      await WaitForNoClaimsInProgress();

      var claimParams = new ClaimParams
      {
        RewardUid = _testRewardUid,
        FromUserUid = _testUserUid,
        ToAddress = _testClaimToAddress
      };
      var response = await _client.CreateClaimAsync(claimParams);
      Assert.NotNull(response.Data.Uid);
    }

    [Fact]
    public async Task GetClaim_ReturnsCorrectClaim()
    {
      var response = await _client.GetClaimAsync(_testClaimUid);
      Assert.Equal(_testClaimUid, response.Data.Uid);
    }

    [Fact]
    public async Task GetClaim_NotFound_Throws404()
    {
      var exception = await Assert.ThrowsAsync<ClientException>(
          async () => await _client.GetClaimAsync("not_found"));

      Assert.Equal(404, exception.Status);
    }

    [Fact]
    public async Task GetClaimsByUserUid_ReturnsList()
    {
      var response = await _client.GetClaimsByUserUidAsync(_testUserUid);
      Assert.IsType<List<Claim>>(response.Data);
    }

    [Fact]
    public async Task GetClaimsByUserUid_WithNoResults_ReturnsEmptyList()
    {
      var response = await _client.GetClaimsByUserUidAsync("no_results");
      Assert.Empty(response.Data);
    }

    private async Task WaitForNoClaimsInProgress()
    {
      var retries = 0;
      while (retries < _retryCounter)
      {
        var response = await _client.GetClaimsByUserUidAsync(_testUserUid);

        if (response.Data.Count == 0)
        {
          return;
        }
        var hasPendingClaims = false;
        foreach (var claim in response.Data)
        {
          if (claim.Status == "pending")
          {
            hasPendingClaims = true;
            break;
          }
        }
        if (!hasPendingClaims)
        {
          return;
        }

        retries++;
        await Task.Delay(TimeSpan.FromSeconds(15));
      }

      throw new TimeoutException("Claims are still in progress after the maximum retry limit.");
    }
  }
}
