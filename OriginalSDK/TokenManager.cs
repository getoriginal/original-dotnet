using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace OriginalSDK
{
  public class TokenManager
  {
    private readonly string _apiSecret;

    public TokenManager(string apiSecret)
    {
      _apiSecret = apiSecret;

      if (string.IsNullOrEmpty(apiSecret))
      {
        throw new ArgumentException("API secret must be provided.");
      }
    }

    // Method to generate JWT token
    public string GenerateToken()
    {
      var payload = new[]
      {
        new Claim("resource", "*"),
        new Claim("action", "*"),
        new Claim("user_id", "*")
    };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_apiSecret));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
          claims: payload,
          expires: DateTime.Now.AddHours(1),
          signingCredentials: creds
      );

      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
