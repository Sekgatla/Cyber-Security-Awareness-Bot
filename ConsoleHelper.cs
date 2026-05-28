using System;
  using System.Threading;

  namespace CyberSecurityChatbot
  {
      // Static helper class for all console UI utilities
      // Keeps Chatbot.cs clean by separating all display logic
      public static class ConsoleHelper
      {
          private const int ConsoleWidth = 60;

          // ── Coloured output ──────────────────────────────────────────

          public static void WriteColored(string text, ConsoleColor color)
          {
              Console.ForegroundColor = color;
              Console.WriteLine(text);
              Console.ResetColor();
          }

          public static void WriteSuccess(string text) { WriteColored(text, ConsoleColor.Green); }
          public static void WriteWarning(string text) { WriteColored(text, ConsoleColor.Yellow); }
          public static void WriteDanger(string text)  { WriteColored(text, ConsoleColor.Red); }
          public static void WriteInfo(string text)    { WriteColored(text, ConsoleColor.Cyan); }
          public static void WriteMuted(string text)   { WriteColored(text, ConsoleColor.DarkGray); }

          // ── Typing effect ────────────────────────────────────────────

          public static void TypeText(string text, ConsoleColor color = ConsoleColor.Green, int delayMs = 20)
          {
              Console.ForegroundColor = color;
              foreach (char c in text)
              {
                  Console.Write(c);
                  Thread.Sleep(delayMs);
              }
              Console.WriteLine();
              Console.ResetColor();
          }

          // ── Borders ──────────────────────────────────────────────────

          public static void DrawBorder()
          {
              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.WriteLine(new string('=', ConsoleWidth));
              Console.ResetColor();
          }

          public static void DrawDivider()
          {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine(new string('-', ConsoleWidth));
              Console.ResetColor();
          }

          // ── Bot prefix with severity colour ──────────────────────────

          public static void PrintBotPrefix(string severity = "INFO")
          {
              ConsoleColor color = ConsoleColor.Cyan;
              if (severity == "WARNING") color = ConsoleColor.Yellow;
              if (severity == "DANGER")  color = ConsoleColor.Red;
              if (severity == "SAFE")    color = ConsoleColor.Green;

              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.Write("  [BOT] ");
              Console.ForegroundColor = color;
              Console.Write("[" + severity + "] ");
              Console.ResetColor();
          }

          // ── Thinking animation ───────────────────────────────────────

          public static void ShowThinking()
          {
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.Write("  [BOT] ");
              Console.ForegroundColor = ConsoleColor.DarkGreen;
              for (int i = 0; i < 3; i++) { Console.Write("."); Thread.Sleep(300); }
              Console.WriteLine();
              Console.ResetColor();
              Thread.Sleep(150);
          }

          // ── Numbered suggestions ─────────────────────────────────────

          public static void ShowSuggestions(string[] suggestions)
          {
              Console.WriteLine();
              WriteMuted("  Quick suggestions:");
              for (int i = 0; i < suggestions.Length; i++)
              {
                  Console.ForegroundColor = ConsoleColor.DarkCyan;
                  Console.Write("    [" + (i + 1) + "] ");
                  Console.ForegroundColor = ConsoleColor.Gray;
                  Console.WriteLine(suggestions[i]);
                  Console.ResetColor();
              }
          }

          // ── User input prompt ────────────────────────────────────────

          public static string Prompt(string label = "You")
          {
              Console.WriteLine();
              Console.ForegroundColor = ConsoleColor.DarkCyan;
              Console.Write("  " + label + " > ");
              Console.ForegroundColor = ConsoleColor.White;
              string input = Console.ReadLine() ?? string.Empty;
              Console.ResetColor();
              return input;
          }
      }
  }
  