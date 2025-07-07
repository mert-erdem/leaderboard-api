namespace LeaderboardApi.Operations.GameScoreOps.Queries.ViewModels;

public class GameScoreViewModel
{
    public int Id { get; set; }
        
    public required string GameName { get; set; }

    public required string PlayerName { get; set; }

    public double Score { get; set; }
    
    public DateTime LastEditedTime { get; set; }
}