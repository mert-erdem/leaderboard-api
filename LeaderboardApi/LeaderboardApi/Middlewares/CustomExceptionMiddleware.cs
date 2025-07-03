using System.Diagnostics;
using System.Net;
using LeaderboardApi.Services.Loggers;
using Newtonsoft.Json;

namespace LeaderboardApi.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILoggerService _logger;

    public CustomExceptionMiddleware(RequestDelegate next, ILoggerService logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        
        try
        {
            stopwatch.Start();
        
            string message = "[Request] HTTP " + context.Request.Method + " " + context.Request.Path;
            _logger.Log(message);
        
            await _next(context);
        
            stopwatch.Stop();
        
            message = "[Response] HTTP " + context.Request.Method + " " + context.Request.Path + " " + 
                      context.Response.StatusCode + " in " + stopwatch.ElapsedMilliseconds + " ms";
            _logger.Log(message);
        }
        catch (Exception e)
        {
            stopwatch.Stop();
            await HandleException(context, e, stopwatch);
        }
    }
    
    private Task HandleException(HttpContext context, Exception e, Stopwatch stopwatch)
    {
        var errorMessage = "[Error] HTTP " + context.Request.Method + " " + context.Response.StatusCode + 
                           " Error Message:" + e.Message + " in " + stopwatch.ElapsedMilliseconds + " ms";
        _logger.Log(errorMessage);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        
        var result = JsonConvert.SerializeObject(new { message = errorMessage }, Formatting.None);
        
        return context.Response.WriteAsync(result);
    }
}