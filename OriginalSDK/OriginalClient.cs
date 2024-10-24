using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace OriginalSDK
{
  public class OriginalClient
  {
    private readonly HttpClient _httpClient;
    private readonly TokenManager _tokenManager;

    private readonly JsonSerializerSettings _jsonSettings = new()
    {
      ContractResolver = new DefaultContractResolver
      {
        NamingStrategy = new SnakeCaseNamingStrategy()
      },
    };

    private const string DEVELOPMENT_URL = "https://api-dev.getoriginal.com/v1/";
    private const string PRODUCTION_URL = "https://api.getoriginal.com/v1/";

    public OriginalClient(string? apiKey = null, string? apiSecret = null, OriginalOptions? options = null)
    {
      apiKey ??= System.Environment.GetEnvironmentVariable("ORIGINAL_API_KEY");
      apiSecret ??= System.Environment.GetEnvironmentVariable("ORIGINAL_API_SECRET");

      if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
      {
        throw new ArgumentException("API key and secret are required.");
      }
      _tokenManager = new TokenManager(apiSecret);

      _httpClient = new HttpClient
      {
        BaseAddress = new Uri(GetBaseUrl(options)),
      };

      _httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    private static string GetBaseUrl(OriginalOptions? options)
    {
      string baseUrl;
      if (options?.BaseUrl != null)
      {
        baseUrl = options.BaseUrl;
      }
      else if (!string.IsNullOrEmpty(System.Environment.GetEnvironmentVariable("ORIGINAL_BASE_URL")))
      {
        baseUrl = System.Environment.GetEnvironmentVariable("ORIGINAL_BASE_URL")!;
      }
      else
      {
        baseUrl = options?.Environment == Environment.Development ? DEVELOPMENT_URL : PRODUCTION_URL;
      }
      return baseUrl;
    }

    private async Task<ApiResponse<T>> SendRequestAsync<T>(Func<Task<HttpResponseMessage>> requestFunc)
    {
      var token = _tokenManager.GenerateToken();
      _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

      var response = await requestFunc();
      response.EnsureSuccessStatusCode();

      var responseData = await response.Content.ReadAsStringAsync();
      var result = JsonConvert.DeserializeObject<ApiResponse<T>>(responseData, _jsonSettings);
      if (result == null)
      {
        throw new InvalidOperationException("Deserialization failed, received null.");
      }

      return result;
    }

    private StringContent PrepareContent(object data)
    {
      var jsonData = JsonConvert.SerializeObject(data, _jsonSettings);
      return new StringContent(jsonData, Encoding.UTF8, "application/json");
    }

    private Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
      return SendRequestAsync<T>(() => _httpClient.GetAsync(endpoint));
    }

    private Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
    {
      var content = PrepareContent(data);
      return SendRequestAsync<T>(() => _httpClient.PostAsync(endpoint, content));
    }

    private Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
    {
      var content = PrepareContent(data);
      return SendRequestAsync<T>(() => _httpClient.PutAsync(endpoint, content));
    }

    private Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object data)
    {
      var content = PrepareContent(data);
      var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) { Content = content };
      return SendRequestAsync<T>(() => _httpClient.SendAsync(request));
    }

    private Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
    {
      return SendRequestAsync<T>(() => _httpClient.DeleteAsync(endpoint));
    }

    public Task<ApiResponse<T>> GetUserAsync<T>(string userUid)
    {
      return GetAsync<T>($"user/{userUid}");
    }
  }
}
