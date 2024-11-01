# Official Server-Side C# .NET SDK for [Original API](https://getoriginal.com)

## Table of Contents

- [Getting Started](#getting-started)
- [Documentation](#documentation)
  - [Initialization](#initialization)
  - [User](#user)
    - [Create a New User](#create-a-new-user)
    - [Get a User by UID](#get-a-user-by-uid)
    - [Get a User by Email](#get-a-user-by-email)
    - [Get a User by External ID](#get-a-user-by-external-id)
  - [Asset](#asset)
    - [Create a New Asset](#create-a-new-asset)
    - [Get an Asset by UID](#get-an-asset-by-uid)
    - [Edit an Asset](#edit-an-asset)
  - [Transfer](#transfer)
    - [Create a Transfer](#create-a-transfer)
    - [Get a Transfer by UID](#get-a-transfer-by-uid)
    - [Get Transfers by User UID](#get-transfers-by-user-uid)
  - [Burn](#burn)
    - [Create a Burn](#create-a-burn)
    - [Get a Burn by UID](#get-a-burn-by-uid)
    - [Get Burns by User UID](#get-burns-by-user-uid)
  - [Deposit](#deposit)
    - [Get Deposit Details by User UID](#get-deposit-details-by-user-and-collection)
  - [Collection](#collection)
    - [Get a Collection by UID](#get-a-collection-by-uid)
  - [Reward](#reward)
    - [Get a Reward by UID](#get-a-reward-by-uid)
  - [Allocation](#allocation)
    - [Create a New Allocation](#create-a-new-allocation)
    - [Get an Allocation by UID](#get-an-allocation-by-uid)
    - [Get Allocations by User UID](#get-allocations-by-user-uid)
  - [Claim](#claim)
    - [Create a New Claim](#create-a-new-claim)
    - [Get a Claim by UID](#get-a-claim-by-uid)
    - [Get Claims by User UID](#get-claims-by-user-uid)
  - [Balance](#balance)
    - [Get Reward Balance by User UID](#get-reward-balance-by-user-uid)
  - [Handling Errors](#handling-errors)

## Getting Started

To start using the C# SDK for the [Original](https://getoriginal.com) API, register for an account and obtain your API key and secret. Once set up, install the SDK via NuGet:

```bash
dotnet add package OriginalSDK
```

# Documentation

For the full documentation, please refer to https://docs.getoriginal.com.

## Initialization

The Original SDK provides access to API methods in a strongly typed manner.

For development environments:

```csharp
using OriginalSDK;

OriginalClient client = new OriginalClient(
  apiKey: "YOUR_API_KEY",
  apiSecret: "YOUR_API_SECRET",
  options: new OriginalOptions { Environment = OriginalEnvironment.Development }
);
```

For production environments:

```csharp
using OriginalSDK;

OriginalClient client = new OriginalClient(
  apiKey: "YOUR_API_KEY",
  apiSecret: "YOUR_API_SECRET",
  options: new OriginalOptions { Environment = OriginalEnvironment.Production }
);
```

### Using environment variables

You can also set environment variables which will be picked up by the SDK:

```
# .env file
ORIGINAL_API_KEY=your_api_key_here
ORIGINAL_API_SECRET=your_api_secret_here
ORIGINAL_ENVIRONMENT=development #(or production)
```

`ORIGINAL_BASE_URL` can also be set, however this is not recommended and is for advanced/internal use cases only.

```csharp
// Utilises environment variables
OriginalClient client = new OriginalClient();
```

## User

### Create a New User

```csharp
UserParams userParams = new UserParams
{
    UserExternalId = "YOUR_USER_EXTERNAL_ID",
    Email = "YOUR_EMAIL"
};
ApiResponse<UidResponse> response = await client.CreateUserAsync(userParams);
var userUid = response.Data.Uid;

// Sample response
new ApiResponse<UidResponse>
{
    Success = true,
    Data = new UidResponse
    {
        Uid = "175324281338"
    }
};
```

### Get a User by UID

```csharp
ApiResponse<User> response = await client.GetUserAsync("USER_UID");
var userDetails = response.Data;

// Sample response on success
new ApiResponse<User>
{
    Success = true,
    Data = new User
    {
        Uid = "754566475542",
        UserExternalId = "user_external_id",
        CreatedAt = DateTime.Parse("2024-02-26T13:12:31.798296Z"),
        Email = "user_email@email.com",
        WalletAddress = "0xa22f2dfe189ed3d16bb5bda5e5763b2919058e40",
        Wallets = new List<Wallet>
        {
            new Wallet
            {
                Address = "0x1d6169328e0a2e0a0709115d1860c682cf8d1398",
                ChainId = 80001,
                ExplorerUrl = "https://amoy.polygonscan.com/address/0x1d6169328e0a2e0a0709115d1860c682cf8d1398",
                Network = "Amoy"
            }
        }
    }
};

// Sample response if user not found
new ApiResponse<User>
{
    Success = false,
    Data = null
};

```

### Get a User by Email

```csharp
ApiResponse<User> response = await client.GetUserByEmailAsync("YOUR_EMAIL");
var userDetails = response.Data;

// Sample response - the same as "Get a User by UID" above
```

### Get a User by External ID

```csharp
ApiResponse<User> response = await client.GetUserByUserExternalIdAsync("YOUR_USER_EXTERNAL_ID");
var userDetails = response.Data;

// Sample response - the same as "Get a User by UID" above
```

## Asset

### Create a New Asset

```csharp
AssetParams assetParams = new AssetParams
{
    UserUid = "USER_UID",
    CollectionUid = "COLLECTION_UID",
    AssetExternalId = "ASSET_EXTERNAL_ID",
    SalePriceInUsd = 9.99m,
    Data = new AssetData
    {
        Name = "Asset Name",
        UniqueName = true,
        ImageUrl = "https://example.com/image.png",
        Attributes = new List<AssetAttribute>
        {
            new AssetAttribute { TraitType = "Eyes", Value = "Green" },
            new AssetAttribute { TraitType = "Hair", Value = "Black" }
        }
    }
};
ApiResponse<UidResponse> response = await client.CreateAssetAsync(assetParams);
var assetUid = response.Data.Uid;

// Sample response
new ApiResponse<UidResponse>
{
    Success = true,
    Data = new UidResponse
    {
        Uid = "151854912345"
    }
};
```

### Get an Asset by UID

```csharp
ApiResponse<Asset> response = await client.GetAssetAsync("ASSET_UID");
var assetDetails = response.Data;

// Sample response
new ApiResponse<Asset>
{
    Success = true,
    Data = new Asset
    {
        Uid = "151854912345",
        Name = "Random Name #2",
        AssetExternalId = "asset_external_id_1",
        CollectionUid = "471616646163",
        CollectionName = "Test SDK Collection 1",
        TokenId = 2,
        CreatedAt = DateTime.Parse("2024-02-16T11:33:19.577827Z"),
        IsMinted = true,
        IsBurned = false,
        IsTransferring = false,
        IsTransferable = true,
        IsEditing = false,
        MintForUserUid = "885810911461",
        OwnerUserUid = "885810911461",
        OwnerAddress = "0x32e28bfe647939d073d39113c697a11e3065ea97",
        Metadata = new Metadata
        {
            Name = "Random Name",
            Image = "https://cryptopunks.app/cryptopunks/cryptopunk1081.png",
            Description = "nft_description",
            OriginalId = "151854912345",
            ExternalUrl = "external_url@example.com",
            OrgImageUrl = "https://cryptopunks.app/cryptopunks/cryptopunk1081.png",
            Attributes = new List<AssetAttribute>
            {
                new AssetAttribute
                {
                    TraitType = "Stamina Increase",
                    DisplayType = "boost_percentage",
                    Value = 10
                }
            }
        },
        ExplorerUrl = "https://mumbai.polygonscan.com/token/0x124a6755ee787153bb6228463d5dc3a02890a7db?a=2",
        TokenUri = "https://storage.googleapis.com/original-production-media/data/metadata/9ac0dad4-75ae-4406-94fd-1a0f6bf75db3.json"
    }
};

```

### Edit an Asset

```csharp
EditAssetParams editAssetParams = new EditAssetParams
{
    Data = new EditAssetData
    {
        Name = "Updated Asset Name",
        Description = "Updated Description",
        Attributes = new List<AssetAttribute>
        {
            new AssetAttribute { TraitType = "Eyes", Value = "Blue" },
            new AssetAttribute { TraitType = "Hair", Value = "Blonde" }
        }
    }
};
ApiResponse<object> response = await client.EditAssetAsync("ASSET_UID", editAssetParams);
bool editSuccess = response.Success;

// Sample response
new ApiResponse<object>
{
    Success = true,
    Data = null
}
```

## Transfer

### Create a Transfer

```csharp
TransferParams transferParams = new TransferParams
{
    AssetUid = "ASSET_UID",
    FromUserUid = "FROM_USER_UID",
    ToAddress = "0xRecipientAddress"
};
ApiResponse<UidResponse> response = await client.CreateTransferAsync(transferParams);
var transferUid = response.Data.Uid;

// Sample response
new ApiResponse<UidResponse>
{
    Success = true,
    Data = new UidResponse
    {
        Uid = "883072660397"
    }
};
```

### Get a Transfer by UID

```csharp
ApiResponse<Transfer> response = await client.GetTransferAsync("TRANSFER_UID");
var transferDetails = response.Data;

// Sample response
new ApiResponse<Transfer>
{
    Success = true,
    Data = new Transfer
    {
        Uid = "883072660397",
        Status = "done",
        AssetUid = "708469717542",
        FromUserUid = "149997600351",
        ToAddress = "0xe02522d0ac9f53e35a56f42cd5e54fc7b5a12f05",
        CreatedAt = DateTime.Parse("2024-02-26T10:20:17.668254Z")
    }
};
```

### Get Transfers by User UID

```csharp
ApiResponse<List<Transfer>> response = await client.GetTransfersByUserUidAsync("USER_UID");
var transferDetails = response.Data;

// Sample response
new ApiResponse<List<Transfer>>
{
    Success = true,
    Data = new List<Transfer>
    {
        new Transfer
        {
            Uid = "883072660397",
            Status = "done",
            AssetUid = "708469717542",
            FromUserUid = "149997600351",
            ToAddress = "0xe02522d0ac9f53e35a56f42cd5e54fc7b5a12f05",
            CreatedAt = DateTime.Parse("2024-02-26T10:20:17.668254Z")
        },
        new Transfer {
          // ...
        }
    }
};
```

## Burn

### Create a Burn

```csharp
BurnParams burnParams = new BurnParams
{
    AssetUid = "ASSET_UID",
    FromUserUid = "USER_UID"
};
ApiResponse<UidResponse> response = await client.CreateBurnAsync(burnParams);
var burnUid = response.Data.Uid;

// Sample response
new ApiResponse<UidResponse>
{
    Success = true,
    Data = new UidResponse
    {
        Uid = "365684656925"
    }
};

```

### Get a Burn by UID

```csharp
ApiResponse<Burn> response = await client.GetBurnAsync("BURN_UID");
var burnDetails = response.Data;

// Sample response
new ApiResponse<Burn>
{
    Success = true,
    Data = new Burn
    {
        Uid = "365684656925",
        Status = "done",
        AssetUid = "708469717542",
        FromUserUid = "483581848722",
        CreatedAt = DateTime.Parse("2024-02-26T10:20:17.668254Z")
    }
};
```

### Get Burns by User UID

```csharp
ApiResponse<List<Burn>> response = await client.GetBurnsByUserUidAsync("USER_UID");
var burnDetails = response.Data;

// Sample response
new ApiResponse<List<Burn>>
{
    Success = true,
    Data = new List<Burn>
    {
        new Burn
        {
            Uid = "365684656925",
            Status = "done",
            AssetUid = "708469717542",
            FromUserUid = "483581848722",
            CreatedAt = DateTime.Parse("2024-02-26T10:20:17.668254Z")
        },
        {
            //...
        }
    }
};
```

## Deposit

### Get Deposit Details by User and Collection

```csharp
ApiResponse<Deposit> response = await client.GetDepositAsync("USER_UID", "COLLECTION_UID");
var depositDetails = response.Data;

// Sample response
new ApiResponse<Deposit>
{
    Success = true,
    Data = new Deposit
    {
        Network = "Mumbai",
        ChainId = 80001,
        WalletAddress = "0x1d6169328e0a2e0a0709115d1860c682cf8d1398",
        QrCodeData = "ethereum:0x1d6169328e0a2e0a0709115d1860c682cf8d1398@80001"
    }
};
```

## Collection

### Get a Collection by UID

```csharp
ApiResponse<Collection> response = await client.GetCollectionAsync("COLLECTION_UID");
var collectionDetails = response.Data;

// Sample response
new ApiResponse<Collection>
{
    Success = true,
    Data = new Collection
    {
        Uid = "221137489875",
        Name = "Test SDK Collection 1",
        Status = "deployed",
        Type = "ERC721",
        CreatedAt = DateTime.Parse("2024-02-13T10:45:56.952745Z"),
        EditableAssets = true,
        ContractAddress = "0x124a6755ee787153bb6228463d5dc3a02890a7db",
        Symbol = "SYM",
        Description = "Description of the collection",
        ExplorerUrl = "https://mumbai.polygonscan.com/address/0x124a6755ee787153bb6228463d5dc3a02890a7db"
    }
};
```

## Reward

### Get a Reward by UID

```csharp
ApiResponse<Reward> response = await client.GetRewardAsync("REWARD_UID");
var rewardDetails = response.Data;

// Sample response
new ApiResponse<Reward>
{
    Success = true,
    Data = new Reward
    {
        Uid = "151854912345",
        Name = "Test SDK Reward 1",
        Status = "deployed",
        TokenType = "ERC20",
        TokenName = "TestnetORI",
        CreatedAt = DateTime.Parse("2024-02-13T10:45:56.952745Z"),
        ContractAddress = "0x124a6755ee787153bb6228463d5dc3a02890a7db",
        WithdrawReceiver = "0x4881ab2f73c48a54b907a8b697b270f490768e6d",
        Description = "Description of the reward",
        ExplorerUrl = "https://mumbai.polygonscan.com/address/0x124a6755ee787153bb6228463d5dc3a02890a7db"
    }
};
```

## Allocation

### Create a New Allocation

```csharp
AllocationParams allocationParams = new AllocationParams
{
    Amount = 100,
    Nonce = "random_nonce",
    RewardUid = "REWARD_UID",
    ToUserUid = "USER_UID"
};
ApiResponse<UidResponse> response = await client.CreateAllocationAsync(allocationParams);
var allocationUid = response.Data.Uid;

// Sample response
new ApiResponse<UidResponse>
{
    Success = true,
    Data = new UidResponse
    {
        Uid = "151854912345"
    }
};
```

### Get an Allocation by UID

```csharp
ApiResponse<Allocation> response = await client.GetAllocationAsync("ALLOCATION_UID");
var allocationDetails = response.Data;

// Sample response
new ApiResponse<Allocation>
{
    Success = true,
    Data = new Allocation
    {
        Uid = "151854912345",
        Status = "done",
        RewardUid = "reward_uid",
        ToUserUid = "754566475542",
        Amount = 123.123,
        Nonce = "nonce1",
        CreatedAt = DateTime.Parse("2024-02-16T11:33:19.577827Z")
    }
};
```

### Get Allocations by User UID

```csharp
ApiResponse<List<Allocation>> response = await client.GetAllocationsByUserUidAsync("USER_UID");
var allocationDetails = response.Data;

// Sample response
new ApiResponse<List<Allocation>>
{
    Success = true,
    Data = new List<Allocation>
    {
        new Allocation {
            Uid = "151854912345",
            Status = "done",
            RewardUid = "reward_uid",
            ToUserUid = "754566475542",
            Amount = 123.123,
            Nonce = "nonce1",
            CreatedAt = DateTime.Parse("2024-02-16T11:33:19.577827Z")
        }
    }
};
```

## Claim

### Create a New Claim

```csharp
ClaimParams claimParams = new ClaimParams
{
    FromUserUid = "USER_UID",
    RewardUid = "REWARD_UID",
    ToAddress = "0xRecipientAddress"
};
ApiResponse<UidResponse> response = await client.CreateClaimAsync(claimParams);
var claimUid = response.Data.Uid;

// Sample response
new ApiResponse<UidResponse>
{
    Success = true,
    Data = new UidResponse
    {
        Uid = "151854912345"
    }
};
```

### Get a Claim by UID

```csharp
ApiResponse<Claim> response = await client.GetClaimAsync("CLAIM_UID");
var claimDetails = response.Data;

// Sample response
new ApiResponse<Claim>
{
    Success = true,
    Data = new Claim
    {
        Uid = "151854912345",
        Status = "done",
        RewardUid = "708469717542",
        FromUserUid = "754566475542",
        ToAddress = "0x4881ab2f73c48a54b907a8b697b270f490768e6d",
        Amount = 123.123,
        CreatedAt = DateTime.Parse("2024-02-16T11:33:19.577827Z")
    }
};

```

### Get Claims by User UID

```csharp
ApiResponse<List<Claim>> response = await client.GetClaimsByUserUidAsync("USER_UID");
var claimDetails = response.Data;

// Sample response
new ApiResponse<List<Claim>>
{
    Success = true,
    Data = new List<Claim>
    {
        new Claim {
            Uid = "151854912345",
            Status = "done",
            RewardUid = "708469717542",
            FromUserUid = "754566475542",
            ToAddress = "0x4881ab2f73c48a54b907a8b697b270f490768e6d",
            Amount = 123.123,
            CreatedAt = DateTime.Parse("2024-02-16T11:33:19.577827Z")
        }
    }
};

```

## Balance

### Get Reward Balance by User UID

```csharp
ApiResponse<Balance> response = await client.GetBalanceAsync("REWARD_UID", "USER_UID");
var balanceData = response.Data;

// Sample response
new ApiResponse<Balance>
{
    Success = true,
    Data = new Balance
    {
        RewardUid = "151854912345",
        UserUid = "754566475542",
        Amount = 123.123
    }
};
```

## Handling Errors

You can catch all errors with the `OriginalException` class:

```csharp
try
{
    var response = await client.CreateUserAsync(new UserParams { UserExternalId = "user_external_id", Email = "invalid_email" });
}
catch (OriginalException ex)
{
    // Handle all errors
    Console.WriteLine($"Error: {ex.Message}");
}
```

Or, catch specific errors:

```csharp
try
{
    var response = await client.CreateUserAsync(new UserParams {
      UserExternalId = "user_external_id",
      Email = "invalid_email"
    });
}
catch (ClientException ex)
{
    Console.WriteLine($"Client error: {ex.Message}");
}
catch (ServerException ex)
{
    Console.WriteLine($"Server error: {ex.Message}");
}
catch (ValidationException ex)
{
    Console.WriteLine($"Validation error: {ex.Message}");
}
```
