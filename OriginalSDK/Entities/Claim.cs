using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class ClaimParams
  {
    [JsonProperty("from_user_uid")]
    public string FromUserUid { get; set; }

    [JsonProperty("reward_uid")]
    public string RewardUid { get; set; }

    [JsonProperty("to_address")]
    public string ToAddress { get; set; }
  }

  public class Claim
  {
    [JsonProperty("amount")]
    public double? Amount { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("from_user_uid")]
    public string FromUserUid { get; set; }

    [JsonProperty("reward_uid")]
    public string RewardUid { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("to_address")]
    public string ToAddress { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }
  }
}