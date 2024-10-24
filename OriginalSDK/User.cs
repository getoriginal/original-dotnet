namespace OriginalSDK
{

  public class UserWallet
  {
    public string Address { get; set; }
    public int? ChainId { get; set; }
    public string ExplorerUrl { get; set; }
    public string Network { get; set; }
  }

  public class User
  {
    public string CreatedAt { get; set; }

    public string Email { get; set; }

    public string Uid { get; set; }


    public string? UserExternalId { get; set; }

    public string? WalletAddress { get; set; }

    public List<UserWallet>? Wallets { get; set; }
  }

}
