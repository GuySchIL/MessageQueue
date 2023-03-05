using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    public class UserMenu
    {
        Manager m = new Manager();

        enum UserChoice
        {
            ExitApplication = 0,
            SendNewMessage = 1,
            ShowAllGroupNames = 2,
            ReadAndRemoveOldestMessageFromGroup = 3,
            ReadAndRemoveOldestMessage = 4,
            ReadOldestMessages = 5,
            ReadNewestMessages = 6,
            ReadMessagesFromDate = 7,
            ReadMessagesToDate = 8,
            ReadMessagesOfGroupByContent = 9
        }

        public UserMenu()
        {
            ShowMenu();
            ActivateLogic();
        }

        private void ShowMenu()
        {
            Console.WriteLine("0 - Exit Application");
            Console.WriteLine("1 - Send a new message");
            Console.WriteLine("2 - Show all contact names");
            Console.WriteLine("3 - Show and remove oldest message of contact");
            Console.WriteLine("4 - Show and remove oldest message");
            Console.WriteLine("5 - Show numerous oldest messages");
            Console.WriteLine("6 - Show numerous newest messages");
            Console.WriteLine("7 - Show messages from a specific date");
            Console.WriteLine("8 - Show messages to a specific date");
            Console.WriteLine("9 - Show messages from contact that contains keywords entered");
        }

        public int GetUserInput()
        {
            Console.WriteLine("\nPlease enter a valid digit of the menu options:");

            bool isValid = int.TryParse(Console.ReadLine(), out int userChoice);
            while (!isValid || userChoice < 0 || userChoice > 9)
            {
                isValid = int.TryParse(Console.ReadLine(), out userChoice);
            }
            return userChoice;
        }
        public DateTime GetDateTimeFromUser()
        {
            while (true)
            {
                Console.WriteLine("Please enter Year:");
                bool isYearValid = int.TryParse(Console.ReadLine(), out int yearEntered);
                while (!isYearValid || yearEntered < 1990 || yearEntered > DateTime.Now.Year)
                {
                    Console.WriteLine("Wrong year entered!");
                    isYearValid = int.TryParse(Console.ReadLine(), out yearEntered);
                }

                Console.WriteLine("Please enter num of month:");
                bool isMonthValid = int.TryParse(Console.ReadLine(), out int monthEntered);
                while (!isMonthValid || monthEntered < 0 || monthEntered > 12)
                {
                    Console.WriteLine("Wrong month entered!");
                    isMonthValid = int.TryParse(Console.ReadLine(), out monthEntered);
                }

                Console.WriteLine("Please enter num of day:");
                bool isDayValid = int.TryParse(Console.ReadLine(), out int dayEntered);
                while (!isDayValid || dayEntered < 0 || dayEntered > 31)
                {
                    Console.WriteLine("Wrong day entered!");
                    isDayValid = int.TryParse(Console.ReadLine(), out dayEntered);
                }

                DateTime dateCheck = new DateTime(yearEntered, monthEntered, dayEntered);

                if (dateCheck < DateTime.Now)
                {
                    return dateCheck;
                }
            }
        }

        public void ActivateLogic()
        {
            while (true)
            {
                var userInput = GetUserInput();
                UserChoice convertedUserInput = (UserChoice)userInput;

                switch (convertedUserInput)
                {
                    case UserChoice.ExitApplication:
                        {
                            Environment.Exit(0);
                            break;
                        }
                    case UserChoice.SendNewMessage:
                        {
                            Console.WriteLine("Enter Contact name: ");
                            string contactName = Console.ReadLine();
                            Console.WriteLine("Enter message content: ");
                            string messageContent = Console.ReadLine();

                            m.SendMessageToGroup(contactName, messageContent);
                            break;
                        }
                    case UserChoice.ShowAllGroupNames:
                        {
                            m.GetAllGroupNames(out string[] contactNames);
                            Console.WriteLine();
                            if(contactNames != null)
                            {
                                foreach (string name in contactNames) Console.WriteLine(name);

                            }
                            break;
                        }
                    case UserChoice.ReadAndRemoveOldestMessageFromGroup:
                        {
                            Console.WriteLine("Enter Contact name: ");
                            string contactName = Console.ReadLine();

                            bool isValid = m.GetAndRemoveMessageFromGroup(contactName, out UserMessage message);
                            if (!isValid) Console.WriteLine("Error!");
                            Console.WriteLine(message);
                            break;
                        }
                    case UserChoice.ReadAndRemoveOldestMessage:
                        {
                            m.GetAndRemoveOldestMessage(out Message message);
                            Console.WriteLine(message);
                            break;
                        }
                    case UserChoice.ReadOldestMessages:
                        {
                            Console.WriteLine("Enter a valid number of messages requested");

                            bool isValid = int.TryParse(Console.ReadLine(), out int numOfMessages);
                            while (!isValid) isValid = int.TryParse(Console.ReadLine(), out numOfMessages);

                            m.GetOldestMessages(numOfMessages, out Message[] messages);

                            for (int i = 0; i < messages.Length; i++)
                            {
                                Console.WriteLine(messages[i]);
                            }
                            break;
                        }
                    case UserChoice.ReadNewestMessages:
                        {
                            Console.WriteLine("Enter a valid number of messages requested");

                            bool isValid = int.TryParse(Console.ReadLine(), out int numOfMessages);
                            while (!isValid) isValid = int.TryParse(Console.ReadLine(), out numOfMessages);

                            m.GetNewestMessages(numOfMessages, out Message[] messages);

                            for (int i = 0; i < messages.Length; i++)
                            {
                                Console.WriteLine(messages[i]);
                            }
                            break;
                        }
                    case UserChoice.ReadMessagesFromDate:
                        {
                            var userDateTimeInput = GetDateTimeFromUser();

                            m.GetMessagesFromDateTime(userDateTimeInput, out Message[] messages);

                            for (int i = 0; i < messages.Length; i++)
                            {
                                Console.WriteLine(messages[i]);
                            }
                            break;
                        }
                    case UserChoice.ReadMessagesToDate:
                        {
                            var userDateTimeInput = GetDateTimeFromUser();

                            m.GetMessagesToDateTime(userDateTimeInput, out Message[] messages);

                            for (int i = 0; i < messages.Length; i++)
                            {
                                Console.WriteLine(messages[i]);
                            }
                            break;
                        }
                    case UserChoice.ReadMessagesOfGroupByContent:
                        {
                            Console.WriteLine("Enter Contact name: ");
                            string contactName = Console.ReadLine();
                            Console.WriteLine("Enter message search by keyword: ");
                            string contentKeyword = Console.ReadLine();

                            m.ReadMessagesByGroupAndContent(contactName, contentKeyword, out Message[] messages);

                            for (int i = 0; i < messages.Length; i++)
                            {
                                Console.WriteLine(messages[i]);
                            }
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Wrong input!");
                            break;
                        }
                }
            }
        }
    }
}
