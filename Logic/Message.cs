using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Message // Includes queueName, DateTime and messageContent
    {
        public string MessageContent { get; set; }
        public string GroupName { get; set; }
        public DateTime ReceivedDateTime { get; set; }

        public Message(string groupName, string messageContent)
        {
            this.MessageContent = messageContent;
            this.GroupName = groupName;
            this.ReceivedDateTime = DateTime.Now;
        }
        public override string ToString()
        {
            return $"Receiver: {GroupName}\nContent: {MessageContent}\nReceived Date: {ReceivedDateTime}";
        }
    }
}
