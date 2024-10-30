using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestAsset : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testCollectionUid;
    private readonly string _testAssetUid;

    public TestAsset()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID")
          ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testCollectionUid = Environment.GetEnvironmentVariable("TEST_COLLECTION_UID")
          ?? throw new InvalidOperationException("TEST_COLLECTION_UID is not set");
      _testAssetUid = Environment.GetEnvironmentVariable("TEST_ASSET_UID")
          ?? throw new InvalidOperationException("TEST_ASSET_UID is not set");
    }

    [Fact]
    public async Task CreateAsset_ReturnsUid()
    {
      var assetName = GetRandomString();
      var assetData = new AssetData
      {
        Name = assetName,
        UniqueName = true,
        ImageUrl = "https://example.com/image.png",
        StoreImageOnIpfs = false,
        Attributes = new List<AssetAttribute>
                {
                    new AssetAttribute { TraitType = "Eyes", Value = "Green" },
                    new AssetAttribute { TraitType = "Hair", Value = "Black" }
                }
      };
      var requestData = new AssetParams
      {
        Data = assetData,
        UserUid = _testUserUid,
        AssetExternalId = assetName,
        CollectionUid = _testCollectionUid
      };

      var response = await _client.CreateAssetAsync(requestData);
      Assert.NotNull(response.Data.Uid);
    }

    [Fact]
    public async Task CreateAsset_WithMintPrice_ReturnsUid()
    {
      var assetName = GetRandomString();
      var assetData = new AssetData
      {
        Name = assetName,
        UniqueName = true,
        ImageUrl = "https://example.com/image.png",
        StoreImageOnIpfs = false,
        Attributes = new List<AssetAttribute>
                {
                    new AssetAttribute { TraitType = "Eyes", Value = "Green" },
                    new AssetAttribute { TraitType = "Hair", Value = "Black" }
                }
      };
      var requestData = new AssetParams
      {
        Data = assetData,
        UserUid = _testUserUid,
        AssetExternalId = assetName,
        CollectionUid = _testCollectionUid,
        SalePriceInUsd = 9.99,
      };

      var response = await _client.CreateAssetAsync(requestData);
      Assert.NotNull(response.Data.Uid);
    }

    [Fact]
    public async Task GetAsset_ReturnsCorrectAsset()
    {
      var response = await _client.GetAssetAsync(_testAssetUid);
      Assert.Equal(_testAssetUid, response.Data.Uid);
    }

    [Fact]
    public async Task GetAsset_NotFound_Throws404()
    {
      var exception = await Assert.ThrowsAsync<ClientException>(async () => await _client.GetAssetAsync("not_found"));
      Assert.Equal(404, exception.Status);
    }

    [Fact]
    public async Task GetAssetsByUserUid_ReturnsList()
    {
      var response = await _client.GetAssetsByUserUidAsync(_testUserUid);
      Assert.IsType<List<Asset>>(response.Data);
    }

    [Fact]
    public async Task GetAssetsByUserUid_WithNoResults_ReturnsEmptyList()
    {
      var response = await _client.GetAssetsByUserUidAsync("no_results");
      Assert.Empty(response.Data);
    }
  }
}
