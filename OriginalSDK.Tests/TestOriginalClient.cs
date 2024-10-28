using System.Net;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OriginalSDK.Entities;

namespace OriginalSDK.Tests
{
  public class TestOriginalClient
  {
    private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
    private readonly OriginalClient _client;

    public TestOriginalClient()
    {
      _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);

      var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
      {
        BaseAddress = new Uri("https://api.getoriginal.com/v1/")
      };

      _client = new OriginalClient("test_api_key", "thisisasupersecretapisecretthatcannotbeguessed");
      typeof(BaseClient).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
          ?.SetValue(_client, httpClient);
    }

    private void SetupMockResponse(string endpoint, string responseData, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      _httpMessageHandlerMock
          .Protected()
          .Setup<Task<HttpResponseMessage>>(
              "SendAsync",
              ItExpr.Is<HttpRequestMessage>(req =>
                  req.RequestUri == new Uri("https://api.getoriginal.com/v1/" + endpoint)),
              ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync(new HttpResponseMessage
          {
            StatusCode = statusCode,
            Content = new StringContent(responseData)
          });
    }

    [Fact]
    public async Task CreateUserAsync_ShouldReturnUidResponse_OnSuccess()
    {
      var userParams = new UserParams { Email = "test@example.com", UserExternalId = "external_123" };
      var expectedResponse = new ApiResponse<UidResponse> { Data = new UidResponse { Uid = "7890" }, Success = true };
      SetupMockResponse("user", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.Created);

      var result = await _client.CreateUserAsync(userParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("7890", result.Data.Uid);
    }

    [Fact]
    public async Task GetUserAsync_ShouldReturnUser_WhenUserExists()
    {
      var userUid = "123456";
      var expectedUser = new User
      {
        Uid = userUid,
        Email = "test@example.com",
        CreatedAt = DateTime.Now.ToString(),
        UserExternalId = "external_123",
        Wallets = new List<UserWallet>()
      };
      var expectedResponse = new ApiResponse<User> { Data = expectedUser, Success = true };
      SetupMockResponse($"user/{userUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetUserAsync(userUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(userUid, result.Data.Uid);
      Assert.Equal("test@example.com", result.Data.Email);
    }

    [Fact]
    public async Task GetUserByEmailAsync_ShouldReturnUser_WhenEmailExists()
    {
      var email = "email@email.com";
      var expectedResponse = new ApiResponse<User>
      {
        Data = new User
        {
          Email = email,
          Uid = "123",
          CreatedAt = DateTime.Now.ToString(),
          UserExternalId = "external_123",
          Wallets = new List<UserWallet>()
        },
        Success = true
      };
      SetupMockResponse($"user?email={email}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetUserByEmailAsync(email);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(email, result.Data.Email);
    }

    [Fact]
    public async Task GetUserByUserExternalIdAsync_ShouldReturnUser_WhenUserExternalIdExists()
    {
      var email = "email@email.com";

      var userExternalId =
          "external_123";
      var expectedResponse = new ApiResponse<User>
      {
        Data = new User
        {
          Email = email,
          Uid = "123",
          CreatedAt = DateTime.Now.ToString(),
          UserExternalId = "external_123",
          Wallets = new List<UserWallet>()
        },
        Success = true
      };
      SetupMockResponse($"user?user_external_id={userExternalId}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetUserByUserExternalIdAsync(userExternalId);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(userExternalId, result.Data.UserExternalId);
    }

    // TODO: Add more tests for other methods...
  }
}
