using OriginalSDK;

class Program
{
  static async Task Main(string[] args)
  {
    var root = Directory.GetCurrentDirectory();
    var dotenv = Path.Combine(root, ".env");
    DotNetEnv.Env.Load(dotenv);

    // Call your SDK methods here
    // var options = new OriginalOptions { Environment = OriginalSDK.Environment.Development, BaseUrl = BASE_URL };
    // var client = new OriginalClient(API_KEY, API_SECRET, options);
    var client = new OriginalClient();

    var response = await client.GetUserAsync<User>("971121805806");
    Console.WriteLine($"email {response.Data.Email}");
    Console.WriteLine($"uid {response.Data.Uid}");
    Console.WriteLine($"createdAt {response.Data.CreatedAt}");
    Console.WriteLine($"userExternalId {response.Data.UserExternalId}");
    Console.WriteLine($"Success", response.Success);
  }
}