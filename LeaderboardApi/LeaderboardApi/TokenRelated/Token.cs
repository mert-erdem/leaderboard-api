namespace LeaderboardApi.TokenRelated;

public class Token
{
    public string AccessToken { get; set; }

    public DateTime AccessTokenExpireDate { get; set; }

    public string RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpireDate { get; set; }
}