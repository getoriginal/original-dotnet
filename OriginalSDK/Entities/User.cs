using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class UserParams
  {
    [JsonProperty("user_external_id")]
    public string UserExternalId { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }
  }

  public class UserWallet
  {
    [JsonProperty("address")]
    public string Address { get; set; }

    [JsonProperty("explorer_url")]
    public string ExplorerUrl { get; set; }

    [JsonProperty("network")]
    public string Network { get; set; }

    [JsonProperty("chain_id")]
    public int ChainId { get; set; }
  }

  public class User
  {
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [JsonProperty("user_external_id")]
    public string UserExternalId { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("email")]
    public string Email { get; set; }

    [JsonProperty("wallets")]
    public List<UserWallet> Wallets { get; set; }
  }
}
