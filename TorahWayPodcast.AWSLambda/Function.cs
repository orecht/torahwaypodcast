using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Amazon;
using Amazon.Lambda.Core;
using Amazon.S3;
using Amazon.S3.Model;
using TorahWayPodcast.BO;
using TorahWayPodcast.BO2;
using TorahWayPodcast.Models;

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

        public class NilLogger : ILogger
        {
            public void Log(string message)
            {
                // do nothing
            }
        }

        /// <summary>
        /// Parse the torah way website and writes the extracted data to file
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task FunctionHandler(ILambdaContext context)
        {
            ILogger logger = new LambdaLoggerAdapter(context.Logger);

            logger.Log("Before start");

            try
            {
                var storageShiurum = new MemoryStorage();
                logger.Log("storageShiurum created");

                // Gather shiurim list from torah way website
                var manager = new PodcastManager(storageShiurum, new NilLogger()); // no logging
                logger.Log("PodcastManager created.");
                logger.Log("Running manager.ParseHtml()");
                manager.ParseHtml();
                logger.Log("END running manager.ParseHtml()");

                // Generate RSS feed text
                var shiurim = manager.Rss2();
                var model = new ShiurimViewModel()
                {
                    Shiurim = new Shiurim(shiurim),
                    RequestUri = new Uri("http://www.torahwaypodcast.org.uk")
                };

                var dir = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);
                var viewFile = "rss2.cshtml";

                logger.Log("Running manager.GenerateRssFeedAsync");
                var generatedRss = await manager.GenerateRssFeedAsync(model, dir, viewFile);
                logger.Log("END Running manager.GenerateRssFeedAsync");
                //logger.Log(generatedRss);

                // Save RRS feed text to file in S3
                RegionEndpoint bucketRegion = RegionEndpoint.EUWest1;
                var client = new AmazonS3Client(bucketRegion); // TODO. Add credentials or access token for authentication
                var putRequest1 = new PutObjectRequest
                {
                    BucketName = "torahwaypodcast",
                    Key = "rss.xml",
                    ContentBody = generatedRss,
                    ContentType = "application/xml"
                };

                logger.Log("Running PutObjectAsync");
                PutObjectResponse response1 = await client.PutObjectAsync(putRequest1);
                logger.Log("END Running PutObjectAsync");
                logger.Log($"response1.HttpStatusCode: {response1.HttpStatusCode}");
            }
            catch (Exception e)
            {
                string message = $"Failed: {e.Message} at {e.StackTrace}";
                logger.Log(message);
            }

            return;
        }

    }
}
