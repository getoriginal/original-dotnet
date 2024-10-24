using Newtonsoft.Json;

namespace OriginalSDK.Models
{
  public class EditAssetData
  {
    [JsonProperty("image_url")]
    public required string ImageUrl { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("unique_name")]
    public bool UniqueName { get; set; }

    [JsonProperty("attributes")]
    public List<AssetAttribute>? Attributes { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }

    [JsonProperty("external_url")]
    public string? ExternalUrl { get; set; }
  }

  public class EditAssetParams
  {
    [JsonProperty("data")]
    public required EditAssetData Data { get; set; }
  }
}