using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using awskinesis.shared.Kinesis;
using awskinesis.shared.logger;
using System;
using System.Collections.Generic;
using System.Text;

namespace awskinesis.consumer
{
    public class Program
    {
        private readonly static IAmazonKinesis _kinesisClient;
        static Program()
        {
            _kinesisClient = KinesisClientFactory.CreateClient();
        }

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            string kinesisStreamName = "dev-kinesis";

            Logger.Log("Starting Kinesis Consumer...");

            var describeRequest = new DescribeStreamRequest
            {
                StreamName = kinesisStreamName
            };

            DescribeStreamResponse describeResponse = await _kinesisClient.DescribeStreamAsync(describeRequest);
            List<Shard> shards = describeResponse.StreamDescription.Shards;

            foreach (Shard shard in shards)
            {
                GetShardIteratorRequest iteratorRequest = new GetShardIteratorRequest
                {
                    StreamName = kinesisStreamName,
                    ShardId = shard.ShardId,
                    ShardIteratorType = ShardIteratorType.TRIM_HORIZON
                };

                GetShardIteratorResponse iteratorResponse = await _kinesisClient.GetShardIteratorAsync(iteratorRequest);
                string iteratorId = iteratorResponse.ShardIterator;

                while (!string.IsNullOrEmpty(iteratorId))
                {
                    GetRecordsRequest getRequest = new GetRecordsRequest
                    {
                        Limit = 1000,
                        ShardIterator = iteratorId
                    };

                    GetRecordsResponse getResponse = await _kinesisClient.GetRecordsAsync(getRequest);
                    string nextIterator = getResponse.NextShardIterator;
                    List<Record> records = getResponse.Records;

                    if (records.Count > 0)
                    {
                        Console.WriteLine("Received {0} records. ", records.Count);
                        foreach (Record record in records)
                        {
                            string json = Encoding.UTF8.GetString(record.Data.ToArray());
                            Console.WriteLine("Json string: " + json);
                        }
                    }
                    iteratorId = nextIterator;
                }
            }
        }
    }
}
