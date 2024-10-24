namespace OriginalSDK
{
  public class UserWallet
  {
    public required string Address { get; set; }
    public string? ExplorerUrl { get; set; }
    public required string Network { get; set; }
    public required int ChainId { get; set; }
  }

  public class User
  {
    public required string Uid { get; set; }
    public string? UserExternalId { get; set; }
    public required string CreatedAt { get; set; }
    public string? Email { get; set; }
    public required List<UserWallet> Wallets { get; set; }
  }
}
