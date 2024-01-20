using Audiomind.RabbitMQ.Moddels;

namespace TestProject
{
    [TestClass]
    public class TestForumMessage
    {
        [TestMethod]
        public void Create()
        {
            string userId = Guid.NewGuid().ToString();
            string forumId = Guid.NewGuid().ToString();
            ForumMessage message = new ForumMessage() { userId = userId, forumId = forumId };

            Assert.Equals(message.userId, userId);
            Assert.Equals(message.forumId, forumId);
        }
    }
}