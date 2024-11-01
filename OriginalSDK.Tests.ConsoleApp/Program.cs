using OriginalSDK.Entities;

namespace OriginalSDK.Tests.ConsoleApp
{
  class Program
  {
    static async Task Main(string[] args)
    {
      LoadEnvironmentVariables();

      // Utilise environment variables
      OriginalClient client = new OriginalClient();

      try
      {
        // Uncomment the methods you want to test

        // User
        // await TestCreateUserAsync(client);
        // await TestGetUserAsync(client);
        // await TestGetUserByEmailAsync(client);
        // await TestGetUserByUserExternalIdAsync(client);

        // Collection
        await TestGetCollectionAsync(client);

        // // Asset
        // await TestCreateAssetAsync(client);
        // await TestGetAssetAsync(client);
        // await TestGetAssetsByUserUidAsync(client);

        // // Edit
        // await TestEditAssetAsync(client);

        // // Transfer
        // await TestCreateTransferAsync(client);
        // await TestGetTransferAsync(client);
        // await TestGetTransfersByUserUidAsync(client);

        // // Burn
        // await TestCreateBurnAsync(client);
        // await TestGetBurnAsync(client);
        // await TestGetBurnsByUserUidAsync(client);

        // // Deposit
        // await TestGetDepositAsync(client);

        // // Reward
        // await TestGetRewardAsync(client);

        // // Allocation
        // await TestCreateAllocationAsync(client);
        // await TestGetAllocationAsync(client);
        // await TestGetAllocationsByUserUidAsync(client);

        // // Claim
        // await TestCreateClaimAsync(client);
        // await TestGetClaimAsync(client);
        // await TestGetClaimsByUserUidAsync(client);

        // // Balance
        // await TestGetBalanceAsync(client);
      }
      catch (ClientException clientException)
      {
        Console.WriteLine(clientException.Message);
      }
      catch (ServerException serverException)
      {
        Console.WriteLine(serverException.Message);
      }
      catch (ValidationException validationException)
      {
        Console.WriteLine(validationException.Message);
      }
      catch (Exception exception)
      {
        Console.WriteLine(exception.Message);
      }
    }

    private static void LoadEnvironmentVariables()
    {
      var root = Directory.GetCurrentDirectory();
      var dotenv = Path.Combine(root, ".env");
      DotNetEnv.Env.TraversePath().Load(dotenv);
    }

    static async Task TestCreateUserAsync(OriginalClient client)
    {
      UserParams userParams = new UserParams
      {
        Email = "newuser1@example.com",
        UserExternalId = "user_external_id_1"
      };
      ApiResponse<UidResponse> response = await client.CreateUserAsync(userParams);
      Console.WriteLine("CreateUserAsync:");
      Console.WriteLine($"New User UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetUserAsync(OriginalClient client)
    {
      var userUid = "147980339890";
      ApiResponse<User> response = await client.GetUserAsync(userUid);
      Console.WriteLine("GetUserAsync:");
      Console.WriteLine($"Email: {response.Data.Email}");
      Console.WriteLine($"UID: {response.Data.Uid}");
      Console.WriteLine($"CreatedAt: {response.Data.CreatedAt}");
      Console.WriteLine($"UserExternalId: {response.Data.UserExternalId}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetUserByEmailAsync(OriginalClient client)
    {
      var email = "newuser1@example.com";
      ApiResponse<User> response = await client.GetUserByEmailAsync(email);
      Console.WriteLine("GetUserByEmailAsync:");
      Console.WriteLine($"Email: {response.Data?.Email}");
      Console.WriteLine($"UID: {response.Data?.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetUserByUserExternalIdAsync(OriginalClient client)
    {
      var userExternalId = "user_external_id_1";
      ApiResponse<User> response = await client.GetUserByUserExternalIdAsync(userExternalId);
      Console.WriteLine("GetUserByUserExternalIdAsync:");
      Console.WriteLine($"Email: {response.Data?.Email}");
      Console.WriteLine($"UID: {response.Data?.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetCollectionAsync(OriginalClient client)
    {
      var collectionUid = "940830618348";
      ApiResponse<Collection> response = await client.GetCollectionAsync(collectionUid);
      Console.WriteLine("GetCollectionAsync:");
      Console.WriteLine($"Chain: {response.Data.Chain}");
      Console.WriteLine($"Chain ID: {response.Data.ChainId}");
      Console.WriteLine($"Contract Address: {response.Data.ContractAddress}");
      Console.WriteLine($"Created At: {response.Data.CreatedAt}");
      Console.WriteLine($"Description: {response.Data.Description}");
      Console.WriteLine($"Editable Assets: {response.Data.EditableAssets}");
      Console.WriteLine($"Explorer URL: {response.Data.ExplorerUrl}");
      Console.WriteLine($"External URL: {response.Data.ExternalUrl}");
      Console.WriteLine($"Name: {response.Data.Name}");
      Console.WriteLine($"Network: {response.Data.Network}");
      Console.WriteLine($"Status: {response.Data.Status}");
      Console.WriteLine($"Symbol: {response.Data.Symbol}");
      Console.WriteLine($"Type: {response.Data.Type}");
      Console.WriteLine($"UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestCreateAssetAsync(OriginalClient client)
    {
      AssetParams assetParams = new AssetParams
      {
        UserUid = "147980339890",
        CollectionUid = "940830618348",
        AssetExternalId = "asset_external_id",
        SalePriceInUsd = 1.0,
        Data = new AssetData
        {
          Name = "Asset Name",
          Description = "Asset Description",
          ExternalUrl = "https://example.com",
          ImageUrl = "https://fastly.picsum.photos/id/912/128/128.jpg?hmac=KjVE4IURO5BwnZLpAswjhNhSJpiMxdLmO1cLbBgdH84",
          Attributes = new List<AssetAttribute>
                {
                    new AssetAttribute { TraitType = "Trait", Value = "Value", DisplayType = "Display Type" }
                }
        }
      };
      ApiResponse<UidResponse> response = await client.CreateAssetAsync(assetParams);
      Console.WriteLine("CreateAssetAsync:");
      Console.WriteLine($"New Asset UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetAssetAsync(OriginalClient client)
    {
      var assetUid = "373337353781";
      ApiResponse<Asset> response = await client.GetAssetAsync(assetUid);
      Console.WriteLine("GetAssetAsync:");
      Console.WriteLine($"AssetExternalId: {response.Data.AssetExternalId}");
      Console.WriteLine($"Collection Name: {response.Data.CollectionName}");
      Console.WriteLine($"Collection UID: {response.Data.CollectionUid}");
      Console.WriteLine($"Created At: {response.Data.CreatedAt}");
      Console.WriteLine($"Explorer URL: {response.Data.ExplorerUrl}");
      Console.WriteLine($"Is Burned: {response.Data.IsBurned}");
      Console.WriteLine($"Is Editing: {response.Data.IsEditing}");
      Console.WriteLine($"Is Minted: {response.Data.IsMinted}");
      Console.WriteLine($"Is Transferable: {response.Data.IsTransferable}");
      Console.WriteLine($"Is Transferring: {response.Data.IsTransferring}");
      Console.WriteLine($"Mint For Address: {response.Data.MintForAddress}");
      Console.WriteLine($"Mint For User UID: {response.Data.MintForUserUid}");
      Console.WriteLine($"Name: {response.Data.Name}");
      Console.WriteLine($"Owner Address: {response.Data.OwnerAddress}");
      Console.WriteLine($"Owner User UID: {response.Data.OwnerUserUid}");
      Console.WriteLine($"Token Address: {response.Data.TokenAddress}");
      Console.WriteLine($"Token ID: {response.Data.TokenId}");
      Console.WriteLine($"Token URI: {response.Data.TokenUri}");
      Console.WriteLine($"UID: {response.Data.Uid}");
      Console.WriteLine($"Metadata Attributes: {response.Data.Metadata.Attributes?.Count}");
      if (response.Data.Metadata.Attributes != null)
      {
        foreach (var attribute in response.Data.Metadata.Attributes)
        {
          Console.WriteLine($"Metadata Attribute: {attribute.TraitType}, {attribute.Value}, {attribute.DisplayType}");
        }
      }
      Console.WriteLine($"Metadata Description: {response.Data.Metadata.Description}");
      Console.WriteLine($"Metadata ExternalUrl: {response.Data.Metadata.ExternalUrl}");
      Console.WriteLine($"Metadata Image: {response.Data.Metadata.Image}");
      Console.WriteLine($"Metadata Name: {response.Data.Metadata.Name}");
      Console.WriteLine($"Metadata OrgImageUrl: {response.Data.Metadata.OrgImageUrl}");
      Console.WriteLine($"Metadata OrginalId: {response.Data.Metadata.OriginalId}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetAssetsByUserUidAsync(OriginalClient client)
    {
      var userUid = "147980339890";
      ApiResponse<List<Asset>> response = await client.GetAssetsByUserUidAsync(userUid);
      Console.WriteLine("GetAssetsByUserUidAsync:");
      foreach (var asset in response.Data)
      {
        Console.WriteLine($"Asset UID: {asset.Uid}, Name: {asset.Name}");
      }
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestEditAssetAsync(OriginalClient client)
    {
      EditAssetParams editParams = new EditAssetParams
      {
        Data = new EditAssetData
        {
          Name = "Updated Asset Name",
          Description = "Updated Description",
          ExternalUrl = "https://example-updated.com",
          ImageUrl = "https://fastly.picsum.photos/id/912/128/128.jpg?hmac=KjVE4IURO5BwnZLpAswjhNhSJpiMxdLmO1cLbBgdH84",
          Attributes = new List<AssetAttribute>
                {
                    new AssetAttribute { TraitType = "Updated Trait", Value = "Updated Value", DisplayType = "Display Type" }
                }
        }
      };
      var assetUid = "373337353781";
      ApiResponse<object> response = await client.EditAssetAsync(assetUid, editParams);
      Console.WriteLine("EditAssetAsync:");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestCreateTransferAsync(OriginalClient client)
    {
      TransferParams transferParams = new TransferParams
      {
        AssetUid = "373337353781",
        FromUserUid = "147980339890",
        ToAddress = "61SrkpNh7BE7S8ZLqVY5JXm4JU7Rm1EaqRMkaJV8XW5R"
      };
      ApiResponse<UidResponse> response = await client.CreateTransferAsync(transferParams);
      Console.WriteLine("CreateTransferAsync:");
      Console.WriteLine($"New Transfer UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetTransferAsync(OriginalClient client)
    {
      var transferUid = "885359965902";
      ApiResponse<Transfer> response = await client.GetTransferAsync(transferUid);
      Console.WriteLine("GetTransferAsync:");
      Console.WriteLine($"Transfer UID: {response.Data.Uid}");
      Console.WriteLine($"From User UID: {response.Data.FromUserUid}");
      Console.WriteLine($"To Address: {response.Data.ToAddress}");
      Console.WriteLine($"Status: {response.Data.Status}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetTransfersByUserUidAsync(OriginalClient client)
    {
      var userUid = "147980339890";
      ApiResponse<List<Transfer>> response = await client.GetTransfersByUserUidAsync(userUid);
      Console.WriteLine("GetTransfersByUserUidAsync:");
      foreach (var transfer in response.Data)
      {
        Console.WriteLine($"Asset UID: {transfer.AssetUid}");
        Console.WriteLine($"Created At: {transfer.CreatedAt}");
        Console.WriteLine($"From User UID: {transfer.FromUserUid}");
        Console.WriteLine($"Status: {transfer.Status}");
        Console.WriteLine($"To Address: {transfer.ToAddress}");
        Console.WriteLine($"UID: {transfer.Uid}");
      }
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestCreateBurnAsync(OriginalClient client)
    {
      BurnParams burnParams = new BurnParams
      {
        AssetUid = "373337353781",
        FromUserUid = "364718224260"
      };
      ApiResponse<UidResponse> response = await client.CreateBurnAsync(burnParams);
      Console.WriteLine("CreateBurnAsync:");
      Console.WriteLine($"New Burn UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetBurnAsync(OriginalClient client)
    {
      var burnUid = "662859149573";
      ApiResponse<Burn> response = await client.GetBurnAsync(burnUid);
      Console.WriteLine("GetBurnAsync:");
      Console.WriteLine($"Asset UID: {response.Data.AssetUid}");
      Console.WriteLine($"Created At: {response.Data.CreatedAt}");
      Console.WriteLine($"From User UID: {response.Data.FromUserUid}");
      Console.WriteLine($"Status: {response.Data.Status}");
      Console.WriteLine($"Burn UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetBurnsByUserUidAsync(OriginalClient client)
    {
      var userUid = "364718224260";
      ApiResponse<List<Burn>> response = await client.GetBurnsByUserUidAsync(userUid);
      Console.WriteLine("GetBurnsByUserUidAsync:");
      foreach (var burn in response.Data)
      {
        Console.WriteLine($"Asset UID: {burn.AssetUid}");
        Console.WriteLine($"Created At: {burn.CreatedAt}");
        Console.WriteLine($"From User UID: {burn.FromUserUid}");
        Console.WriteLine($"Status: {burn.Status}");
        Console.WriteLine($"Burn UID: {burn.Uid}");
      }
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetDepositAsync(OriginalClient client)
    {
      var userUid = "147980339890";
      var collection_uid = "940830618348";
      ApiResponse<Deposit> response = await client.GetDepositAsync(userUid, collection_uid);
      Console.WriteLine("GetDepositAsync:");
      Console.WriteLine($"QR Code Data: {response.Data.QrCodeData}");
      Console.WriteLine($"Wallet Address: {response.Data.WalletAddress}");
      Console.WriteLine($"Network: {response.Data.Network}");
      Console.WriteLine($"Chain ID: {response.Data.ChainId}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetRewardAsync(OriginalClient client)
    {
      var rewardUid = "501676762240";
      ApiResponse<Reward> response = await client.GetRewardAsync(rewardUid);
      Console.WriteLine("GetRewardAsync:");
      Console.WriteLine($"Contract Address: {response.Data.ContractAddress}");
      Console.WriteLine($"Created At: {response.Data.CreatedAt}");
      Console.WriteLine($"Description: {response.Data.Description}");
      Console.WriteLine($"Explorer URL: {response.Data.ExplorerUrl}");
      Console.WriteLine($"Name: {response.Data.Name}");
      Console.WriteLine($"Status: {response.Data.Status}");
      Console.WriteLine($"Token Name: {response.Data.TokenName}");
      Console.WriteLine($"Token Type: {response.Data.TokenType}");
      Console.WriteLine($"UID: {response.Data.Uid}");
      Console.WriteLine($"Withdraw Receiver: {response.Data.WithdrawReceiver}");
      Console.WriteLine($"Chain: {response.Data.Chain}");
      Console.WriteLine($"Network: {response.Data.Network}");
      Console.WriteLine($"Chain ID: {response.Data.ChainId}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestCreateAllocationAsync(OriginalClient client)
    {
      var randomNonce = new Random().Next(100000, 999999);
      AllocationParams allocationParams = new AllocationParams
      {
        Amount = 0.1,
        Nonce = $"{randomNonce}",
        RewardUid = "501676762240",
        ToUserUid = "654716880029"
      };
      ApiResponse<UidResponse> response = await client.CreateAllocationAsync(allocationParams);
      Console.WriteLine("CreateAllocationAsync:");
      Console.WriteLine($"New Allocation UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetAllocationAsync(OriginalClient client)
    {
      var allocationUid = "529168034396";
      ApiResponse<Allocation> response = await client.GetAllocationAsync(allocationUid);
      Console.WriteLine("GetAllocationAsync:");
      Console.WriteLine($"Amount: {response.Data.Amount}");
      Console.WriteLine($"Created At: {response.Data.CreatedAt}");
      Console.WriteLine($"Nonce: {response.Data.Nonce}");
      Console.WriteLine($"Reward UID: {response.Data.RewardUid}");
      Console.WriteLine($"Status: {response.Data.Status}");
      Console.WriteLine($"To User UID: {response.Data.ToUserUid}");
      Console.WriteLine($"UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetAllocationsByUserUidAsync(OriginalClient client)
    {
      var userUid = "654716880029";
      ApiResponse<List<Allocation>> response = await client.GetAllocationsByUserUidAsync(userUid);
      Console.WriteLine("GetAllocationsByUserUidAsync:");
      foreach (var allocation in response.Data)
      {
        Console.WriteLine($"Amount: {allocation.Amount}");
        Console.WriteLine($"Created At: {allocation.CreatedAt}");
        Console.WriteLine($"Nonce: {allocation.Nonce}");
        Console.WriteLine($"Reward UID: {allocation.RewardUid}");
        Console.WriteLine($"Status: {allocation.Status}");
        Console.WriteLine($"To User UID: {allocation.ToUserUid}");
        Console.WriteLine($"UID: {allocation.Uid}");
      }
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestCreateClaimAsync(OriginalClient client)
    {
      ClaimParams claimParams = new ClaimParams
      {
        FromUserUid = "654716880029",
        RewardUid = "501676762240",
        ToAddress = "0x79998F1C1eA7d58Ee70dc0301db3e1370F2f1e90"
      };
      ApiResponse<UidResponse> response = await client.CreateClaimAsync(claimParams);
      Console.WriteLine("CreateClaimAsync:");
      Console.WriteLine($"New Claim UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetClaimAsync(OriginalClient client)
    {
      var claimUid = "227092682937";
      ApiResponse<Claim> response = await client.GetClaimAsync(claimUid);
      Console.WriteLine("GetClaimAsync:");
      Console.WriteLine($"Amount: {response.Data.Amount}");
      Console.WriteLine($"Created At: {response.Data.CreatedAt}");
      Console.WriteLine($"From User UID: {response.Data.FromUserUid}");
      Console.WriteLine($"Reward UID: {response.Data.RewardUid}");
      Console.WriteLine($"Status: {response.Data.Status}");
      Console.WriteLine($"To Address: {response.Data.ToAddress}");
      Console.WriteLine($"UID: {response.Data.Uid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetClaimsByUserUidAsync(OriginalClient client)
    {
      var userUid = "144745202401";
      ApiResponse<List<Claim>> response = await client.GetClaimsByUserUidAsync(userUid);
      Console.WriteLine("GetClaimsByUserUidAsync:");
      foreach (var claim in response.Data)
      {
        Console.WriteLine($"Amount: {claim.Amount}");
        Console.WriteLine($"Created At: {claim.CreatedAt}");
        Console.WriteLine($"From User UID: {claim.FromUserUid}");
        Console.WriteLine($"Reward UID: {claim.RewardUid}");
        Console.WriteLine($"Status: {claim.Status}");
        Console.WriteLine($"To Address: {claim.ToAddress}");
        Console.WriteLine($"UID: {claim.Uid}");
      }
      Console.WriteLine($"Success: {response.Success}\n");
    }

    static async Task TestGetBalanceAsync(OriginalClient client)
    {
      var rewardUid = "501676762240";
      var userUid = "654716880029";
      ApiResponse<Balance> response = await client.GetBalanceAsync(rewardUid, userUid);
      Console.WriteLine("TestGetBalanceAsync:");
      Console.WriteLine($"Amount: {response.Data.Amount}");
      Console.WriteLine($"Reward UID: {response.Data.RewardUid}");
      Console.WriteLine($"User UID: {response.Data.UserUid}");
      Console.WriteLine($"Success: {response.Success}\n");
    }
  }
}