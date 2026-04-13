using System;

namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set console title
            Console.Title = "Cybersecurity Awareness Bot";

            // Play voice greeting
            AudioPlayer audio = new AudioPlayer();
            audio.PlayGreeting();

            // Start chatbot system
            Chatbot bot = new Chatbot();
            bot.Start();

            // Prevent console from closing immediately
            Console.ReadKey();
        }
    }
}
