using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestCreateTransferBurnAsset : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testCollectionUid;
    private readonly string _transferToUserUid;
    private readonly string _transferToWalletAddress;
    private readonly int _retryCounter;

    public TestCreateTransferBurnAsset()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testCollectionUid = Environment.GetEnvironmentVariable("TEST_COLLECTION_UID")
          ?? throw new InvalidOperationException("TEST_COLLECTION_UID is not set");
      _transferToUserUid = Environment.GetEnvironmentVariable("TEST_TRANSFER_TO_USER_UID")
          ?? throw new InvalidOperationException("TEST_TRANSFER_TO_USER_UID is not set");
      _transferToWalletAddress = Environment.GetEnvironmentVariable("TEST_TRANSFER_TO_WALLET_ADDRESS")
          ?? throw new InvalidOperationException("TEST_TRANSFER_TO_WALLET_ADDRESS is not set");
      _retryCounter = 20;
    }

    [Fact]
    public async Task FullCreateTransferBurnAssetFlow_CompletesSuccessfully()
    {
      // Step 1: Create Asset
      var assetName = GetRandomString();
      var assetData = new AssetData
      {
        Name = assetName,
        UniqueName = true,
        ImageUrl = "https://example.com/image.png",
        StoreImageOnIpfs = false,
        Description = "Asset description",
        Attributes = new List<AssetAttribute>
                {
                    new AssetAttribute { TraitType = "Eyes", Value = "Green" },
                    new AssetAttribute { TraitType = "Hair", Value = "Black" }
                }
      };

      var createAssetParams = new AssetParams
      {
        Data = assetData,
        UserUid = _testUserUid,
        AssetExternalId = assetName,
        CollectionUid = _testCollectionUid,
        SalePriceInUsd = 10.1
      };

      var assetResponse = await _client.CreateAssetAsync(createAssetParams);
      var assetUid = assetResponse.Data.Uid;
      Assert.NotNull(assetUid);

      // Step 2: Wait for Asset to be Transferable
      var isTransferable = await WaitForConditionAsync(() => _client.GetAssetAsync(assetUid), response => response.Data.IsTransferable);
      Assert.True(isTransferable, $"Asset {assetUid} is not transferable.");

      // Step 3: Transfer Asset
      var transferParams = new TransferParams
      {
        AssetUid = assetUid,
        FromUserUid = _testUserUid,
        ToAddress = _transferToWalletAddress
      };

      var transferResponse = await _client.CreateTransferAsync(transferParams);
      Assert.True(transferResponse.Success);
      var transferUid = transferResponse.Data.Uid;

      // Step 4: Wait for Transfer to complete
      var transferDone = await WaitForConditionAsync(() => _client.GetTransferAsync(transferUid), response => response.Data.Status == "done");
      Assert.True(transferDone, $"Transfer {transferUid} did not complete.");

      // Step 5: Burn Asset
      var burnParams = new BurnParams
      {
        AssetUid = assetUid,
        FromUserUid = _transferToUserUid
      };

      var burnResponse = await _client.CreateBurnAsync(burnParams);
      Assert.True(burnResponse.Success);
      var burnUid = burnResponse.Data.Uid;

      // Step 6: Wait for Burn to complete
      var burnDone = await WaitForConditionAsync(() => _client.GetBurnAsync(burnUid), response => response.Data.Status == "done");
      Assert.True(burnDone, $"Burn {burnUid} did not complete.");

      // Step 7: Verify Final Asset Burned Status
      var isBurned = await WaitForConditionAsync(() => _client.GetAssetAsync(assetUid), response => response.Data.IsBurned);
      Assert.True(isBurned, $"Asset {assetUid} was not burned successfully.");
    }

    private async Task<bool> WaitForConditionAsync<T>(Func<Task<ApiResponse<T>>> action, Func<ApiResponse<T>, bool> condition, int delaySeconds = 15)
    {
      var retries = 0;
      while (retries < _retryCounter)
      {
        var response = await action();
        if (condition(response))
        {
          return true;
        }

        await Task.Delay(TimeSpan.FromSeconds(delaySeconds));
        retries++;
      }

      return false;
    }
  }
}
