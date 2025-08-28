using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LeaderboardApi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LeaderboardApi.TokenRelated;

public class TokenHandler
{
    private readonly IConfiguration _config;

    public TokenHandler(IConfiguration config)
    {
        _config = config;
    }
    
    public Token GenerateToken(User user)
    {
        var token = new Token();

        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Email)
        };
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:SecurityKey"]!));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        token.ExpireDate = DateTime.UtcNow.AddMinutes(15);
        
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _config["Token:Issuer"], 
            audience: _config["Token:Audience"], 
            claims: claims,
            expires: token.ExpireDate, 
            notBefore: DateTime.UtcNow, 
            signingCredentials: signingCredentials
        );
        
        // Create Tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
        token.RefreshToken = CreateRefreshToken();

        return token;
    }
    
    private string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}