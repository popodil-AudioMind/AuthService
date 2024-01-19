using MassTransit;

namespace Audiomind.RabbitMQ
{
    public interface IMessageProducer
    {
        void SendMessage<T>(T message, IPublishEndpoint publishEndPoint);
    }
}
