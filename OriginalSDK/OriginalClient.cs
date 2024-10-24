using OriginalSDK.Models;

namespace OriginalSDK
{
  public class OriginalClient : BaseClient
  {
    public Task<ApiResponse<UidResponse>> CreateUserAsync(UserParams userParams)
    {
      return PostAsync<UidResponse>("user", userParams);
    }

    public Task<ApiResponse<User>> GetUserAsync(string userUid)
    {
      return GetAsync<User>($"user/{userUid}");
    }

    public Task<ApiResponse<User>> GetUserByEmailAsync(string email)
    {
      return GetAsync<User>($"user/?email={email}");
    }

    public Task<ApiResponse<User>> GetUserByUserExternalIdAsync(string userExternalId)
    {
      return GetAsync<User>($"user/?user_external_id={userExternalId}");
    }

    public Task<ApiResponse<Collection>> GetCollectionAsync(string collectionUid)
    {
      return GetAsync<Collection>($"collection/{collectionUid}");
    }

    public Task<ApiResponse<UidResponse>> CreateAssetAsync(AssetParams assetParams)
    {
      return PostAsync<UidResponse>("asset", assetParams);
    }
  }
}
