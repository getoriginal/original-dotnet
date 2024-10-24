using OriginalSDK;
using OriginalSDK.Models;

class Program
{
  static async Task Main(string[] args)
  {
    var root = Directory.GetCurrentDirectory();
    var dotenv = Path.Combine(root, ".env");
    DotNetEnv.Env.Load(dotenv);

    // Call your SDK methods here
    // var options = new OriginalOptions { Environment = OriginalSDK.Environment.Development, BaseUrl = BASE_URL };
    // var client = new OriginalClient(API_KEY, API_SECRET, options);
    var client = new OriginalClient();

    var response = await client.GetUserAsync("364718224260");
    Console.WriteLine($"email {response.Data.Email}");
    Console.WriteLine($"uid {response.Data.Uid}");
    Console.WriteLine($"createdAt {response.Data.CreatedAt}");
    Console.WriteLine($"userExternalId {response.Data.UserExternalId}");
    Console.WriteLine($"Success", response.Success);

    var col = await client.GetCollectionAsync("940830618348");
    Console.WriteLine($"uid {col.Data.Uid}");
    Console.WriteLine($"name {col.Data.Name}");
    Console.WriteLine($"status {col.Data.Status}");
    Console.WriteLine($"type {col.Data.Type}");
    Console.WriteLine($"createdAt {col.Data.CreatedAt}");
    Console.WriteLine($"editableAssets {col.Data.EditableAssets}");
    Console.WriteLine($"contractAddress {col.Data.ContractAddress}");
    Console.WriteLine($"symbol {col.Data.Symbol}");

    var assetResponse = await client.CreateAssetAsync(new AssetParams
    {
      UserUid = "364718224260",
      CollectionUid = "940830618348",
      AssetExternalId = "AssetExternalId",
      SalePriceInUsd = (decimal)1.0,
      Data = new AssetData
      {
        Name = "Asset Name",
        Description = "Asset Description",
        ExternalUrl = "https://example.com",
        ImageUrl = "https://example.com/image.jpg",
        Attributes = new List<AssetAttribute>
        {
          new AssetAttribute
          {
            TraitType = "Trait Type",
            Value = "Value",
            DisplayType = "Display Type"
          }
        }
      }
    });

    Console.WriteLine($"assetUid {assetResponse.Data.Uid}");

  }
}