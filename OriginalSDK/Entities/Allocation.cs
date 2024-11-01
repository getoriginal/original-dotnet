using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class AllocationParams
  {
    [JsonProperty("amount")]
    public double Amount { get; set; }

    [JsonProperty("nonce")]
    public string Nonce { get; set; }

    [JsonProperty("reward_uid")]
    public string RewardUid { get; set; }

    [JsonProperty("to_user_uid")]
    public string ToUserUid { get; set; }
  }

  public class Allocation
  {
    [JsonProperty("amount")]
    public double Amount { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("nonce")]
    public string Nonce { get; set; }

    [JsonProperty("reward_uid")]
    public string RewardUid { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("to_user_uid")]
    public string ToUserUid { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }
  }
}