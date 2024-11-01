using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class TransferParams
  {
    [JsonProperty("asset_uid")]
    public string AssetUid { get; set; }

    [JsonProperty("from_user_uid")]
    public string FromUserUid { get; set; }

    [JsonProperty("to_address")]
    public string ToAddress { get; set; }
  }

  public class Transfer
  {
    [JsonProperty("asset_uid")]
    public string AssetUid { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("from_user_uid")]
    public string FromUserUid { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("to_address")]
    public string ToAddress { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }
  }
}