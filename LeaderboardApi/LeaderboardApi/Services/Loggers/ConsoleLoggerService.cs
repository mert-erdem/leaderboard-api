namespace LeaderboardApi.Services.Loggers;

public class ConsoleLoggerService : ILoggerService
{
    public void Log(string message)
    {
        Console.WriteLine("[ConsoleLogger] - " + message);
    }
}