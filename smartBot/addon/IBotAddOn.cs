using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.addon
{
    public interface IBotAddOn
    {
        bool ShouldAction(string message);
        void Action(string message);

    }
}
