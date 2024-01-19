using MassTransit;

namespace Audiomind.RabbitMQ
{
    public class RabbitMQProducer : IMessageProducer
    {
        public void SendMessage<T>(T message, IPublishEndpoint publishEndPoint)
        {
            publishEndPoint.Publish(message);
        }
    }
}
