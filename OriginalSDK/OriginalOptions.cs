namespace OriginalSDK
{
  public enum OriginalEnvironment
  {
    Development,
    Production
  }

  public class OriginalOptions
  {
    public OriginalEnvironment Environment { get; set; } = OriginalEnvironment.Production;
    public string? BaseUrl { get; set; }
  }
}
