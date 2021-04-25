using System.Threading.Tasks;

namespace awskinesis.shared.Kinesis
{
    public interface IKinesisPublisher<TInput> where TInput : class
    {
        Task PublishAsync(TInput input);
    }
}