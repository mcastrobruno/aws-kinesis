using awskinesis.shared.Kinesis;
using awskinesis.shared.logger;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace awskinesis.consumer
{
    public class Program
    {
        private readonly static KinesisConsumer _kinesisConsumer;
        static Program()
        {
            _kinesisConsumer = new KinesisConsumer();
        }

        static async Task Main(string[] args)
        {
            Logger.Log("Starting Kinesis Consumer...");

            await _kinesisConsumer.StartAsync("dev-kinesis");

            Logger.Log("Kinesis Consumer Started.");

            while (true)
            {
                var response = await _kinesisConsumer.ReadNext<dynamic>(100);
                Logger.Log(JsonConvert.SerializeObject(response));

                await Task.Delay(5000);
            }
        }

    }
}
