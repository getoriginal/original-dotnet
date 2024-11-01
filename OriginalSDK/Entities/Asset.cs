using Newtonsoft.Json;

namespace OriginalSDK.Entities
{
  public class AssetAttribute
  {
    [JsonProperty("trait_type")]
    public string TraitType { get; set; }

    [JsonProperty("value")]
    public string Value { get; set; }

    [JsonProperty("display_type")]
    public string DisplayType { get; set; }
  }
  public class AssetData
  {
    [JsonProperty("image_url")]
    public string ImageUrl { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("store_image_on_ipfs")]
    public bool StoreImageOnIpfs { get; set; }

    [JsonProperty("unique_name")]
    public bool UniqueName { get; set; }

    [JsonProperty("attributes")]
    public List<AssetAttribute> Attributes { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("external_url")]
    public string ExternalUrl { get; set; }
  }

  public class AssetParams
  {
    [JsonProperty("asset_external_id")]
    public string AssetExternalId { get; set; }

    [JsonProperty("collection_uid")]
    public string CollectionUid { get; set; }

    [JsonProperty("data")]
    public AssetData Data { get; set; }

    [JsonProperty("user_uid")]
    public string UserUid { get; set; }

    [JsonProperty("sale_price_in_usd")]
    public double SalePriceInUsd { get; set; }
  }

  public class AssetMetadata
  {
    [JsonProperty("image")]
    public string Image { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("org_image_url")]
    public string OrgImageUrl { get; set; }

    [JsonProperty("original_id")]
    public string OriginalId { get; set; }

    [JsonProperty("attributes")]
    public List<AssetAttribute> Attributes { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("external_url")]
    public string ExternalUrl { get; set; }
  }

  public class Asset
  {
    [JsonProperty("asset_external_id")]
    public string AssetExternalId { get; set; }

    [JsonProperty("collection_name")]
    public string CollectionName { get; set; }

    [JsonProperty("collection_uid")]
    public string CollectionUid { get; set; }

    [JsonProperty("created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty("is_burned")]
    public bool IsBurned { get; set; }

    [JsonProperty("is_editing")]
    public bool IsEditing { get; set; }

    [JsonProperty("is_minted")]
    public bool IsMinted { get; set; }

    [JsonProperty("is_transferable")]
    public bool IsTransferable { get; set; }

    [JsonProperty("is_transferring")]
    public bool IsTransferring { get; set; }

    [JsonProperty("mint_for_user_uid")]
    public string MintForUserUid { get; set; }

    [JsonProperty("mint_for_address")]
    public string MintForAddress { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("token_id")]
    public string TokenId { get; set; }

    [JsonProperty("uid")]
    public string Uid { get; set; }

    [JsonProperty("explorer_url")]
    public string ExplorerUrl { get; set; }

    [JsonProperty("metadata")]
    public AssetMetadata Metadata { get; set; }

    [JsonProperty("owner_address")]
    public string OwnerAddress { get; set; }

    [JsonProperty("owner_user_uid")]
    public string OwnerUserUid { get; set; }

    [JsonProperty("token_uri")]
    public string TokenUri { get; set; }

    [JsonProperty("token_address")]
    public string TokenAddress { get; set; }
  }

}