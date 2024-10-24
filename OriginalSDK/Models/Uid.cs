using Newtonsoft.Json;

namespace OriginalSDK.Models
{
  public class UidResponse
  {
    [JsonProperty("uid")]
    public required string Uid { get; set; }
  }
}