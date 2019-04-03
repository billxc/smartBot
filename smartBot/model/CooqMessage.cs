using System;
using System.Collections.Generic;
using System.Text;

namespace smartBot.model
{
    class CoolqEvent
    {
        public string post_type { get; set; } // message, notice, request
        public long time { get; set; }
        public long self_id { get; set; } // self QQ number
    }

    class PrivateSender
    {
        public long user_id { get; set; }
        public string nickname { get; set; }
        public string sex { get; set; } //性别，male 或 female 或 unknown
        public int age { get; set; }
    }
    class PrivateChatEvent : CoolqEvent
    {
        public string message_type { get; set; }
        public string sub_type { get; set; }
        //friend、group、discuss、other	消息子类型，如果是好友则是 friend，如果从群或讨论组来的临时会话则分别是 group、discuss
        public int message_id { get; set; }
        public int user_id { get; set; } // 发送者 QQ 号
        public string message { get; set; } //消息内容
        public string raw_message { get; set; } //消息内容
        public int font { get; set; } //字体
        public PrivateSender sender { get; set; } //发送人信
    }


    class GroupChatEvent : CoolqEvent
    {
        public string message_type { get; set; }
        public string sub_type { get; set; }
        //friend、group、discuss、other	消息子类型，如果是好友则是 friend，如果从群或讨论组来的临时会话则分别是 group、discuss
        public int message_id { get; set; }
        public int group_id { get; set; }
        public int user_id { get; set; } // 发送者 QQ 号
        public object anonymous { get; set; } // 发送者 QQ 号
        public string message { get; set; } //消息内容
        public string raw_message { get; set; } //消息内容
        public int font { get; set; } //字体
        public PrivateSender sender { get; set; } //发送人信
    }

    class GroupSender
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
