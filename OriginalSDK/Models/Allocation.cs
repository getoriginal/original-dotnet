using Newtonsoft.Json;

namespace OriginalSDK.Models
{
  public class AllocationParams
  {
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("nonce")]
    public required string Nonce { get; set; }

    [JsonProperty("reward_uid")]
    public required string RewardUid { get; set; }

    [JsonProperty("to_user_uid")]
    public required string ToUserUid { get; set; }
  }

  public class Allocation
  {
    [JsonProperty("amount")]
    public decimal Amount { get; set; }

    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }

    [JsonProperty("nonce")]
    public required string Nonce { get; set; }

    [JsonProperty("reward_uid")]
    public required string RewardUid { get; set; }

    [JsonProperty("status")]
    public required string Status { get; set; }

    [JsonProperty("to_user_uid")]
    public required string ToUserUid { get; set; }

    [JsonProperty("uid")]
    public required string Uid { get; set; }
  }
}