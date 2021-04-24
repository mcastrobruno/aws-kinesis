using Amazon.Kinesis;
using awskinesis.shared.Kinesis;
using awskinesis.shared.logger;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace awskinesis.publisher
{
    public class Program
    {
        private static readonly IAmazonKinesis _kinesisClient;
        static Program()
        {
            _kinesisClient = KinesisClientFactory.CreateClient();
        }
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Logger.Log("Starting Kinesis Publisher...");

            while (true)
            {
                Logger.Log("Publishing to kinesis...");

                try
                {
                    var response = await _kinesisClient.PutRecordAsync(new Amazon.Kinesis.Model.PutRecordRequest
                    {
                        PartitionKey = "dev-01",
                        StreamName = "dev-kinesis",
                        Data = new MemoryStream(Encoding.UTF8.GetBytes("This is a test message"))
                    });

                    Logger.Log($"Response: {response.HttpStatusCode}");

                }
                catch (Exception e)
                {
                    Logger.LogError($"Publishing failed", e);
                }
                finally
                {
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
