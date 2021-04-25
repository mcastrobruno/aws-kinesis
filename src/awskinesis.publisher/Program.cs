using awskinesis.shared.Kinesis;
using awskinesis.shared.logger;
using System;
using System.Threading;

namespace awskinesis.publisher
{
    public class TestMessage
    {
        public Guid Id { get; set; }
        public string Message { get; set; }

        public TestMessage(string message = "Hello World!")
        {
            Id = Guid.NewGuid();
            Message = message;
        }
    }

    public class Program
    {
        private static KinesisPublisher<TestMessage> _publisher;

        static Program()
        {
            _publisher = new KinesisPublisher<TestMessage>();
        }

        static async System.Threading.Tasks.Task Main(string[] args)
        {
            Logger.Log("Starting Kinesis Publisher...");

            while (true)
            {
                Logger.Log("Publishing to kinesis...");

                try
                {
                    await _publisher.PublishAsync(new TestMessage());
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
