using System;
  using System.IO;
  using System.Media;
  using System.Threading;

  namespace CyberSecurityChatbot
  {
      public class AudioPlayer
      {
          // Path to the WAV greeting file
          private readonly string _filePath = "greeting.wav";

          public void PlayGreeting()
          {
              try
              {
                  if (File.Exists(_filePath))
                  {
                      SoundPlayer player = new SoundPlayer(_filePath);
                      player.PlaySync();
                  }
                  else
                  {
                      // Fallback text greeting if WAV file is missing
                      Console.ForegroundColor = ConsoleColor.Cyan;
                      TypeFallback("  Welcome to the Cybersecurity Awareness Bot.");
                      TypeFallback("  Created by Sekgatla.");
                      TypeFallback("  I am here to help you stay safe online.");
                      Console.ResetColor();
                      Console.WriteLine();
                  }
              }
              catch (Exception ex)
              {
                  Console.ForegroundColor = ConsoleColor.DarkGray;
                  Console.WriteLine("  [Audio] Could not play greeting: " + ex.Message);
                  Console.ResetColor();
              }
          }

          private void TypeFallback(string text)
          {
              foreach (char c in text)
              {
                  Console.Write(c);
                  Thread.Sleep(20);
              }
              Console.WriteLine();
          }
      }
  }
  