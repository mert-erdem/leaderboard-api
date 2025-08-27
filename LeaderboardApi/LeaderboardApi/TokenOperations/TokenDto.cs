namespace LeaderboardApi.TokenOperations;

public class TokenDto(string accessToken, string refreshToken)
{
    public string AccessToken { get; private set; } = accessToken;

    public string RefreshToken { get; private set; } = refreshToken;
}