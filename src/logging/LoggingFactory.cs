using Microsoft.Extensions.Logging;

namespace VontobelTest.src.logging
{
    public interface ILoggingFactory
    {
        ILogger<T> CreateLogger<T>();
    }

    public class LoggingFactory : ILoggingFactory
    {
        private static readonly ILoggerFactory _loggerFactory = LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        });

        public ILogger<T> CreateLogger<T>()
        {
            return _loggerFactory.CreateLogger<T>();
        }

    }
}