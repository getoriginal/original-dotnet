namespace OriginalSDK.Tests.Unit.Helpers
{
  public abstract class TestBase
  {
    static TestBase()
    {
      LoadEnvironmentVariables();
    }

    private static void LoadEnvironmentVariables()
    {
      Environment.SetEnvironmentVariable("ORIGINAL_API_KEY", null);
      Environment.SetEnvironmentVariable("ORIGINAL_API_SECRET", null);
      Environment.SetEnvironmentVariable("ORIGINAL_BASE_URL", null);
      Environment.SetEnvironmentVariable("ORIGINAL_ENVIRONMENT", null);

      var root = Directory.GetCurrentDirectory();
      var dotenv = Path.Combine(root, ".env.test.unit");
      DotNetEnv.Env.TraversePath().Load(dotenv);
    }
  }
}

