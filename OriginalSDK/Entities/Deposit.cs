using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class Deposit
  {
    [JsonProperty("chain_id")]
    public int ChainId { get; set; }

    [JsonProperty("network")]
    public required string Network { get; set; }

    [JsonProperty("qr_code_data")]
    public required string QrCodeData { get; set; }

    [JsonProperty("wallet_address")]
    public required string WalletAddress { get; set; }
  }
}