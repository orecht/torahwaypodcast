using System;
using Amazon.Lambda.Core;

using TorahWayPodcast.BO;
using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TorahWayPodcast.AWSLambda
{
    public class Function
    {
        public class LambdaLoggerAdapter : ILogger
        {
            private ILambdaLogger _lambdaLogger;

            public LambdaLoggerAdapter(ILambdaLogger lambdaLogger)
            {
                _lambdaLogger = lambdaLogger ?? throw new ArgumentNullException(nameof(lambdaLogger));
            }

            public void Log(string message)
            {
                _lambdaLogger.LogLine(message);
            }
        }

        public class MemoryStorage : IStorage<Shiurim>
        {
            private Shiurim _shiurim;

            public Shiurim Read()
            {
                return _shiurim;
            }

            public void Write(Shiurim shiurim)
            {
                _shiurim = shiurim;
            }
        }

        /// <summary>
        /// Parse the torah way website and writes the extracted data to file
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
            var storage = new MemoryStorage();

            var manager = new PodcastManager(storage, new LambdaLoggerAdapter(context.Logger));
            manager.ParseHtml();

            return "ParseHtml() OK";
        }

    }
}
