using System.Net;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OriginalSDK.Entities;
using OriginalSDK.Tests.Helpers;


namespace OriginalSDK.Tests
{
  public class TestBaseClient : TestBase
  {
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;

    private readonly HttpClient _httpClient;

    public class TestableBaseClient : BaseClient
    {
      public TestableBaseClient(string? apiKey = null, string? apiSecret = null, OriginalOptions? options = null)
          : base(apiKey, apiSecret, options) { }

      new public Task<ApiResponse<T>> GetAsync<T>(string endpoint) => base.GetAsync<T>(endpoint);
      new public Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data) => base.PostAsync<T>(endpoint, data);
      new public Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data) => base.PutAsync<T>(endpoint, data);
      new public Task<ApiResponse<T>> PatchAsync<T>(string endpoint, object data) => base.PatchAsync<T>(endpoint, data);
      new public Task<ApiResponse<T>> DeleteAsync<T>(string endpoint) => base.DeleteAsync<T>(endpoint);
      new public string GetBaseUrl(OriginalOptions? options) => base.GetBaseUrl(options);
    }

    private readonly TestableBaseClient _client;

    public TestBaseClient()
    {
      _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
      _httpClient = new HttpClient(_httpMessageHandlerMock.Object)
      {
        BaseAddress = new Uri("https://api.getoriginal.com/v1/")
      };

      // Injecting dependencies to BaseClient
      _client = new TestableBaseClient("test_api_key", "thisisasupersecretapisecretthatcannotbeguessed");

      typeof(BaseClient).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          ?.SetValue(_client, _httpClient);
    }

    private void SetHttpClientResponse(string responseContent, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      _httpMessageHandlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
          )
          .ReturnsAsync(new HttpResponseMessage
          {
            StatusCode = statusCode,
            Content = new StringContent(responseContent)
          });
    }

    [Fact]
    public void ConstructorThrows_WhenKeysAreMissing()
    {
      Assert.Throws<ArgumentException>(() => new BaseClient(apiKey: null, apiSecret: null));
      Assert.Throws<ArgumentException>(() => new BaseClient(apiKey: "someKey", apiSecret: null));
      Assert.Throws<ArgumentException>(() => new BaseClient(apiKey: null, apiSecret: "someSecret"));
    }

    [Fact]
    public async Task GetAsync_ShouldReturnExpectedData()
    {
      var expectedResponse = new ApiResponse<string> { Data = "mocked data", Success = true };
      var responseContent = JsonConvert.SerializeObject(expectedResponse);
      SetHttpClientResponse(responseContent);

      var result = await _client.GetAsync<string>("test-endpoint");

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("mocked data", result.Data);
    }

    [Fact]
    public async Task PostAsync_SendsPostRequestWithCorrectBody()
    {
      var requestData = new { Name = "Created" };
      var expectedResponse = new ApiResponse<string> { Data = "created", Success = true };
      var responseContent = JsonConvert.SerializeObject(expectedResponse);
      SetHttpClientResponse(responseContent);

      var result = await _client.PostAsync<string>("test-endpoint", requestData);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("created", result.Data);
    }

    [Fact]
    public async Task SendRequestAsync_ThrowsException_OnNonSuccessStatusCode()
    {
      _httpMessageHandlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.IsAny<HttpRequestMessage>(),
              ItExpr.IsAny<CancellationToken>()
          )
          .ReturnsAsync(new HttpResponseMessage
          {
            StatusCode = HttpStatusCode.BadRequest,
            Content = new StringContent("Bad Request")
          });

      await Assert.ThrowsAsync<HttpRequestException>(() => _client.GetAsync<string>("test-endpoint"));
    }

    [Fact]
    public async Task PutAsync_SendsPutRequestWithCorrectBody()
    {
      var requestData = new { Field = "Updated" };
      var expectedResponse = new ApiResponse<string> { Data = "updated", Success = true };
      var responseContent = JsonConvert.SerializeObject(expectedResponse);
      SetHttpClientResponse(responseContent);

      var result = await _client.PutAsync<string>("test-endpoint", requestData);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("updated", result.Data);
    }

    [Fact]
    public async Task PatchAsync_SendsPatchRequestWithCorrectBody()
    {
      var requestData = new { Field = "Patched" };
      var expectedResponse = new ApiResponse<string> { Data = "patched", Success = true };
      var responseContent = JsonConvert.SerializeObject(expectedResponse);
      SetHttpClientResponse(responseContent);

      var result = await _client.PatchAsync<string>("test-endpoint", requestData);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("patched", result.Data);
    }

    [Fact]
    public async Task DeleteAsync_SendsDeleteRequestToCorrectUrl()
    {
      var expectedResponse = new ApiResponse<string> { Data = "deleted", Success = true };
      var responseContent = JsonConvert.SerializeObject(expectedResponse);
      SetHttpClientResponse(responseContent);

      var result = await _client.DeleteAsync<string>("test-endpoint");

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("deleted", result.Data);
    }

    [Fact]
    public void GetBaseUrl_ReturnsCorrectDevUrl()
    {
      var options = new OriginalOptions { Environment = OriginalEnvironment.Development };
      var result = _client.GetBaseUrl(options);

      Assert.Equal("https://api-dev.getoriginal.com/v1/", result);
    }

    [Fact]
    public void GetBaseUrl_ReturnsCorrectProdUrl()
    {
      var options = new OriginalOptions { Environment = OriginalEnvironment.Production };
      var result = _client.GetBaseUrl(options);

      Assert.Equal("https://api.getoriginal.com/v1/", result);
    }

    [Fact]
    public void GetBaseUrl_ReturnsCorrectCustomUrl()
    {
      var options = new OriginalOptions { BaseUrl = "https://custom-url.com/v1/" };
      var result = _client.GetBaseUrl(options);

      Assert.Equal("https://custom-url.com/v1/", result);
    }

    [Fact]
    public void GetBaseUrl_ReturnsCorrectEnvUrl()
    {
      Environment.SetEnvironmentVariable("ORIGINAL_BASE_URL", "https://env-url.com/v1/");
      var options = new OriginalOptions();
      var result = _client.GetBaseUrl(options);

      Assert.Equal("https://env-url.com/v1/", result);
      Environment.SetEnvironmentVariable("ORIGINAL_BASE_URL", null);
    }
  }
}
