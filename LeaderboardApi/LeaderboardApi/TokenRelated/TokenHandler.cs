using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LeaderboardApi.Entities;
using Microsoft.IdentityModel.Tokens;

namespace LeaderboardApi.TokenRelated;

public class TokenHandler
{
    private readonly double _accessTokenExpireMinutes;
    private readonly double _refreshTokenExpireDays;

    private readonly IConfiguration _config;

    public TokenHandler(IConfiguration config)
    {
        _config = config;
        _accessTokenExpireMinutes = _config.GetValue<double>("JwtSettings:AccessTokenMinutes");
        _refreshTokenExpireDays = _config.GetValue<double>("JwtSettings:RefreshTokenDays");
    }
    
    /// <summary>
    /// Generates JWT Token for access and refresh purposes.
    /// Expire times of tokens are set by this method.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    public Token GenerateToken(User user)
    {
        var token = new Token();

        var claims = new List<Claim>
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Email)
        };
        
        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:SecurityKey"]!));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
        token.AccessTokenExpireDate = DateTime.UtcNow.AddMinutes(_accessTokenExpireMinutes);
        
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _config["Token:Issuer"], 
            audience: _config["Token:Audience"], 
            claims: claims,
            expires: token.AccessTokenExpireDate, 
            notBefore: DateTime.UtcNow, 
            signingCredentials: signingCredentials
        );
        
        // Create Tokens
        var tokenHandler = new JwtSecurityTokenHandler();
        token.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);
        token.RefreshToken = CreateRefreshToken();
        token.RefreshTokenExpireDate = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

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