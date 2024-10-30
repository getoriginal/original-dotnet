using System.Net;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using OriginalSDK.Entities;
using OriginalSDK.Tests.Unit.Helpers;

namespace OriginalSDK.Tests.Unit
{
  public class TestOriginalClient : TestBase
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
    public async Task ClientException_IsThrownForClientError()
    {
      var originalErrorData = new OriginalErrorData
      {
        Error = new Error
        {
          Type = OriginalErrorCode.ClientError,
          Detail = new ErrorDetail
          {
            Message = "A client error occurred",
            Code = "error_code",
            FieldName = null
          }
        }
      };

      string errorJson = JsonConvert.SerializeObject(originalErrorData);

      var userUid = "nonexistent_uid";
      SetupMockResponse($"user/{userUid}", errorJson, HttpStatusCode.BadRequest);

      await Assert.ThrowsAsync<ClientException>(() => _client.GetUserAsync(userUid));
    }

    [Fact]
    public async Task ServerException_IsThrownForServerError()
    {
      var originalErrorData = new OriginalErrorData
      {
        Error = new Error
        {
          Type = OriginalErrorCode.ServerError,
          Detail = new ErrorDetail
          {
            Message = "A server error occurred",
            Code = "error_code",
            FieldName = null
          }
        }
      };

      string errorJson = JsonConvert.SerializeObject(originalErrorData);

      var userUid = "nonexistent_uid";
      SetupMockResponse($"user/{userUid}", errorJson, HttpStatusCode.BadRequest);

      await Assert.ThrowsAsync<ServerException>(() => _client.GetUserAsync(userUid));
    }

    [Fact]
    public async Task ValidationException_IsThrownForValidationError()
    {
      var originalErrorData = new OriginalErrorData
      {
        Error = new Error
        {
          Type = OriginalErrorCode.ValidationError,
          Detail = new ErrorDetail
          {
            Message = "A validation error occurred",
            Code = "error_code",
            FieldName = null
          }
        }
      };

      string errorJson = JsonConvert.SerializeObject(originalErrorData);

      var userUid = "nonexistent_uid";
      SetupMockResponse($"user/{userUid}", errorJson, HttpStatusCode.BadRequest);

      await Assert.ThrowsAsync<ValidationException>(() => _client.GetUserAsync(userUid));
    }

    [Fact]
    public async Task HttpRequestException_IsThrownForForbidden()
    {
      var userUid = "nonexistent_uid";
      SetupMockResponse($"user/{userUid}", "{}", HttpStatusCode.Forbidden);

      await Assert.ThrowsAsync<ClientException>(() => _client.GetUserAsync(userUid));
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

    [Fact]
    public async Task GetCollectionAsync_ShouldReturnCollection_OnSuccess()
    {
      var collectionUid = "940830618348";
      var expectedCollection = new Collection
      {
        Uid = collectionUid,
        Name = "Test Collection",
        Status = "deployed",
        Type = "ERC721",
        CreatedAt = DateTime.Now.ToString(),
        Chain = "Amoy",
        ChainId = 80002,
        Network = "Polygon",
      };
      var expectedResponse = new ApiResponse<Collection> { Data = expectedCollection, Success = true };
      SetupMockResponse($"collection/{collectionUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetCollectionAsync(collectionUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(collectionUid, result.Data.Uid);
      Assert.Equal("Test Collection", result.Data.Name);
    }

    [Fact]
    public async Task CreateAssetAsync_ShouldReturnUidResponse_OnSuccess()
    {
      var assetParams = new AssetParams
      {
        UserUid = "147980339890",
        CollectionUid = "940830618348",
        AssetExternalId = "asset_external_id",
        SalePriceInUsd = 1.0,
        Data = new AssetData
        {
          Name = "Test Asset",
          ImageUrl = "https://example.com/image.jpg",
        }
      };
      var expectedResponse = new ApiResponse<UidResponse> { Data = new UidResponse { Uid = "new_asset_uid" }, Success = true };
      SetupMockResponse("asset", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.Created);

      var result = await _client.CreateAssetAsync(assetParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("new_asset_uid", result.Data.Uid);
    }

    [Fact]
    public async Task GetAssetAsync_ShouldReturnAsset_OnSuccess()
    {
      var assetUid = "373337353781";
      var expectedAsset = new Asset
      {
        Uid = assetUid,
        Name = "Test Asset",
        AssetExternalId = "asset_external_id",
        CollectionName = "Test Collection",
        CollectionUid = "940830618348",
        CreatedAt = DateTime.Now.ToString(),
        MintForUserUid = "147980339890",
        MintForAddress = "recipient_address",
        TokenId = "1",
        Metadata = new AssetMetadata
        {
          Name = "Test Asset",
          Image = "https://example.com/image.jpg",
          OrgImageUrl = "https://example.com/image.jpg",
          OriginalId = "1",
        }
      };

      var expectedResponse = new ApiResponse<Asset> { Data = expectedAsset, Success = true };
      SetupMockResponse($"asset/{assetUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetAssetAsync(assetUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(assetUid, result.Data.Uid);
      Assert.Equal("Test Asset", result.Data.Name);
    }

    [Fact]
    public async Task GetAssetsByUserUidAsync_ShouldReturnAssets_OnSuccess()
    {
      var userUid = "147980339890";
      var expectedAsset = new Asset
      {
        Uid = "asset1",
        Name = "Test Asset",
        AssetExternalId = "asset_external_id",
        CollectionName = "Test Collection",
        CollectionUid = "940830618348",
        CreatedAt = DateTime.Now.ToString(),
        MintForUserUid = "147980339890",
        MintForAddress = "recipient_address",
        TokenId = "1",
        Metadata = new AssetMetadata
        {
          Name = "Test Asset",
          Image = "https://example.com/image.jpg",
          OrgImageUrl = "https://example.com/image.jpg",
          OriginalId = "1",
        }
      };
      var expectedAssets = new List<Asset>
      {
        expectedAsset,
        expectedAsset
      };
      var expectedResponse = new ApiResponse<List<Asset>> { Data = expectedAssets, Success = true };
      SetupMockResponse($"asset?user_uid={userUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetAssetsByUserUidAsync(userUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task EditAssetAsync_ShouldReturnSuccess_OnSuccess()
    {
      var editParams = new EditAssetParams
      {
        Data = new EditAssetData { Name = "Updated Asset Name", ImageUrl = "https://example.com/updated.jpg" }
      };
      var assetUid = "373337353781";
      var expectedResponse = new ApiResponse<object> { Data = null!, Success = true };
      SetupMockResponse($"asset/{assetUid}", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.OK);

      var result = await _client.EditAssetAsync(assetUid, editParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
    }

    [Fact]
    public async Task CreateTransferAsync_ShouldReturnUidResponse_OnSuccess()
    {
      var transferParams = new TransferParams
      {
        AssetUid = "373337353781",
        FromUserUid = "147980339890",
        ToAddress = "recipient_address"
      };
      var expectedResponse = new ApiResponse<UidResponse> { Data = new UidResponse { Uid = "new_transfer_uid" }, Success = true };
      SetupMockResponse("transfer", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.Created);

      var result = await _client.CreateTransferAsync(transferParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("new_transfer_uid", result.Data.Uid);
    }

    [Fact]
    public async Task GetTransferAsync_ShouldReturnTransfer_OnSuccess()
    {
      var transferUid = "885359965902";
      var expectedTransfer = new Transfer
      {
        Uid = transferUid,
        Status = "done",
        AssetUid = "373337353781",
        CreatedAt = DateTime.Now.ToString(),
        FromUserUid = "147980339890",
        ToAddress = "recipient_address"
      };
      var expectedResponse = new ApiResponse<Transfer> { Data = expectedTransfer, Success = true };
      SetupMockResponse($"transfer/{transferUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetTransferAsync(transferUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(transferUid, result.Data.Uid);
      Assert.Equal("done", result.Data.Status);
    }

    [Fact]
    public async Task CreateBurnAsync_ShouldReturnUidResponse_OnSuccess()
    {
      var burnParams = new BurnParams
      {
        AssetUid = "373337353781",
        FromUserUid = "147980339890"
      };
      var expectedResponse = new ApiResponse<UidResponse> { Data = new UidResponse { Uid = "new_burn_uid" }, Success = true };
      SetupMockResponse("burn", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.Created);

      var result = await _client.CreateBurnAsync(burnParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("new_burn_uid", result.Data.Uid);
    }

    [Fact]
    public async Task GetBurnAsync_ShouldReturnBurn_OnSuccess()
    {
      var burnUid = "662859149573";
      var expectedBurn = new Burn
      {
        Uid = burnUid,
        Status = "done",
        AssetUid = "373337353781",
        CreatedAt = DateTime.Now.ToString(),
        FromUserUid = "147980339890"
      };
      var expectedResponse = new ApiResponse<Burn> { Data = expectedBurn, Success = true };
      SetupMockResponse($"burn/{burnUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetBurnAsync(burnUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(burnUid, result.Data.Uid);
      Assert.Equal("done", result.Data.Status);
    }

    [Fact]
    public async Task GetDepositAsync_ShouldReturnDeposit_OnSuccess()
    {
      var userUid = "147980339890";
      var collectionUid = "940830618348";
      var expectedDeposit = new Deposit
      {
        WalletAddress = "wallet_address",
        Network = "Polygon",
        QrCodeData = "qr_code_data",
      };
      var expectedResponse = new ApiResponse<Deposit> { Data = expectedDeposit, Success = true };
      SetupMockResponse($"deposit?user_uid={userUid}&collection_uid={collectionUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetDepositAsync(userUid, collectionUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("wallet_address", result.Data.WalletAddress);
    }

    [Fact]
    public async Task GetRewardAsync_ShouldReturnReward_OnSuccess()
    {
      var rewardUid = "501676762240";
      var expectedReward = new Reward
      {
        Uid = rewardUid,
        Name = "Reward Name",
        CreatedAt = DateTime.Now.ToString(),
        ContractAddress = "0x1234567890",
        Description = "Reward Description",
        ExplorerUrl = "https://example.com/explorer",
        Status = "deployed",
        TokenName = "Token Name",
        TokenType = "ERC20",
        WithdrawReceiver = "0x1234567890",
        Chain = "Amoy",
        Network = "Polygon",
      };
      var expectedResponse = new ApiResponse<Reward> { Data = expectedReward, Success = true };
      SetupMockResponse($"reward/{rewardUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetRewardAsync(rewardUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(rewardUid, result.Data.Uid);
      Assert.Equal("Reward Name", result.Data.Name);
    }

    [Fact]
    public async Task CreateAllocationAsync_ShouldReturnUidResponse_OnSuccess()
    {
      var allocationParams = new AllocationParams
      {
        Amount = 0.1,
        Nonce = "unique_nonce",
        RewardUid = "reward_uid_123",
        ToUserUid = "user_uid_456"
      };
      var expectedResponse = new ApiResponse<UidResponse> { Data = new UidResponse { Uid = "new_allocation_uid" }, Success = true };
      SetupMockResponse("reward/allocate", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.Created);

      var result = await _client.CreateAllocationAsync(allocationParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("new_allocation_uid", result.Data.Uid);
    }

    [Fact]
    public async Task GetAllocationAsync_ShouldReturnAllocation_OnSuccess()
    {
      var allocationUid = "allocation_uid_123";
      var expectedAllocation = new Allocation
      {
        Uid = allocationUid,
        Amount = 0.1,
        RewardUid = "reward_uid_123",
        ToUserUid = "user_uid_456",
        CreatedAt = DateTime.Now.ToString(),
        Status = "allocated",
        Nonce = "unique_nonce"
      };
      var expectedResponse = new ApiResponse<Allocation> { Data = expectedAllocation, Success = true };
      SetupMockResponse($"reward/allocate/{allocationUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetAllocationAsync(allocationUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(allocationUid, result.Data.Uid);
      Assert.Equal("allocated", result.Data.Status);
    }

    [Fact]
    public async Task GetAllocationsByUserUidAsync_ShouldReturnAllocations_OnSuccess()
    {
      var userUid = "user_uid_456";
      var expectedAllocations = new List<Allocation>
    {
        new Allocation
        {
            Uid = "allocation_uid_1",
            Amount = 0.1,
            RewardUid = "reward_uid_123",
            ToUserUid = userUid,
            CreatedAt = DateTime.Now.ToString(),
            Status = "allocated",
            Nonce = "unique_nonce_1"
        },
        new Allocation
        {
            Uid = "allocation_uid_2",
            Amount = 0.2,
            RewardUid = "reward_uid_456",
            ToUserUid = userUid,
            CreatedAt = DateTime.Now.ToString(),
            Status = "allocated",
            Nonce = "unique_nonce_2"
        }
    };
      var expectedResponse = new ApiResponse<List<Allocation>> { Data = expectedAllocations, Success = true };
      SetupMockResponse($"reward/allocate?user_uid={userUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetAllocationsByUserUidAsync(userUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(2, result.Data.Count);
    }

    [Fact]
    public async Task CreateClaimAsync_ShouldReturnUidResponse_OnSuccess()
    {
      var claimParams = new ClaimParams
      {
        FromUserUid = "from_user_uid_123",
        RewardUid = "reward_uid_123",
        ToAddress = "0x1234567890abcdef"
      };
      var expectedResponse = new ApiResponse<UidResponse> { Data = new UidResponse { Uid = "new_claim_uid" }, Success = true };
      SetupMockResponse("reward/claim", JsonConvert.SerializeObject(expectedResponse), HttpStatusCode.Created);

      var result = await _client.CreateClaimAsync(claimParams);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal("new_claim_uid", result.Data.Uid);
    }

    [Fact]
    public async Task GetClaimAsync_ShouldReturnClaim_OnSuccess()
    {
      var claimUid = "claim_uid_123";
      var expectedClaim = new Claim
      {
        Uid = claimUid,
        Amount = 50.0,
        RewardUid = "reward_uid_123",
        FromUserUid = "from_user_uid_123",
        ToAddress = "0x1234567890abcdef",
        CreatedAt = DateTime.Now.ToString(),
        Status = "claimed"
      };
      var expectedResponse = new ApiResponse<Claim> { Data = expectedClaim, Success = true };
      SetupMockResponse($"reward/claim/{claimUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetClaimAsync(claimUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(claimUid, result.Data.Uid);
      Assert.Equal("claimed", result.Data.Status);
    }

    [Fact]
    public async Task GetClaimsByUserUidAsync_ShouldReturnClaims_OnSuccess()
    {
      var userUid = "user_uid_456";
      var expectedClaims = new List<Claim>
    {
        new Claim
        {
            Uid = "claim_uid_1",
            Amount = 50.0,
            RewardUid = "reward_uid_123",
            FromUserUid = userUid,
            ToAddress = "0x1234567890abcdef",
            CreatedAt = DateTime.Now.ToString(),
            Status = "claimed"
        },
        new Claim
        {
            Uid = "claim_uid_2",
            Amount = 75.0,
            RewardUid = "reward_uid_456",
            FromUserUid = userUid,
            ToAddress = "0xabcdef1234567890",
            CreatedAt = DateTime.Now.ToString(),
            Status = "claimed"
        }
    };
      var expectedResponse = new ApiResponse<List<Claim>> { Data = expectedClaims, Success = true };
      SetupMockResponse($"reward/claim?user_uid={userUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetClaimsByUserUidAsync(userUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(2, result.Data.Count);
    }


    [Fact]
    public async Task GetBalanceAsync_ShouldReturnBalance_OnSuccess()
    {
      var rewardUid = "501676762240";
      var userUid = "654716880029";
      var expectedBalance = new Balance
      {
        RewardUid = rewardUid,
        UserUid = userUid,
        Amount = 100.0
      };
      var expectedResponse = new ApiResponse<Balance> { Data = expectedBalance, Success = true };
      SetupMockResponse($"reward/balance?reward_uid={rewardUid}&user_uid={userUid}", JsonConvert.SerializeObject(expectedResponse));

      var result = await _client.GetBalanceAsync(rewardUid, userUid);

      Assert.NotNull(result);
      Assert.True(result.Success);
      Assert.Equal(rewardUid, result.Data.RewardUid);
      Assert.Equal(userUid, result.Data.UserUid);
      Assert.Equal(100.0, result.Data.Amount);
    }
  }
}
