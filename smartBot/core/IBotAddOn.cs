using System;
using System.Collections.Generic;
using System.Text;
using smartBot.manager;
using smartBot.model;

namespace smartBot.manager
{
    public interface IBotAddOn
    {
        void Action(Bot bot, CoolqEvent message);
        string Info();
    }
}
