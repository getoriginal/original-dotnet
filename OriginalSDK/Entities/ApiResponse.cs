using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class ApiResponse<T>
  {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("data")]
    public required T Data { get; set; }
  }
}