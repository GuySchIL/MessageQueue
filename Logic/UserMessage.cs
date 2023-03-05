using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class UserMessage // The output that the user gets from the program (Not the actual messaage used in logic)
    {
        public string MessageContent { get; set; }
        public string GroupName { get; set; }
        public DateTime ReceivedDateTime { get; set; }

        public UserMessage(string groupName, string messageContent, DateTime messageDateTime)
        {
            this.MessageContent = messageContent;
            this.GroupName = groupName;
            this.ReceivedDateTime = messageDateTime;
        }

        public override string ToString()
        {
            return $"Receiver: {GroupName}\nContent: {MessageContent}\nReceived Date: {ReceivedDateTime}";
        }
    }
}
