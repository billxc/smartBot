using System;
using System.Collections.Generic;
using System.Text;
using smartBot.core;
using smartBot.model;

namespace smartBot.Addon
{
    class Echo : SimpleCommandAddOn
    {
        public static readonly Echo Instance = new Echo();

        public override string Info()
        {
            return @"复读机";
        }

        public override string onMessage(string input)
        {
            return input;
        }
    }
}
