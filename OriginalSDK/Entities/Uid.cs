using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class UidResponse
  {
    [JsonProperty("uid")]
    public string Uid { get; set; }
  }
}