using Amazon.Kinesis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace awskinesis.shared.Kinesis
{
    public class KinesisPublisher<TInput> where TInput : class
    {
        private readonly IAmazonKinesis _kinesisClient;

        public KinesisPublisher()
        {
            _kinesisClient = KinesisClientFactory.CreateClient();
        }

        public Task PublishAsync(TInput input)
        {
            var task = _kinesisClient.PutRecordAsync(new Amazon.Kinesis.Model.PutRecordRequest
            {
                PartitionKey = "dev-01",
                StreamName = "dev-kinesis",
                Data = new MemoryStream(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(input)))
            });

            return task;
        }
    }
}
