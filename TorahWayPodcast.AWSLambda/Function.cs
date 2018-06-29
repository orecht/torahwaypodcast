using Amazon.Lambda.Core;

using TorahWayPodcast.BO;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TorahWayPodcast.AWSLambda
{
    public class Function
    {
        
        /// <summary>
        /// Parse the torah way website and writes the extracted data to file
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public string FunctionHandler(string input, ILambdaContext context)
        {
            var manager = new PodcastManager();
            return manager.ParseHtml();
        }

    }
}
