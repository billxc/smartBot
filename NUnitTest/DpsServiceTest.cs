using NUnit.Framework;
using smartBot.manager;
using smartBot.Service;
using System;
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
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("��ħ"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("��ħ��ʦ"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("BLM"));
            Assert.AreEqual(7, dpsService.GetClassIdByAlias("��ɮ"));
            Assert.AreEqual(-1, dpsService.GetClassIdByAlias(""));
        }

        [Test]
        public async Task TestCrawl()
        {
            var d = new Dps();
            d.CrawlDpsListAsync(21, 51, "Bard", -1).Wait();
        }



        [Test]
        public async Task TestDPS()
        {
            var d = new Dps();
            Console.WriteLine(d.onMessage("dps ��ȸ BlackMage"));
        }
    }
}