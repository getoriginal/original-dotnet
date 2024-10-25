using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class BurnParams
  {
    [JsonProperty("asset_uid")]
    public required string AssetUid { get; set; }

    [JsonProperty("from_user_uid")]
    public required string FromUserUid { get; set; }
  }

  public class Burn
  {
    [JsonProperty("asset_uid")]
    public required string AssetUid { get; set; }

    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }

    [JsonProperty("from_user_uid")]
    public required string FromUserUid { get; set; }

    [JsonProperty("status")]
    public required string Status { get; set; }

    [JsonProperty("uid")]
    public required string Uid { get; set; }
  }
}