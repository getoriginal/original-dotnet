using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class BurnParams
  {
    [JsonProperty("asset_uid")]
    public string AssetUid { get; set; }

    [JsonProperty("from_user_uid")]
    public string FromUserUid { get; set; }
  }

  public class Burn
  {
    [JsonProperty("asset_uid")]
    public string AssetUid { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("from_user_uid")]
    public string FromUserUid { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }
  }
}