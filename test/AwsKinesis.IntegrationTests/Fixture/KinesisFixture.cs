using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using awskinesis.shared.Kinesis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AwsKinesis.IntegrationTests.Fixture
{
    public class KinesisFixture : IAsyncLifetime
    {
        private IAmazonKinesis _kinesisClient;
        public KinesisConsumer KinesisConsumer { get; private set; }

        public async Task InitializeAsync()
        {
            _kinesisClient = KinesisClientFactory.CreateClient();

            await DeleteStreamIfExits("dev-kinesis");

            await _kinesisClient.CreateStreamAsync(new Amazon.Kinesis.Model.CreateStreamRequest
            {
                ShardCount = 1,
                StreamName = "dev-kinesis"
            });

            await Task.Delay(1000); //awaits 1 second until the stream is created.

            KinesisConsumer = new KinesisConsumer();
            await KinesisConsumer.StartAsync("dev-kinesis");
        }

        public async Task DisposeAsync()
        {
            await DeleteStreamIfExits("dev-kinesis");
        }

        private async Task DeleteStreamIfExits(string streamName)
        {
            try
            {
                var response = await _kinesisClient.DescribeStreamAsync(new DescribeStreamRequest
                {
                    StreamName = streamName
                });

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    await _kinesisClient.DeleteStreamAsync(new DeleteStreamRequest
                    {
                        StreamName = "dev-kinesis",
                        EnforceConsumerDeletion = true
                    });
                }
            }
            catch (ResourceNotFoundException)
            {
                Debug.WriteLine("Stream does not exists. Skipping deletion.");
            }


        }
    }
}
