using Newtonsoft.Json;

namespace OriginalSDK.Models
{
  public class UserParams
  {
    [JsonProperty("user_external_id")]
    public string? UserExternalId { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }
  }

  public class UserWallet
  {
    [JsonProperty("address")]
    public required string Address { get; set; }

    [JsonProperty("explorer_url")]
    public string? ExplorerUrl { get; set; }

    [JsonProperty("network")]
    public required string Network { get; set; }

    [JsonProperty("chain_id")]
    public required int ChainId { get; set; }
  }

  public class User
  {
    [JsonProperty("uid")]
    public required string Uid { get; set; }

    [JsonProperty("user_external_id")]
    public string? UserExternalId { get; set; }

    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("wallets")]
    public required List<UserWallet> Wallets { get; set; }
  }
}
