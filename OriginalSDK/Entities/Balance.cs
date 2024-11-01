using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class Balance
  {
    [JsonProperty("user_uid")]
    public string UserUid { get; set; }

    [JsonProperty("reward_uid")]
    public string RewardUid { get; set; }

    [JsonProperty("amount")]
    public double Amount { get; set; }

  }
}
