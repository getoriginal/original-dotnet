using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class Reward
  {
    [JsonProperty("contract_address")]
    public string ContractAddress { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("explorer_url")]
    public string ExplorerUrl { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("token_name")]
    public string TokenName { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }

    [JsonProperty("withdraw_receiver")]
    public string WithdrawReceiver { get; set; }

    [JsonProperty("chain")]
    public string Chain { get; set; }

    [JsonProperty("network")]
    public string Network { get; set; }

    [JsonProperty("chain_id")]
    public int? ChainId { get; set; }
  }

}