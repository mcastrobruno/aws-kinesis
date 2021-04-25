using Amazon.Kinesis;
using awskinesis.shared.Kinesis;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AwsKinesis.IntegrationTests.Fixture
{
    public class KinesisFixture : IAsyncLifetime
    {
        private IAmazonKinesis _kinesisClient;
        public async Task InitializeAsync()
        {
            _kinesisClient = KinesisClientFactory.CreateClient();
            await _kinesisClient.CreateStreamAsync(new Amazon.Kinesis.Model.CreateStreamRequest
            {
                ShardCount = 1,
                StreamName = "dev-kinesis"
            });

            await Task.Delay(1000); //awaits 1 second until the stream is created.
        }

        public async Task DisposeAsync()
        {
            await _kinesisClient.DeleteStreamAsync(new Amazon.Kinesis.Model.DeleteStreamRequest
            {
                StreamName = "dev-kinesis"
            });
        }
    }
}
