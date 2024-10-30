using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestAllocation : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testRewardUid;
    private readonly string _testAllocationUid;

    public TestAllocation()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testRewardUid = Environment.GetEnvironmentVariable("TEST_REWARD_UID")
          ?? throw new InvalidOperationException("TEST_REWARD_UID is not set");
      _testAllocationUid = Environment.GetEnvironmentVariable("TEST_ALLOCATION_UID")
          ?? throw new InvalidOperationException("TEST_ALLOCATION_UID is not set");
    }

    [Fact]
    public async Task CreateAllocation_ReturnsUid()
    {
      var allocationData = new AllocationParams
      {
        Amount = 0.001,
        Nonce = GetRandomString(),
        RewardUid = _testRewardUid,
        ToUserUid = _testUserUid
      };

      var response = await _client.CreateAllocationAsync(allocationData);
      Assert.NotNull(response.Data.Uid);
    }

    [Fact]
    public async Task GetAllocation_ReturnsCorrectAllocation()
    {
      var response = await _client.GetAllocationAsync(_testAllocationUid);
      Assert.Equal(_testAllocationUid, response.Data.Uid);
    }

    [Fact]
    public async Task GetAllocation_NotFound_Throws404()
    {
      var exception = await Assert.ThrowsAsync<ClientException>(async () => await _client.GetAllocationAsync("not_found"));
      Assert.Equal(404, exception.Status);
    }

    [Fact]
    public async Task GetAllocationsByUserUid_ReturnsList()
    {
      var response = await _client.GetAllocationsByUserUidAsync(_testUserUid);
      Assert.IsType<List<Allocation>>(response.Data);
    }

    [Fact]
    public async Task GetAllocationsByUserUid_WithNoResults_ReturnsEmptyList()
    {
      var response = await _client.GetAllocationsByUserUidAsync("no_results");
      Assert.Empty(response.Data);
    }
  }
}
