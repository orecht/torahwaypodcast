using System;
using System.Reflection;
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
        public string FunctionHandler(ILambdaContext context)
        {
            ILogger logger = new LambdaLoggerAdapter(context.Logger);

            logger.Log("Before start");

            try
            {
                var storageShiurum = new MemoryStorage();

                logger.Log("storageShiurum created");

                // Gather shiurim list from torah way website
                var manager = new PodcastManager(storageShiurum, logger);
                logger.Log("PodcastManager created.");
                logger.Log("Running manager.ParseHtml()");
                manager.ParseHtml();
                logger.Log("END running manager.ParseHtml()");

                var dir = Assembly.GetExecutingAssembly().CodeBase;
                var viewFile = "rss2.cshtml";

                // Write RSS feed
                logger.Log("Running manager.GenerateRssFeedAsync");
                var resultTask = manager.GenerateRssFeedAsync(manager.Rss2(), dir, viewFile);
                logger.Log("END Running manager.GenerateRssFeedAsync");
                var result = resultTask.Result;

                return "OK";
            }
            catch (Exception e)
            {
                string message = $"Failed: {e.Message} at {e.StackTrace}";
                logger.Log(message);
                return message;
            }
        }

    }
}
