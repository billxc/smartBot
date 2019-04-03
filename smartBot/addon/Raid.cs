using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.manager
{
    class Raid : SimpleCommandAddOn
    {
        public override string Info()
        {
            return "/raid: 查询零式英雄榜，例如 raid 蓝色裂痕 萌芽池";
        }

        public override string onMessage(string input)
        {
            throw new NotImplementedException();
        }
    }
}
