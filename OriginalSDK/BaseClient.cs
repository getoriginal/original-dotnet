using System.Text;
using Newtonsoft.Json;
using OriginalSDK.Entities;

namespace OriginalSDK
{
  public class BaseClient
  {
    private readonly HttpClient _httpClient;
    private readonly TokenManager _tokenManager;

    private const string DEVELOPMENT_URL = "https://api-dev.getoriginal.com/v1/";
    private const string PRODUCTION_URL = "https://api.getoriginal.com/v1/";

    public BaseClient(string? apiKey = null, string? apiSecret = null, OriginalOptions? options = null)
    {
      apiKey ??= Environment.GetEnvironmentVariable("ORIGINAL_API_KEY");
      apiSecret ??= Environment.GetEnvironmentVariable("ORIGINAL_API_SECRET");

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

    protected string GetBaseUrl(OriginalOptions? options)
    {
      string baseUrl;
      if (options?.BaseUrl != null)
      {
        baseUrl = options.BaseUrl;
      }
      else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ORIGINAL_BASE_URL")))
      {
        baseUrl = Environment.GetEnvironmentVariable("ORIGINAL_BASE_URL")!;
      }
      else if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ORIGINAL_ENVIRONMENT")))
      {
        baseUrl = Environment.GetEnvironmentVariable("ORIGINAL_ENVIRONMENT") == OriginalConstants.DEVELOPMENT_ENVIRONMENT ? DEVELOPMENT_URL : PRODUCTION_URL;
      }
      else
      {
        baseUrl = options?.Environment == OriginalEnvironment.Development ? DEVELOPMENT_URL : PRODUCTION_URL;
      }
      return baseUrl;
    }

    private async Task<ApiResponse<T>> SendRequestAsync<T>(Func<Task<HttpResponseMessage>> requestFunc)
    {
      var token = _tokenManager.GenerateToken();
      _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

      var response = await requestFunc();
      var responseData = await response.Content.ReadAsStringAsync();

      if (!response.IsSuccessStatusCode)
      {
        var errorData = null as OriginalErrorData;
        var errorMessage = "Unknown error occurred";
        try
        {
          errorData = JsonConvert.DeserializeObject<OriginalErrorData>(responseData);
          errorMessage = errorData?.Error?.GetMessage() ?? "Unknown error occurred";
        }
        catch (JsonReaderException)
        {
          Console.WriteLine("Failed to parse original error data.");
        }
        ErrorUtils.ParseAndRaiseError(errorData, errorMessage, (int)response.StatusCode);
      }

      var result = JsonConvert.DeserializeObject<ApiResponse<T>>(responseData);
      if (result == null)
      {
        throw new InvalidOperationException("Deserialization failed, received null.");
      }

      return result;
    }

    private static StringContent PrepareContent(object data)
    {
      var jsonData = JsonConvert.SerializeObject(data);
      return new StringContent(jsonData, Encoding.UTF8, "application/json");
    }

    protected Task<ApiResponse<T>> GetAsync<T>(string endpoint)
    {
      return SendRequestAsync<T>(() => _httpClient.GetAsync(endpoint));
    }

    protected Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
    {
      var content = PrepareContent(data);
      return SendRequestAsync<T>(() => _httpClient.PostAsync(endpoint, content));
    }

    protected Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
    {
      var content = PrepareContent(data);
      return SendRequestAsync<T>(() => _httpClient.PutAsync(endpoint, content));
    }

    protected Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object data)
    {
      var content = PrepareContent(data);
      var request = new HttpRequestMessage(new HttpMethod("PATCH"), endpoint) { Content = content };
      return SendRequestAsync<T>(() => _httpClient.SendAsync(request));
    }

    protected Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
    {
      return SendRequestAsync<T>(() => _httpClient.DeleteAsync(endpoint));
    }
  }
}
