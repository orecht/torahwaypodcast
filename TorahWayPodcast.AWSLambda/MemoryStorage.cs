using Amazon.Lambda.Core;
using TorahWayPodcast.Models;
using TorahWayPodcast.Storage;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace TorahWayPodcast.AWSLambda
{
    public partial class Function
    {
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

    }
}
