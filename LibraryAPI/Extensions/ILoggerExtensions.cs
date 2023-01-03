using Serilog;
using Serilog.Context;

public static class SerilogExtensions
{
    public static Microsoft.Extensions.Logging.ILogger SetSessionID(this Serilog.ILogger logger, string sessionID)
    {
        //DOESNT WORK YET
        LogContext.PushProperty("SessionID", sessionID);
        return (Microsoft.Extensions.Logging.ILogger) logger.ForContext("SessionID", sessionID);
    }
}
