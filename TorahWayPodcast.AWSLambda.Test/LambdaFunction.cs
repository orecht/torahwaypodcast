using System.Collections.Generic;
using System.Text;
using Amazon.Lambda.Core;
using NUnit.Framework;
using TorahWayPodcast.AWSLambda;

namespace TorahWayPodcast.AWSLambda.Test
{
    [TestFixture]
    public class LambdaFunction
    {
        [Test]
        public void RunFunction()
        {
            ILambdaContext context = new FakeLambdaContext();

            new Function().FunctionHandler(context);
        }
    }
}
