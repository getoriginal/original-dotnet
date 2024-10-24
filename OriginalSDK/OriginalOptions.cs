namespace OriginalSDK
{
  public class OriginalOptions
  {
    public Environment Environment { get; set; } = Environment.Production;
    public string? BaseUrl { get; set; }
  }

  public enum Environment
  {
    Development,
    Production
  }
}
