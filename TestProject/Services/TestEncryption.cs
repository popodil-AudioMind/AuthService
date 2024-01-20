using Audiomind.RabbitMQ.Moddels;
using AuthService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject.Services
{
    [TestClass]
    public class TestEncryption
    {
        EncryptionService service = new EncryptionService();
        [TestMethod]
        public void Generate()
        {
            string unhashedPassword = "JustAPassword";
            string hashedPassword = service.Hash(unhashedPassword);

            bool verify = service.Verify(unhashedPassword, hashedPassword);
            Assert.IsTrue(verify);
        }
    }
}
