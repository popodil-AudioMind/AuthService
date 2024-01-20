using Audiomind.RabbitMQ.Moddels;
using TestProject.Models;

namespace TestProject
{
    [TestClass]
    public class TestLoginUser
    {
        private StringGenerator _generator;
        [TestMethod]
        public void Create()
        {
            _generator = new StringGenerator();
            string id = Guid.NewGuid().ToString();
            string displayName = _generator.GenerateString(10);
            CreateMessage message = new CreateMessage() { id = id, displayName = displayName };

            Assert.Equals(message.id, id);
            Assert.Equals(message.displayName, displayName);
        }
    }
}