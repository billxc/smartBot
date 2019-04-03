using System;
using System.Collections.Generic;
using System.Text;
using smartBot.core;
using smartBot.model;

namespace smartBot.Addon
{
    public interface IBotAddOn
    {
        void Action(Bot bot, CoolqEvent message);
        string Info();
    }
}
