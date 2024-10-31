namespace OriginalSDK
{
  public enum OriginalEnvironment
  {
    Development = 0,
    Production = 1
  }

  public static class OriginalConstants
  {
    public const string DEVELOPMENT_ENVIRONMENT = "development";
    public const string PRODUCTION_ENVIRONMENT = "production";
  }

  public class OriginalOptions
  {
    public OriginalEnvironment Environment { get; set; } = OriginalEnvironment.Production;
    public string? BaseUrl { get; set; }
  }
}
