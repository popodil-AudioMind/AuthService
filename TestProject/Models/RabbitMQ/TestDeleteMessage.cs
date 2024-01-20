using Audiomind.RabbitMQ.Moddels;

namespace TestProject
{
    [TestClass]
    public class TestDeleteMessage
    {
        [TestMethod]
        public void Create()
        {
            string id = Guid.NewGuid().ToString();
            DeleteMessage message = new DeleteMessage() { id = id };

            Assert.Equals(message.id, id);
        }
    }
}