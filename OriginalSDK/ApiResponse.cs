namespace OriginalSDK
{
  public class ApiResponse<T>
  {
    public bool Success { get; set; }

    public required T Data { get; set; }
  }
}