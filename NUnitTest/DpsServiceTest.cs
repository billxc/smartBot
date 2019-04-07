using NUnit.Framework;
using smartBot.Service;
using System.Threading.Tasks;

namespace smartBotTest
{
    public class DepServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task TestLoadClassDataAsync()
        {
            var dpsService = DpsService.Instance;
            await dpsService.LoadClassRemotely();
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("ºÚÄ§"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("ºÚÄ§·¨Ê¦"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("BLM"));
            Assert.AreEqual(7, dpsService.GetClassIdByAlias("ÎäÉ®"));
            Assert.AreEqual(-1, dpsService.GetClassIdByAlias(""));
        }
    }
}