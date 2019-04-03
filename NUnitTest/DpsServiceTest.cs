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
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("ºÚÄ§"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("ºÚÄ§·¨Ê¦"));
            Assert.AreEqual(3, dpsService.GetClassIdByAlias("BLM"));
            Assert.AreEqual(7, dpsService.GetClassIdByAlias("ÎäÉ®"));
            Assert.AreEqual(-1, dpsService.GetClassIdByAlias(""));
        }
    }
}