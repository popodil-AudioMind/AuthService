using Audiomind.RabbitMQ.Moddels;
using AuthService.Interfaces;
using AuthService.Models;
using System.Data;
using TestProject.Models;

namespace TestProject
{
    [TestClass]
    public class TestILoginUser
    {
        private StringGenerator _generator;
        [TestMethod]
        public void Create()
        {
            _generator = new StringGenerator();
            string id = "143254";
            string username = _generator.GenerateString(10);
            string hashedPassword = _generator.GenerateString(15);
            string role = "user";
            string forumIds = "id1,id2,id3";
            ILoginUser iUser = new()
            {
                id = id,
                username = username,
                hashedPassword = hashedPassword,
                role = role,
                forumIDs = forumIds
            };

            Assert.Equals(iUser.id, id);
            Assert.Equals(iUser.username, username);
            Assert.Equals(iUser.hashedPassword, hashedPassword);
            Assert.Equals(iUser.role, role);
            Assert.Equals(iUser.forumIDs, forumIds);
        }

        [TestMethod]
        public void Convert()
        {
            _generator = new StringGenerator();
            Guid id = Guid.Parse("143254");
            string username = _generator.GenerateString(10);
            string hashedPassword = _generator.GenerateString(15);
            string role = "user";
            string forumIds = "id1,id2,id3";
            List<Guid> forumIDs = new();
            foreach (string item in forumIds.Split(',').ToList())
            {
                if (item != "")
                    forumIDs.Add(Guid.Parse(item));
            }


            LoginUser user = new()
            {
                id = id,
                username = username,
                hashedPassword = hashedPassword,
                role = role,
                forumIDs = forumIDs
            };

            ILoginUser iUser = new(user);

            Assert.Equals(iUser.id, id);
            Assert.Equals(iUser.username, username);
            Assert.Equals(iUser.hashedPassword, hashedPassword);
            Assert.Equals(iUser.role, role);
            Assert.Equals(iUser.forumIDs, forumIds);
        }
    }
}