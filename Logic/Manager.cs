using DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logic
{
    public class Manager
    {
        ProjectDoubleLinkedList<Message> messagesByDateList;
        ProjectHashTable<string, ProjectLinkedQueue<ProjectDoubleLinkedList<Message>.Node>> groupHashTable;

        public Manager()
        {
            messagesByDateList = new ProjectDoubleLinkedList<Message>();
            groupHashTable = new ProjectHashTable<string, ProjectLinkedQueue<ProjectDoubleLinkedList<Message>.Node>>();
        }

        public UserMessage ConvertToUserMessage(Message message)
        {
            UserMessage userMessage = new UserMessage(message.GroupName, message.MessageContent, message.ReceivedDateTime);
            return userMessage;
        }
        public void SendMessageToGroup(string groupName, string message)
        {
            Message newMessage = new Message(groupName, message);
            messagesByDateList.AddLast(newMessage);

            if (groupHashTable.ContainsKey(groupName))
            {
                //Adds a new message node to the existing queue.
                ProjectLinkedQueue<ProjectDoubleLinkedList<Message>.Node> queue = groupHashTable.GetValue(groupName);
                queue.EnQueue(messagesByDateList.Tail);
            }
            else
            {
                //Create a new queue of the new groupname
                ProjectLinkedQueue<ProjectDoubleLinkedList<Message>.Node> queue = new ProjectLinkedQueue<ProjectDoubleLinkedList<Message>.Node>();
                groupHashTable.Add(groupName, queue);
                queue.EnQueue(messagesByDateList.Tail);
            }
            //If gruopName entered doesnt exist, create a new gruop with groupName, then add the message entered.
            //If the groupName already exists, add the message to the queue.
        }
        public void GetAllGroupNames(out string[] groupNames)
        {
            groupNames = default;
            if (groupHashTable.ItemsCount == 0) return;

            groupNames = new string[groupHashTable.ItemsCount];
            int index = 0;
            foreach (var item in groupHashTable)
            {
                groupNames[index] = item;
                index++;
            }
            //Returns a string of all the group names.
            return;
        }

        public bool GetAndRemoveMessageFromGroup(string groupName, out UserMessage userMessage) // Reads and takes out the oldest message in the key group entered.
        {
            userMessage = default;
            Message message = default;

            if (groupHashTable.ContainsKey(groupName))
            {
                ProjectDoubleLinkedList<Message>.Node doubleLinkedListNode;

                var messageQueueByGroupName = groupHashTable.GetValue(groupName);

                if (messageQueueByGroupName.Peek(out doubleLinkedListNode))
                {
                    var tmp = doubleLinkedListNode;
                    var messageOutput = messagesByDateList.GetDataByNode(tmp);
                    messageQueueByGroupName.DeQueue();
                    messagesByDateList.RemoveByNode(doubleLinkedListNode);
                    message = messageOutput;
                    userMessage =  ConvertToUserMessage(message);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public bool GetAndRemoveOldestMessage(out Message message) 
        {
            message = default;

            if (messagesByDateList.Head != null)
            {
                messagesByDateList.GetAt(0, out Message oldestMessage);
                message = oldestMessage;
                var groupName = oldestMessage.GroupName;
                ProjectLinkedQueue<ProjectDoubleLinkedList<Message>.Node> queue = groupHashTable.GetValue(groupName);
                queue.DeQueue();
                messagesByDateList.RemoveFirst();
                return true;
            }
            return false;
        }

        public bool GetOldestMessages(int numOfMessages, out Message[] messages)
        {
            messages = default;

            if (messagesByDateList.Count == 0) return false;
            if (messagesByDateList.Count < numOfMessages) numOfMessages = messagesByDateList.Count;

            messages = new Message[numOfMessages];

            int index = 0;
            foreach (Message message in messagesByDateList)
            {
                if (index >= numOfMessages)
                {
                    return true;
                }
                messages[index] = message;
                index++;
            }
            return true;
        }
        public bool GetNewestMessages(int numOfMessages, out Message[] messages) // Need fixing
        {
            messages = default;

            if (messagesByDateList.Count == 0) return false;
            if (messagesByDateList.Count < numOfMessages) numOfMessages = messagesByDateList.Count;

            messages = new Message[numOfMessages];

            var currentNode = messagesByDateList.Tail;
            
            for (int i = 0; i < numOfMessages; i++)
            {
                var outputMessage = messagesByDateList.GetDataByNode(currentNode);

                messages[i] = outputMessage;

                currentNode = messagesByDateList.GetPreviousNode(currentNode);
            }
            return true;
        }
        public bool GetMessagesFromDateTime(DateTime dateTime, out Message[] messages) // From messages list (Without removing)
        {
            messages = default;
            if (messagesByDateList.Count == 0) return false;

            int messagesCounter = 0;

            foreach (Message message in messagesByDateList)
            {
                if (message.ReceivedDateTime > dateTime) messagesCounter++;
            }
            messages = new Message[messagesCounter];
            int index = 0;
            foreach (Message message in messagesByDateList)
            {
                if (message.ReceivedDateTime > dateTime)
                {
                    messages[index] = message;
                    index++;
                }
            }
            return true;
        }
        public bool GetMessagesToDateTime(DateTime dateTime, out Message[] messages) // From messages list (Without removing)
        {
            messages = default;
            if (messagesByDateList.Count == 0) return false;

            int messagesCounter = 0;

            foreach (Message message in messagesByDateList)
            {
                if (message.ReceivedDateTime < dateTime) messagesCounter++;
            }
            messages = new Message[messagesCounter];
            int index = 0;
            foreach (Message message in messagesByDateList)
            {
                if (message.ReceivedDateTime < dateTime)
                {
                    messages[index] = message;
                    index++;
                }
            }
            return true;
        }
        public bool ReadMessagesByGroupAndContent(string groupName, string content, out Message[] messages) // Does not delete the message
        {
            messages = default;
            if (messagesByDateList.Count == 0) return false;

            string lowerCaseContent = content.ToLower();
            string lowerGroupName = groupName.ToLower();

            Regex regex = new Regex($@".{lowerCaseContent}");


            int messagesCount = 0;

            foreach (Message message in messagesByDateList)
            {
                if (message.GroupName.ToLower() == lowerGroupName)
                {
                    if (Regex.IsMatch(message.MessageContent.ToLower(), @"\b" + content + @"\b"))
                    {
                        messagesCount++;
                    }
                }
            }

            if (messagesCount == 0) return false;

            int index = 0;
            messages = new Message[messagesCount];

            foreach (Message message in messagesByDateList)
            {
                if (message.GroupName.ToLower() == lowerGroupName)
                {
                    if (Regex.IsMatch(message.MessageContent.ToLower(), @"\b" + content + @"\b"))
                    {
                        messages[index] = message;
                        index++;
                    }
                }
            }

            return true;
        }
    }
}
