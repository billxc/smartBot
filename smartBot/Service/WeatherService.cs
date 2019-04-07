using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.Service
{
    public static class Translator
    {
        private static Dictionary<string, string> dic = new Dictionary<string, string>()
        {
            {"Gales","强风" },
            {"Umbral Wind","灵风" },
            {"Showers","暴雨" },
            {"Clear Skies","晴朗" },
            {"Fog","薄雾" },
            {"Heat Waves","热浪" },
            {"Snow","小雪" },
            {"Thunder","打雷" },
            {"Blizzards","暴雪" },
            // 热浪 暴雪 小雪 晴朗 打雷
            {"Gloom","" },
            {"Thunderstorms","打雷" },
        };

        public static string Translate(this string str)
        {
            return dic[str];
        }
    }

    public class WeatherService
    {

        public string GetSpecificWeatherTimes(string territory, string targetWeather)
        {
            var weatherStartTime = GetWeatherTimeFloor(DateTimeOffset.Now.ToUnixTimeSeconds());
            var count = 5;
            var match = 0;
            var try_time = 0;
            var now_time = weatherStartTime;
            var weatherCal = GetFunction(territory);
            if (weatherCal == null)
            {
                return null;
            }

            var ret = "接下来优雷卡常风之地的强风天气如下：";
            
//            小雪->强风 ET: 16:00 LT: 04 - 04 14:36:40
//            晴朗->强风 ET: 8:00  LT: 04 - 04 15:23:20
//            强风->强风 ET: 16:00 LT: 04 - 04 15:46:40
            while (match < count && try_time <= 1000)
            {
                try_time += 1;
                var chance = CalculateForecastTarget(now_time);
                var weather = weatherCal(chance).Translate();
                var pre_chance = CalculateForecastTarget(now_time - 8 * 175);
                var preWeather = weatherCal(pre_chance).Translate();
                if (weather == targetWeather)
                {
                    match += 1;
                    ret += $"\n{preWeather}->{weather} " +
                           $"ET: {GetEorzeaHour(now_time)}:00 " +
                           $"LT:{DateTimeOffset.FromUnixTimeSeconds(now_time).DateTime}";
                }

                now_time += 8 * 175;
            }

            return ret;

        }

        public Func<int, string> GetFunction(string territory)
        {
            switch (territory)
            {
                case "Anemos":
                    return Anemos;
                case "Pagos":
                    return Pagos;
                case "Pyros":
                    return Pyros;
                case "Hydatos":
                    return Hydatos;
            }

            return null;
        }

        public int GetEorzeaHour(long unixSeconds)
        {
            // Get Eorzea hour
            return (int)(unixSeconds / 175 % 24);
        }

        public long GetWeatherTimeFloor(long unixSeconds)
        {
            // Get Eorzea hour for weather start
            var bell = (unixSeconds / 175) % 24; // hour
            var startBell = bell - (bell % 8); //minute
            var startUnixSeconds = unixSeconds - (175 * (bell - startBell));
            return startUnixSeconds;
        }


        //风
        public string Anemos(int chance)
        {
            if ((chance -= 30) < 0) { return "Clear Skies"; }

            if ((chance -= 30) < 0) { return "Gales"; }

            if ((chance -= 30) < 0) { return "Showers"; }

            return "Snow";
        }

        //冰
        public string Pagos(int chance)
        {
            if ((chance -= 10) < 0) { return "Clear Skies"; }

            if ((chance -= 18) < 0) { return "Fog"; }

            if ((chance -= 18) < 0) { return "Heat Waves"; }

            if ((chance -= 18) < 0) { return "Snow"; }

            if ((chance -= 18) < 0) { return "Thunder"; }

            return "Blizzards";
        }

        //火
        public string Pyros(int chance)
        {
            if ((chance -= 10) < 0) { return "Clear Skies"; }

            if ((chance -= 18) < 0) { return "Heat Waves"; }

            if ((chance -= 18) < 0) { return "Thunder"; }

            if ((chance -= 18) < 0) { return "Blizzards"; }

            if ((chance -= 18) < 0) { return "Umbral Wind"; }

            return "Snow";
        }

        //
        public string Hydatos(int chance)
        {
            if ((chance -= 12) < 0) { return "Fair Skies"; }

            if ((chance -= 22) < 0) { return "Showers"; }

            if ((chance -= 22) < 0) { return "Gloom"; }

            if ((chance -= 22) < 0) { return "Thunderstorms"; }

            return "Snow";
        }


        public static int CalculateForecastTarget(long unixSeconds)
        {
            // I dont know why write this, just
            // Thanks to Rogueadyn's SaintCoinach library for this calculation.

            // Get Eorzea hour for weather start
            var bell = unixSeconds / 175;

            // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
            var increment = (bell + 8 - bell % 8) % 24;

            // Take Eorzea days since unix epoch
            var totalDays = (uint)(unixSeconds / 4200);

            // 0x64 = 100
            var calcBase = totalDays * 100 + increment;

            // 0xB = 11
            var step1 = (uint)((calcBase << 11) ^ calcBase);
            var step2 = (step1 >> 8) ^ step1;

            // 0x64 = 100
            return (int)(step2 % 100);
        }


    }
}
