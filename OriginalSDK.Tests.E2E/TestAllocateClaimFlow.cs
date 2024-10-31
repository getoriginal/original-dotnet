using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestAllocationClaimFlow : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testRewardUid;
    private readonly string _testClaimToAddress;
    private readonly int _retryCounter;

    public TestAllocationClaimFlow()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testRewardUid = Environment.GetEnvironmentVariable("TEST_REWARD_UID")
          ?? throw new InvalidOperationException("TEST_REWARD_UID is not set");
      _testClaimToAddress = Environment.GetEnvironmentVariable("TEST_CLAIM_TO_ADDRESS")
          ?? throw new InvalidOperationException("TEST_CLAIM_TO_ADDRESS is not set");
      _retryCounter = 20;
    }

    [Fact]
    public async Task FullAllocateClaimFlow_CompletesSuccessfully()
    {
      // Step 1: Create Allocation
      var allocationData = new AllocationParams
      {
        Amount = 0.001,
        Nonce = GetRandomString(),
        RewardUid = _testRewardUid,
        ToUserUid = _testUserUid
      };

      var response = await _client.CreateAllocationAsync(allocationData);
      var allocationUid = response.Data.Uid;
      Assert.NotNull(allocationUid);

      // Step 2: Wait for Allocation to Complete
      await WaitForStatusChange(
          async () => await _client.GetAllocationAsync(allocationUid),
          allocation => allocation.Data.Status == "done",
          "Allocation");

      // Step 3: Wait for No Claims in Progress
      await WaitForNoClaimsInProgress();

      // Step 4: Create Claim
      var claimData = new ClaimParams
      {
        RewardUid = _testRewardUid,
        FromUserUid = _testUserUid,
        ToAddress = _testClaimToAddress
      };

      response = await _client.CreateClaimAsync(claimData);
      var claimUid = response.Data.Uid;
      Assert.NotNull(claimUid);

      // Step 5: Wait for Claim to Complete
      await WaitForStatusChange(
          async () => await _client.GetClaimAsync(claimUid),
          claim => claim.Data.Status == "done",
          "Claim");
    }

    private async Task WaitForNoClaimsInProgress()
    {
      for (var retries = 0; retries < _retryCounter; retries++)
      {
        var response = await _client.GetClaimsByUserUidAsync(_testUserUid);

        // Exit if no claims have a "pending" status
        if (response.Data.All(claim => claim.Status != "pending"))
        {
          return;
        }

        await Task.Delay(TimeSpan.FromSeconds(15));
      }

      throw new TimeoutException("Claims are still in progress after the maximum retry limit.");
    }


    private async Task WaitForStatusChange<T>(Func<Task<ApiResponse<T>>> getStatusFunc, Func<ApiResponse<T>, bool> isDoneFunc, string actionName)
    {
      var retries = 0;
      while (retries < _retryCounter)
      {
        var response = await getStatusFunc();
        if (isDoneFunc(response))
        {
          Assert.True(response.Success, $"{actionName} is not complete.");
          return;
        }

        retries++;
        await Task.Delay(TimeSpan.FromSeconds(15));
      }

      throw new TimeoutException($"{actionName} did not complete within the expected time.");
    }
  }
}
