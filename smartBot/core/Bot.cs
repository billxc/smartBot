using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Fleck;
using Newtonsoft.Json;
using NLog;
using smartBot.Addon;
using smartBot.model;

namespace smartBot.core
{
    public class Bot
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public readonly long QQ;
        public readonly IWebSocketConnection Socket;
        public readonly List<IBotAddOn> AddOnList;

        public Bot(long qq, IWebSocketConnection socket)
        {
            QQ = qq;
            Socket = socket;
            AddOnList = new List<IBotAddOn>();
            LoadAddons();
        }

        public void LoadAddons()
        {
            AddOnList.Add(Nuannuan.Instance);
            Logger.Debug("load nuannuan");

            AddOnList.Add(Nuannuan.Instance);
            //load core addon
        }

        public void AddOnAction(CoolqEvent coolqEvent)
        {
            foreach(var addon in AddOnList)
            {
                addon.Action(this,coolqEvent);
            }
        }


        public void Action(string eventJson)
        {
            Logger.Debug($"receive event string {eventJson}");
            var cooqEvent = JsonConvert.DeserializeObject<CoolqEvent>(eventJson);
            if (cooqEvent.post_type == null)
            {
                return;
            }
            switch (cooqEvent.post_type)
            {
                case "message":
                    var message = JsonConvert.DeserializeObject<CoolqMessage>(eventJson);
                    switch (message.message_type)
                    {
                        case "private":
                        case "group":
                            Logger.Debug($"receive {message.message_type} msg from {message.user_id}," +
                                         $"message is {message.message}, group id is {message.group_id}, " +
                                         $"sender nickname is {message.sender.nickname}");
                            AddOnAction(message);
                            break;
                    }
                    break;
                case "notice":
                //notice
                case "request":
                //request
                default:
                    break;
            }
            //            socket.Send(message);
        }

        public void SendPrivateMsg(long toQQ, string message, bool autoEscape)
        {
            message = JsonConvert.ToString(message);
            var sendPrivateTemplate = @"{""action"": ""send_private_msg"",""params"": {""user_id"": user_qq,""message"": content_msg,""auto_escape"":""autoEscapeBool""}}";
            var str = sendPrivateTemplate.Replace("user_qq", toQQ.ToString());
            str = str.Replace("content_msg", message);
            str = str.Replace("autoEscapeBool", autoEscape ? "true":"false");
            Logger.Debug(str);
            Socket.Send(str);
        }

        public void SendGroupMsg(long groupId, string message, bool autoEscape)
        {
            message = JsonConvert.ToString(message);
            var sendPrivateTemplate = @"{""action"": ""send_group_msg"",""params"": {""group_id"": groupId,""message"": content_msg}}";
            var str = sendPrivateTemplate.Replace("groupId", groupId.ToString());
            str = str.Replace("content_msg", message);
            str = str.Replace("autoEscapeBool", autoEscape ? "true" : "false");
            Logger.Debug(str);
            Socket.Send(str);
        }

        public void SendDiscussMsg(long discussId, string message, bool autoEscape)
        {

            message = JsonConvert.ToString(message);
            var sendPrivateTemplate = @"{""action"": ""send_discuss_msg"",""params"": {""discuss_id"": discussId,""message"": content_msg}}";
            var str = sendPrivateTemplate.Replace("user_qq", discussId.ToString());
            str = str.Replace("content_msg", message);
            str = str.Replace("autoEscapeBool", autoEscape ? "true" : "false");
            Logger.Debug(str);
            Socket.Send(str);
        }

    }
}
