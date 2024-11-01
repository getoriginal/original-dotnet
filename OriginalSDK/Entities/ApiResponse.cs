using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class ApiResponse<T>
  {
    [JsonProperty("success")]
    public bool Success { get; set; }

    [JsonProperty("data")]
    public T Data { get; set; }
  }
}