using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class Reward
  {
    [JsonProperty("contract_address")]
    public required string ContractAddress { get; set; }

    [JsonProperty("created_at")]
    public required string CreatedAt { get; set; }

    [JsonProperty("description")]
    public required string Description { get; set; }

    [JsonProperty("explorer_url")]
    public required string ExplorerUrl { get; set; }

    [JsonProperty("name")]
    public required string Name { get; set; }

    [JsonProperty("status")]
    public required string Status { get; set; }

    [JsonProperty("token_name")]
    public required string TokenName { get; set; }

    [JsonProperty("token_type")]
    public required string TokenType { get; set; }

    [JsonProperty("uid")]
    public required string Uid { get; set; }

    [JsonProperty("withdraw_receiver")]
    public required string WithdrawReceiver { get; set; }

    [JsonProperty("chain")]
    public required string Chain { get; set; }

    [JsonProperty("network")]
    public required string Network { get; set; }

    [JsonProperty("chain_id")]
    public int? ChainId { get; set; }
  }

}