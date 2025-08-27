using Microsoft.Extensions.Logging;

namespace BlazorHRManagement.Api.Logger;

public class FileLoggerProvider : ILoggerProvider
{
    public ILogger CreateLogger(string categoryName)
    {
        return new FileLogger();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}
public class FileLogger : ILogger
{
    private readonly string logPath;
    private readonly int maxFielSize;
    private readonly LogLevel logLevel;
    private readonly string categoryName;

    public FileLogger(IConfiguration configuration,Func<string,bool> logLevelFunc,string categoryName)
    {
        logPath = configuration["Logging:File:MaxFileSize"];
        maxFielSize = configuration.GetValue<int>("Logging:File:MaxFileSize");
        
        string logLeveLString = configuration["Logging:LogLevel:Default"];
        Enum.TryParse<LogLevel>(logLeveLString, out logLevel);
        this.logLevelFunc = logLevelFunc;
        this.categoryName = categoryName;
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        throw new NotImplementedException();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
      if( logLevel < this.logLevel)
            return true;
        return logLevelFunc(categoryName);
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {

        }
    }
}
