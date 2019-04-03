using NUnit.Framework;
using smartBot.Service;
using System.Threading.Tasks;

namespace Tests
{
    public class Tests
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
    }
}