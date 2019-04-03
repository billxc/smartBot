using NUnit.Framework;
using smartBot.Service;

namespace Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestLoadClassData()
        {
            var dpsService = DpsService.Instance;
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("��ħ"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("��ħ��ʦ"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("BLM"));
            Assert.AreEqual(7, dpsService.GetClassIdByAlias("��ɮ"));
            Assert.AreEqual(-1, dpsService.GetClassIdByAlias(""));
        }
    }
}