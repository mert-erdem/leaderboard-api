using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeaderboardApi.Entities;

/// <summary>
/// The score holder class.
/// GameScore's are permanent.
/// </summary>
public class GameScore
{
    [Key] public int Id { get; set; }
    
    [ForeignKey("GameId")] public int GameId { get; set; }

    public Game? Game { get; set; }
    
    [ForeignKey("PlayerId")] public int PlayerId { get; set; }

    public Player? Player { get; set; }

    public double Score { get; set; }

    public DateTime LastEditedTime { get; set; }
}