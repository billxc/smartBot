using System;
using NUnit.Framework;
using smartBot.Service;
using System.Threading.Tasks;
using smartBot.manager;

namespace smartBotTest
{
    public class WeatherTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCalculateForecastTarget()
        {
            Assert.AreEqual(56,WeatherService.CalculateForecastTarget(1000));
            Assert.AreEqual(65, WeatherService.CalculateForecastTarget(1554368739));
            Assert.AreEqual(59, WeatherService.CalculateForecastTarget(1554348739));
            Assert.AreEqual(91, WeatherService.CalculateForecastTarget(1554347659));
        }

        [Test]
        public void TestTranslatorComplete()
        {
            var ws = new WeatherService();
            for (var i = 0; i <= 100; i++)
            {
                Console.WriteLine(ws.Anemos(i).Translate());
                Console.WriteLine(ws.Pagos(i).Translate());
                Console.WriteLine(ws.Pyros(i).Translate());
                Console.WriteLine(ws.Hydatos(i).Translate());
            }
        }

        [Test]
        public void TestAnemos()
        {
            var ws = new WeatherService();
            Console.WriteLine(ws.GetSpecificWeatherTimes("Anemos", "Ç¿·ç"));
        }
    }
}