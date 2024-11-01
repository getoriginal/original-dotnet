using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class Collection
  {
    [JsonProperty("uid")]
    public string Uid { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("editable_assets")]
    public bool EditableAssets { get; set; }

    [JsonProperty("contract_address")]
    public string ContractAddress { get; set; }

    [JsonProperty("symbol")]
    public string Symbol { get; set; }

    [JsonProperty("external_url")]
    public string ExternalUrl { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("explorer_url")]
    public string ExplorerUrl { get; set; }

    [JsonProperty("chain")]
    public string Chain { get; set; }

    [JsonProperty("network")]
    public string Network { get; set; }

    [JsonProperty("chain_id")]
    public int? ChainId { get; set; }
  }
}
