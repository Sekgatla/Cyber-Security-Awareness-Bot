using System;

  namespace CyberSecurityChatbot
  {
      class Program
      {
          static void Main(string[] args)
          {
              Console.Title = "Cybersecurity Awareness Bot";
              Console.OutputEncoding = System.Text.Encoding.UTF8;

              // Play voice greeting
              AudioPlayer audio = new AudioPlayer();
              audio.PlayGreeting();

              // Start the chatbot
              Chatbot bot = new Chatbot();
              bot.Start();
          }
      }
  }
  