using Audiomind.RabbitMQ.Moddels;

namespace TestProject
{
    [TestClass]
    public class TestCreateMessage
    {
        [TestMethod]
        public void Create()
        {
            Random rnd = new Random();
            string id = Guid.NewGuid().ToString();
            string displayName = GenerateString(10,rnd);
            CreateMessage message = new CreateMessage() { id = id, displayName = displayName };

            Assert.Equals(message.id, id);
            Assert.Equals(message.displayName, displayName);
        }

        private string GenerateString(int length, Random random)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}