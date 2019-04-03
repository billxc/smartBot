using System;
using System.Collections.Generic;
using System.Text;
// ReSharper disable InconsistentNaming

namespace smartBot.model
{
    public class CoolqEvent
    {
        public string post_type { get; set; } // message, notice, request
        public long time { get; set; }
        public long self_id { get; set; } // self QQ number
    }

    public class CoolqMessage :CoolqEvent
    {
        public string message_type { get; set; }

        public string sub_type { get; set; }
        //for private chat : friend、group、discuss、other	消息子类型，如果是好友则是 friend，如果从群或讨论组来的临时会话则分别是 group、discuss
        //for group chat : normal、anonymous、notice 系统提示（如「管理员已禁止群内匿名聊天」）是 notice

        public int message_id { get; set; }
        public int user_id { get; set; } // 发送者 QQ 号
        public string message { get; set; } //消息内容
        public string raw_message { get; set; } //消息内容
        public int font { get; set; } //字体
        public Sender sender { get; set; } //发送人信

        public long group_id { get; set; }
        public long discuss_id { get; set; }
        public object anonymous { get; set; } // 发送者 QQ 号
    }

    public class CooqNotice :CoolqEvent
    {
        public string notice_type { get; set; }

    }
    public class CooqRequest :CoolqEvent
    {
        public string request_type { get; set; }

    }

    public class Sender
    {
        public long user_id { get; set; }
        public string nickname { get; set; }
        public string card { get; set; } //昵称
        public string sex { get; set; } //性别，male 或 female 或 unknown
        public int age { get; set; }
        public string area { get; set; }
        public string level { get; set; }
        public string role { get; set; } // 角色，owner 或 admin 或 member
        public string title { get; set; } // 专属头衔
    }

}
