using smartBot.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.manager  .simpleaddon
{
    class Test : SimpleCommandAddOn
    {
        public override string Info()
        {
            return "";
        }

        public override string onMessage(string input)
        {
            if (input.StartsWith("GetBossIdByAlias"))
            {
                return DpsService.Instance.GetBossIdByAlias(input.Split(" ")[1]).ToString();
            }
            if (input.StartsWith("GetClassIdByAlias"))
            {
                return DpsService.Instance.GetClassIdByAlias(input.Split(" ")[1]).ToString();
            }
            return null;
        }
    }
}
