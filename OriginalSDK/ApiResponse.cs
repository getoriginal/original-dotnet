namespace OriginalSDK
{
  public class ApiResponse<T>
  {
    public bool Success { get; set; }

    public T Data { get; set; }
  }
}