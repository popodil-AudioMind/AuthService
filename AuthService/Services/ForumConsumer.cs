using Audiomind.RabbitMQ.Moddels;
using AuthService.Data;
using MassTransit;

namespace Audiomind.RabbitMQ
{
    public class ForumConsumer : IConsumer<ForumMessage>
    {
        private ISqlLoginUser _sqlUser;
        public ForumConsumer(ISqlLoginUser sqlUser) 
        { 
            _sqlUser = sqlUser;
        }
        public Task Consume(ConsumeContext<ForumMessage> context)
        {
            _sqlUser.AddForumToUser(context.Message);
            return Task.CompletedTask;
        }
    }
}
