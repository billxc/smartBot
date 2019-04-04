using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.manager
{
    class Weather : SimpleCommandAddOn
    {
        public override string Info()
        {
            return "weather: ";
        }

        public override string onMessage(string input)
        {
            return null;

        }

        private int CalculateForecastTarget()
        {
            var unixSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
            // Get Eorzea hour for weather start
            var bell = unixSeconds / 175;

            // Do the magic 'cause for calculations 16:00 is 0, 00:00 is 8 and 08:00 is 16
            var increment = (bell + 8 - bell % 8) % 24;

            // Take Eorzea days since unix epoch
            var totalDays = (uint) (unixSeconds / 4200);

            // 0x64 = 100
            var calcBase = totalDays * 100 + increment;

            // 0xB = 11
            var step1 = (calcBase << 11) ^ calcBase;
            var step2 = ((uint)step1 >> 8) ^ step1;

            // 0x64 = 100
            return (int) (step2 % 100);
        }
    }
}
