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

    }
}
