using Amazon.Kinesis;
using Amazon.Runtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace awskinesis.shared.Kinesis
{
    public class KinesisClientFactory
    {
        public static IAmazonKinesis CreateClient(string serviceUrl = "http://localhost:4566")
        {
            var credentials = new BasicAWSCredentials("dev", "dev");
            var kinesisConfig = new AmazonKinesisConfig
            {
                ServiceURL = serviceUrl
            };

            return new AmazonKinesisClient(credentials, kinesisConfig);
        }
    }
}
