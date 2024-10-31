# Official Server-Side C# SDK for [Original](https://getoriginal.com) API

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

## Initialization

The Original SDK provides access to API methods in a strongly typed manner.

For development environments:

```csharp
using OriginalSDK;

var client = new OriginalClient("YOUR_DEV_APP_API_KEY", "YOUR_DEV_APP_SECRET", Environment.Development);
```

For production environments:

```csharp
using OriginalSDK;

var client = new OriginalClient("YOUR_PROD_APP_API_KEY", "YOUR_PROD_APP_SECRET");
```

## User

### Create a New User

```csharp
var userParams = new UserParams
{
    UserExternalId = "YOUR_USER_EXTERNAL_ID",
    Email = "YOUR_EMAIL"
};
var response = await client.CreateUserAsync(userParams);
var userUid = response.Data.Uid;
```

### Get a User by UID

```csharp
var response = await client.GetUserAsync("USER_UID");
var userDetails = response.Data;
```

### Get a User by Email

```csharp
var response = await client.GetUserByEmailAsync("YOUR_EMAIL");
var userDetails = response.Data;
```

### Get a User by External ID

```csharp
var response = await client.GetUserByUserExternalIdAsync("YOUR_USER_EXTERNAL_ID");
var userDetails = response.Data;
```

## Asset

### Create a New Asset

```csharp
var assetParams = new AssetParams
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
var response = await client.CreateAssetAsync(assetParams);
var assetUid = response.Data.Uid;
```

### Get an Asset by UID

```csharp
var response = await client.GetAssetAsync("ASSET_UID");
var assetDetails = response.Data;
```

### Edit an Asset

```csharp
var editAssetParams = new EditAssetParams
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
var response = await client.EditAssetAsync("ASSET_UID", editAssetParams);
bool editSuccess = response.Success;
```

## Transfer

### Create a Transfer

```csharp
var transferParams = new TransferParams
{
    AssetUid = "ASSET_UID",
    FromUserUid = "FROM_USER_UID",
    ToAddress = "0xRecipientAddress"
};
var response = await client.CreateTransferAsync(transferParams);
var transferUid = response.Data.Uid;
```

### Get a Transfer by UID

```csharp
var response = await client.GetTransferAsync("TRANSFER_UID");
var transferDetails = response.Data;
```

### Get Transfers by User UID

```csharp
var response = await client.GetTransfersByUserUidAsync("USER_UID");
var transferDetails = response.Data;
```

## Burn

### Create a Burn

```csharp
var burnParams = new BurnParams
{
    AssetUid = "ASSET_UID",
    FromUserUid = "USER_UID"
};
var response = await client.CreateBurnAsync(burnParams);
var burnUid = response.Data.Uid;
```

### Get a Burn by UID

```csharp
var response = await client.GetBurnAsync("BURN_UID");
var burnDetails = response.Data;
```

### Get Burns by User UID

```csharp
var response = await client.GetBurnsByUserUidAsync("USER_UID");
var burnDetails = response.Data;
```

## Deposit

### Get Deposit Details by User and Collection

```csharp
var response = await client.GetDepositAsync("USER_UID", "COLLECTION_UID");
var depositDetails = response.Data;
```

## Collection

### Get a Collection by UID

```csharp
var response = await client.GetCollectionAsync("COLLECTION_UID");
var collectionDetails = response.Data;
```

## Reward

### Get a Reward by UID

```csharp
var response = await client.GetRewardAsync("REWARD_UID");
var rewardDetails = response.Data;
```

## Allocation

### Create a New Allocation

```csharp
var allocationParams = new AllocationParams
{
    Amount = 100,
    Nonce = "random_nonce",
    RewardUid = "REWARD_UID",
    ToUserUid = "USER_UID"
};
var response = await client.CreateAllocationAsync(allocationParams);
var allocationUid = response.Data.Uid;
```

### Get an Allocation by UID

```csharp
var response = await client.GetAllocationAsync("ALLOCATION_UID");
var allocationDetails = response.Data;
```

### Get Allocations by User UID

```csharp
var response = await client.GetAllocationsByUserUidAsync("USER_UID");
var allocationDetails = response.Data;
```

## Claim

### Create a New Claim

```csharp
var claimParams = new ClaimParams
{
    FromUserUid = "USER_UID",
    RewardUid = "REWARD_UID",
    ToAddress = "0xRecipientAddress"
};
var response = await client.CreateClaimAsync(claimParams);
var claimUid = response.Data.Uid;
```

### Get a Claim by UID

```csharp
var response = await client.GetClaimAsync("CLAIM_UID");
var claimDetails = response.Data;
```

### Get Claims by User UID

```csharp
var response = await client.GetClaimsByUserUidAsync("USER_UID");
var claimDetails = response.Data;
```

## Balance

### Get Reward Balance by User UID

```csharp
var response = await client.GetBalanceAsync("REWARD_UID", "USER_UID");
var balanceData = response.Data;
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
