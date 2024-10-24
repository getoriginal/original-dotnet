using Newtonsoft.Json;

namespace OriginalSDK.Models
{
  public class BalanceParams
  {
    [JsonProperty("user_uid")]
    public string? UserUid { get; set; }

    [JsonProperty("reward_uid")]
    public string? RewardUid { get; set; }
  }

  public class Balance
  {
    [JsonProperty("user_uid")]
    public required string UserUid { get; set; }

    [JsonProperty("reward_uid")]
    public required string RewardUid { get; set; }

    [JsonProperty("amount")]
    public decimal Amount { get; set; }

  }
}
