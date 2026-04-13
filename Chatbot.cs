using System;
using System.Threading;

namespace CyberSecurityChatbot
{
    public class Chatbot
    {
        // Store current user
        private User _user = new User();

        public void Start()
        {
            // Display ASCII art header
            DisplayHeader();

            // Ask for user name
            GetUserName();

            // Start chatbot interaction
            RunChat();
        }

        private void DisplayHeader()
        {
            // Set console color
            Console.ForegroundColor = ConsoleColor.Green;

            // Top border
            Console.WriteLine("====================================================");

            // ASCII Art Title
            Console.WriteLine(@"
   ____            _               ____        _   
  / ___| _   _ ___| |_ ___ _ __   | __ )  ___ | |_ 
 | |    | | | / __| __/ _ \ '__|  |  _ \ / _ \| __|
 | |___ | |_| \__ \ ||  __/ |     | |_) | (_) | |_ 
  \____| \__, |___/\__\___|_|     |____/ \___/ \__|
         |___/                                    
");

            // Title text
            Console.WriteLine("        CYBERSECURITY AWARENESS BOT");

            // Bottom border
            Console.WriteLine("====================================================");

            // Reset color
            Console.ResetColor();
        }

        private void GetUserName()
        {
            // Prompt user for name
            Console.Write("\nEnter your name: ");
            string? input = Console.ReadLine();

            // Validate input
            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("Name cannot be empty. Enter your name: ");
                input = Console.ReadLine();
            }

            // Store user
            _user = new User { Name = input };

            // Display welcome message
            TypeEffect($"\nHello {_user.Name}! Welcome to the Cybersecurity Awareness Bot.\n");
        }

        private void RunChat()
        {
            while (true)
            {
                // Prompt user
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("\nAsk a question (type 'exit' to quit): ");
                Console.ResetColor();

                string? input = Console.ReadLine()?.ToLower();

                // Validate empty input
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please enter something.");
                    continue;
                }

                // Exit condition
                if (input == "exit")
                {
                    Console.WriteLine("Goodbye! Stay safe online.");
                    break;
                }

                // Process response
                Respond(input);
            }
        }

        private void Respond(string input)
        {
            // Respond to greeting
            if (input.Contains("how are you"))
            {
                Console.WriteLine("I'm just a program, but I'm here to help you stay safe online!");
            }
            // Respond to purpose
            else if (input.Contains("purpose"))
            {
                Console.WriteLine("My purpose is to educate users about cybersecurity threats.");
            }
            // Respond to password questions
            else if (input.Contains("password"))
            {
                Console.WriteLine("Use strong passwords with uppercase, lowercase, numbers, and symbols.");
            }
            // Respond to phishing questions
            else if (input.Contains("phishing"))
            {
                Console.WriteLine("Do not click suspicious links. Always verify the sender of emails.");
            }
            // Respond to safe browsing
            else if (input.Contains("safe browsing"))
            {
                Console.WriteLine("Use secure websites (https) and avoid downloading unknown files.");
            }
            // Default response
            else
            {
                Console.WriteLine("I didn't quite understand that. Could you rephrase?");
            }
        }

        private void TypeEffect(string message)
        {
            // Simulate typing animation
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(30);
            }
        }
    }
}
