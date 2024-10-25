using OriginalSDK.Entities;

namespace OriginalSDK
{
  public class OriginalClient : BaseClient
  {
    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <param name="userParams">Details of the user to be created.</param>
    /// <returns>Response containing the UID of the created user.</returns>
    public Task<ApiResponse<UidResponse>> CreateUserAsync(UserParams userParams)
    {
      return PostAsync<UidResponse>("user", userParams);
    }

    /// <summary>
    /// Retrieves a user by UID.
    /// </summary>
    /// <param name="uid">The UID of the user.</param>
    /// <returns>Response containing user details.</returns>
    public Task<ApiResponse<User>> GetUserAsync(string uid)
    {
      return GetAsync<User>($"user/{uid}");
    }

    /// <summary>
    /// Retrieves a user by email.
    /// </summary>
    /// <param name="email">The email of the user.</param>
    /// <returns>Response containing user details or null if not found.</returns>
    public Task<ApiResponse<User>> GetUserByEmailAsync(string email)
    {
      return GetAsync<User>($"user?email={email}");
    }

    /// <summary>
    /// Retrieves a user by external user ID.
    /// </summary>
    /// <param name="userExternalId">The external ID of the user.</param>
    /// <returns>Response containing user details or null if not found.</returns>
    public Task<ApiResponse<User>> GetUserByUserExternalIdAsync(string userExternalId)
    {
      return GetAsync<User>($"user?user_external_id={userExternalId}");
    }

    /// <summary>
    /// Retrieves details of a collection by UID.
    /// </summary>
    /// <param name="uid">The UID of the collection.</param>
    /// <returns>Response containing collection details.</returns>
    public Task<ApiResponse<Collection>> GetCollectionAsync(string uid)
    {
      return GetAsync<Collection>($"collection/{uid}");
    }

    /// <summary>
    /// Creates a new asset.
    /// </summary>
    /// <param name="assetParams">Details of the asset to be created.</param>
    /// <returns>Response containing the UID of the created asset.</returns>
    public Task<ApiResponse<UidResponse>> CreateAssetAsync(AssetParams assetParams)
    {
      return PostAsync<UidResponse>("asset", assetParams);
    }

    /// <summary>
    /// Retrieves an asset by UID.
    /// </summary>
    /// <param name="uid">The UID of the asset.</param>
    /// <returns>Response containing asset details.</returns>
    public Task<ApiResponse<Asset>> GetAssetAsync(string uid)
    {
      return GetAsync<Asset>($"asset/{uid}");
    }

    /// <summary>
    /// Retrieves a list of assets by user UID.
    /// </summary>
    /// <param name="userUid">UID of the asset owner.</param>
    /// <returns>Response containing a list of assets owned by the user, or an empty list if none found.</returns>
    public Task<ApiResponse<List<Asset>>> GetAssetsByUserUidAsync(string userUid)
    {
      return GetAsync<List<Asset>>($"asset?user_uid={userUid}");
    }

    /// <summary>
    /// Updates an asset, overwriting the existing parameters with the new ones.
    /// </summary>
    /// <param name="uid">The UID of the asset to update.</param>
    /// <param name="editAssetParams">New details of the asset. 
    /// The existing parameters will be overwritten by the newAssetParams. 
    /// If you wish to maintain some of the existing parameters, you should define them again inside the newAssetParams.
    /// </param>
    /// <returns>Response indicating the success of the update. The data will be null.</returns>
    public Task<ApiResponse<object>> EditAssetAsync(string uid, EditAssetParams editAssetParams)
    {
      return PutAsync<object>($"asset/{uid}", editAssetParams);
    }

    /// <summary>
    /// Creates a transfer of an asset.
    /// </summary>
    /// <param name="transferParams">Details of the transfer to be created.</param>
    /// <returns>Response containing the UID of the created transfer.</returns>
    public Task<ApiResponse<UidResponse>> CreateTransferAsync(TransferParams transferParams)
    {
      return PostAsync<UidResponse>("transfer", transferParams);
    }

    /// <summary>
    /// Retrieves a transfer by UID.
    /// </summary>
    /// <param name="uid">The UID of the transfer.</param>
    /// <returns>Response containing the transfer details.</returns>
    public Task<ApiResponse<Transfer>> GetTransferAsync(string uid)
    {
      return GetAsync<Transfer>($"transfer/{uid}");
    }

    /// <summary>
    /// Retrieves a list of transfers by user UID.
    /// </summary>
    /// <param name="userUid">UID of the user who made the transfers.</param>
    /// <returns>Response containing a list of transfers made by the user, or an empty list if none found.</returns>
    public Task<ApiResponse<List<Transfer>>> GetTransfersByUserUidAsync(string userUid)
    {
      return GetAsync<List<Transfer>>($"transfer?user_uid={userUid}");
    }

    /// <summary>
    /// Creates a burn action.
    /// </summary>
    /// <param name="burnParams">Details of the burn to be created.</param>
    /// <returns>Response containing the UID of the created burn.</returns>
    public Task<ApiResponse<UidResponse>> CreateBurnAsync(BurnParams burnParams)
    {
      return PostAsync<UidResponse>("burn", burnParams);
    }

    /// <summary>
    /// Retrieves burn details by UID.
    /// </summary>
    /// <param name="uid">The UID of the burn action.</param>
    /// <returns>Response containing burn details.</returns>
    public Task<ApiResponse<Burn>> GetBurnAsync(string uid)
    {
      return GetAsync<Burn>($"burn/{uid}");
    }

    /// <summary>
    /// Retrieves a list of burns by user UID.
    /// </summary>
    /// <param name="userUid">UID of the user who initiated the burns.</param>
    /// <returns>Response containing a list of burns made by the user, or an empty list if none found.</returns>
    public Task<ApiResponse<List<Burn>>> GetBurnsByUserUidAsync(string userUid)
    {
      return GetAsync<List<Burn>>($"burn?user_uid={userUid}");
    }

    /// <summary>
    /// Retrieves deposit details.
    /// </summary>
    /// <param name="userUid">UID of the user.</param>
    /// <param name="collectionUid">Optional UID of the collection.</param>
    /// <returns>Response containing deposit details.</returns>
    public Task<ApiResponse<Deposit>> GetDepositAsync(string userUid, string collectionUid)
    {
      return GetAsync<Deposit>($"deposit?user_uid={userUid}&collection_uid={collectionUid}");
    }

    /// <summary>
    /// Retrieves reward details by UID.
    /// </summary>
    /// <param name="uid">The UID of the reward.</param>
    /// <returns>Response containing reward details.</returns>
    public Task<ApiResponse<Reward>> GetRewardAsync(string uid)
    {
      return GetAsync<Reward>($"reward/{uid}");
    }

    /// <summary>
    /// Creates a reward allocation.
    /// </summary>
    /// <param name="allocationParams">Details of the allocation to be created.</param>
    /// <returns>Response containing the UID of the created allocation.</returns>
    public Task<ApiResponse<UidResponse>> CreateAllocationAsync(AllocationParams allocationParams)
    {
      return PostAsync<UidResponse>("reward/allocate", allocationParams);
    }

    /// <summary>
    /// Retrieves allocation details by UID.
    /// </summary>
    /// <param name="uid">The UID of the allocation.</param>
    /// <returns>Response containing allocation details.</returns>
    public Task<ApiResponse<Allocation>> GetAllocationAsync(string uid)
    {
      return GetAsync<Allocation>($"reward/allocate/{uid}");
    }

    /// <summary>
    /// Retrieves a list of allocations by user UID.
    /// </summary>
    /// <param name="userUid">UID of the user with allocations.</param>
    /// <returns>Response containing a list of allocations available to the user, or an empty list if none found.</returns>
    public Task<ApiResponse<List<Allocation>>> GetAllocationsByUserUidAsync(string userUid)
    {
      return GetAsync<List<Allocation>>($"reward/allocate?user_uid={userUid}");
    }

    /// <summary>
    /// Creates a claim.
    /// </summary>
    /// <param name="claimParams">Details of the claim to be created.</param>
    /// <returns>Response containing the UID of the created claim.</returns>
    public Task<ApiResponse<UidResponse>> CreateClaimAsync(ClaimParams claimParams)
    {
      return PostAsync<UidResponse>("reward/claim", claimParams);
    }

    /// <summary>
    /// Retrieves claim details by UID.
    /// </summary>
    /// <param name="uid">The UID of the claim.</param>
    /// <returns>Response containing claim details.</returns>
    public Task<ApiResponse<Claim>> GetClaimAsync(string uid)
    {
      return GetAsync<Claim>($"reward/claim/{uid}");
    }

    /// <summary>
    /// Retrieves a list of claims by user UID.
    /// </summary>
    /// <param name="userUid">UID of the user with claims.</param>
    /// <returns>Response containing a list of claims available to the user, or an empty list if none found.</returns>
    public Task<ApiResponse<List<Claim>>> GetClaimsByUserUidAsync(string userUid)
    {
      return GetAsync<List<Claim>>($"reward/claim?user_uid={userUid}");
    }

    /// <summary>
    /// Retrieves the balance of a reward for a specific user.
    /// </summary>
    /// <param name="rewardUid">The UID of the reward.</param>
    /// <param name="userUid">The UID of the user.</param>
    /// <returns>Response containing the balance of the reward for the specified user.</returns>
    public Task<ApiResponse<Balance>> GetBalanceAsync(string rewardUid, string userUid)
    {
      return GetAsync<Balance>($"reward/balance?reward_uid={rewardUid}&user_uid={userUid}");
    }
  }
}
