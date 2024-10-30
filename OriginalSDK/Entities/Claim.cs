using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class ClaimParams
  {
    [JsonProperty("from_user_uid")]
    public required string FromUserUid { get; set; }

    [JsonProperty("reward_uid")]
    public required string RewardUid { get; set; }

    [JsonProperty("to_address")]
    public required string ToAddress { get; set; }
  }

  public class Claim
  {
    [JsonProperty("amount")]
    public double? Amount { get; set; }

    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }

    [JsonProperty("from_user_uid")]
    public required string FromUserUid { get; set; }

    [JsonProperty("reward_uid")]
    public required string RewardUid { get; set; }

    [JsonProperty("status")]
    public required string Status { get; set; }

    [JsonProperty("to_address")]
    public required string ToAddress { get; set; }

    [JsonProperty("uid")]
    public required string Uid { get; set; }
  }
}