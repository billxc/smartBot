using smartBot.manager;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.manager
{
    class Dps : SimpleCommandAddOn
    {
        public override string Info()
        {
            return "dps:查询国际服同期DPS排名，例：/dps 8s 骑士";
        }

        public override string onMessage(string input)
        {
            throw new NotImplementedException();
        }
    }
}
