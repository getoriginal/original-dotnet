
using OriginalSDK.Entities;
using OriginalSDK.Tests.E2E.Helpers;

namespace OriginalSDK.Tests.E2E
{
  public class TestUser : TestBase
  {
    private readonly OriginalClient _client;
    private readonly string _testUserUid;
    private readonly string _testUserEmail;
    private readonly string _testUserUserExternalId;

    public TestUser()
    {
      _client = new OriginalClient();
      _testUserUid = Environment.GetEnvironmentVariable("TEST_APP_USER_UID") ?? throw new InvalidOperationException("TEST_APP_USER_UID is not set");
      _testUserEmail = Environment.GetEnvironmentVariable("TEST_APP_USER_EMAIL") ?? throw new InvalidOperationException("TEST_APP_USER_EMAIL is not set");
      _testUserUserExternalId = Environment.GetEnvironmentVariable("TEST_APP_USER_USER_EXTERNAL_ID") ?? throw new InvalidOperationException("TEST_APP_USER_USER_EXTERNAL_ID is not set");
    }

    [Fact]
    public async Task CreateUser_WithParams_ReturnsUid()
    {
      var userExternalId = GetRandomString();
      var response = await _client.CreateUserAsync(new UserParams
      {
        Email = $"{userExternalId}@test.com",
        UserExternalId = userExternalId
      });
      Assert.NotNull(response.Data.Uid);
    }

    [Fact]
    public async Task CreateUser_WithNoParams_ReturnsUid()
    {
      var response = await _client.CreateUserAsync(new UserParams());
      Assert.NotNull(response.Data.Uid);
    }

    [Fact]
    public async Task GetUser_ReturnsCorrectUser()
    {
      var response = await _client.GetUserAsync(_testUserUid);
      Assert.Equal(_testUserUid, response.Data.Uid);
      Assert.Equal(_testUserEmail, response.Data.Email);
    }

    [Fact]
    public async Task GetUserByEmail_ReturnsCorrectUser()
    {
      var response = await _client.GetUserByEmailAsync(_testUserEmail);
      Assert.Equal(_testUserUid, response.Data.Uid);
      Assert.Equal(_testUserEmail, response.Data.Email);
    }

    [Fact]
    public async Task GetUserByUserExternalId_ReturnsCorrectUser()
    {
      var response = await _client.GetUserByUserExternalIdAsync(_testUserUserExternalId);
      Assert.Equal(_testUserUid, response.Data.Uid);
      Assert.Equal(_testUserEmail, response.Data.Email);
    }

    [Fact]
    public async Task GetUser_NotFound_Throws404()
    {
      var exception = await Assert.ThrowsAsync<ClientException>(async () => await _client.GetUserAsync("not_found"));
      Assert.Equal(404, exception.Status);
    }
  }
}