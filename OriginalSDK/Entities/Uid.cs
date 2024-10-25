using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class UidResponse
  {
    [JsonProperty("uid")]
    public required string Uid { get; set; }
  }
}