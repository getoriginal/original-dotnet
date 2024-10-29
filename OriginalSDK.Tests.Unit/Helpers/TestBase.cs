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
      var root = Directory.GetCurrentDirectory();
      var dotenv = Path.Combine(root, ".env.test");
      DotNetEnv.Env.TraversePath().Load(dotenv);
    }
  }
}

