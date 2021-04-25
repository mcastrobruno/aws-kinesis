using Amazon.Kinesis;
using Amazon.Kinesis.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace awskinesis.shared.Kinesis
{
    public class KinesisConsumer
    {
        private Dictionary<string, string> _shardsPosition = new Dictionary<string, string>();
        private readonly IAmazonKinesis _kinesisClient = KinesisClientFactory.CreateClient();

        public async Task StartAsync(string streamName)
        {
            var describeRequest = new DescribeStreamRequest
            {
                StreamName = streamName
            };

            var describeResponse = await _kinesisClient.DescribeStreamAsync(describeRequest);

            var shards = describeResponse.StreamDescription.Shards;

            foreach (var shard in shards)
            {
                var iteratorRequest = new GetShardIteratorRequest
                {
                    StreamName = streamName,
                    ShardId = shard.ShardId,
                    ShardIteratorType = ShardIteratorType.LATEST
                };

                var iteratorResponse = await _kinesisClient.GetShardIteratorAsync(iteratorRequest);

                var iteratorId = iteratorResponse.ShardIterator;

                if (!string.IsNullOrEmpty(iteratorId))
                {
                    _shardsPosition.Add(shard.ShardId, iteratorId);
                }
            }
        }

        public async Task<IEnumerable<TOutput>> ReadNext<TOutput>(int limit = 1000)
        {
            var response = new List<TOutput>();

            for (int i = 0; i < _shardsPosition.Keys.Count; i++)
            {
                var shard = _shardsPosition.ToList()[i];

                if (string.IsNullOrEmpty(shard.Value))
                {
                    continue;
                }

                var getRequest = new GetRecordsRequest
                {
                    Limit = limit,
                    ShardIterator = shard.Value
                };

                var getResponse = await _kinesisClient.GetRecordsAsync(getRequest);

                var records = getResponse.Records;

                if (records.Count > 0)
                {
                    foreach (var record in records)
                    {
                        response.Add(JsonConvert.DeserializeObject<TOutput>(Encoding.UTF8.GetString(record.Data.ToArray())));
                    }
                }
                _shardsPosition[shard.Key] = getResponse.NextShardIterator;
            }

            return response;
        }

    }
}
