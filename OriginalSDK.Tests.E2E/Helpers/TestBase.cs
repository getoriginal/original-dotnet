namespace OriginalSDK.Tests.E2E.Helpers
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
      var dotenv = Path.Combine(root, ".env.test.e2e");
      DotNetEnv.Env.TraversePath().Load(dotenv);
    }

    protected static string GetRandomString(int length = 8)
    {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      var random = new Random();
      return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
    }
  }
}

