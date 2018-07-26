using System;
using Amazon.Lambda.Core;

using TorahWayPodcast.BO;

namespace TorahWayPodcast.AWSLambda
{
    public partial class Function
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
