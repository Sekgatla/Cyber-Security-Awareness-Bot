using System;

  namespace CyberSecurityChatbot
  {
      // Stores all information about the current user session
      public class User
      {
          public string Name           { get; set; } = string.Empty;
          public string SessionId      { get; set; } = string.Empty;
          public DateTime SessionStart { get; set; }
          public int MessageCount      { get; set; }

          // Initialise a new session for the given name
          public User(string name)
          {
              Name         = name;
              SessionId    = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
              SessionStart = DateTime.Now;
              MessageCount = 0;
          }

          // Default constructor required before name is captured
          public User() { }

          // Return how long the session has been running
          public string GetDuration()
          {
              TimeSpan duration = DateTime.Now - SessionStart;
              if (duration.Minutes > 0)
                  return duration.Minutes + "m " + duration.Seconds + "s";
              return duration.Seconds + "s";
          }
      }
  }
  