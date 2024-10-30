using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestCreateEditAsset : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testCollectionUid;
    private readonly int _retryCounter;

    public TestCreateEditAsset()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testCollectionUid = Environment.GetEnvironmentVariable("TEST_COLLECTION_UID")
          ?? throw new InvalidOperationException("TEST_COLLECTION_UID is not set");
      _retryCounter = 20;
    }

    [Fact]
    public async Task CreateAndEditAssetFlow_CompletesSuccessfully()
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
      var isTransferable = await WaitForAssetTransferable(assetUid);
      Assert.True(isTransferable, $"Asset {assetUid} is not transferable.");

      // Step 3: Edit Asset
      assetData.Description = "Asset description edited";
      var editAssetParams = new EditAssetParams
      {
        Data = new EditAssetData
        {
          ImageUrl = "https://example.com/image.png",
          Name = assetName,
          UniqueName = true,
          Attributes = new List<AssetAttribute>
          {
            new AssetAttribute { TraitType = "Eyes", Value = "Green" },
            new AssetAttribute { TraitType = "Hair", Value = "Black" }
          },
          Description = "Asset description edited",
          ExternalUrl = "https://example.com/asset",
        }
      };
      var editResponse = await _client.EditAssetAsync(assetUid, editAssetParams);

      Assert.True(editResponse.Success, "Asset edit operation failed.");
    }

    private async Task<bool> WaitForAssetTransferable(string assetUid)
    {
      var retries = 0;
      while (retries < _retryCounter)
      {
        var response = await _client.GetAssetAsync(assetUid);
        if (response.Data.IsTransferable)
        {
          return true;
        }

        retries++;
        await Task.Delay(TimeSpan.FromSeconds(15));
      }

      return false;
    }
  }
}
