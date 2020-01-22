using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Primitives;

namespace BookReadingRepositoryUnitTest
{
    public class TestHelper
    {
        public static ILogger CreateLogger(LoggerTypes type = LoggerTypes.Null)
        {
            ILogger logger;

            if (type == LoggerTypes.List)
            {
                logger = new ListLogger();
            }
            else
            {
                logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            }

            return logger;
        }

        public static HttpRequest RequestWithFileInForm(Dictionary<string, StringValues> formData)
        {
            var stream = File.OpenRead("..\\..\\..\\TestUploadFile.txt");
            var file = new FormFile(stream, 0, stream.Length, "file", Path.GetFileName(stream.Name));

            var httpContext = new DefaultHttpContext();

            httpContext.Request.Form = new FormCollection(formData, new FormFileCollection { file });
            var request = new DefaultHttpRequest(httpContext);

            string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
            request.ContentType = "multipart/form-data; boundary=" + boundary;

            return request;
        }
    }

    public class NullScope : IDisposable
    {
        public static NullScope Instance { get; } = new NullScope();
        private NullScope() { }
        public void Dispose() { }
    }

    public class ListLogger : ILogger
    {
        public IList<string> Logs;

        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        public bool IsEnabled(LogLevel logLevel) => false;

        public ListLogger()
        {
            this.Logs = new List<string>();
        }

        public void Log<TState>(LogLevel logLevel,
                                EventId eventId,
                                TState state,
                                Exception exception,
                                Func<TState, Exception, string> formatter)
        {
            string message = formatter(state, exception);
            this.Logs.Add(message);
        }
    }

    public enum LoggerTypes
    {
        Null,
        List
    }
}
