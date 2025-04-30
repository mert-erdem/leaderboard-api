using System.ComponentModel.DataAnnotations;

namespace LeaderboardApi.Entities;

/// <summary>
/// Games are permanent.
/// </summary>
public class Game
{
    [Key] public int Id { get; set; }

    public required string Name { get; set; }
}