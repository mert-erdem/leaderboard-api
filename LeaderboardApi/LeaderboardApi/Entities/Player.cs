using System.ComponentModel.DataAnnotations;

namespace LeaderboardApi.Entities;

/// <summary>
/// Players are permanent.
/// </summary>
public class Player
{
    [Key] public int Id { get; set; }
    
    public required string Name { get; set; }

    public required int UserId { get; set; }

    /// <summary>
    /// User can be deleted, however Player's records will stay.
    /// </summary>
    public User? User { get; set; }
    
    public List<GameScore>? GameScores { get; set; }
}