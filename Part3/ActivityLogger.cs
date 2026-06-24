using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Records all significant chatbot actions with timestamps.
    // Used by Task 4 — Activity Log Feature.
    public class ActivityLogger
    {
        private List<string> _log;
        private const int MaxEntries = 50;

        public ActivityLogger()
        {
            _log = new List<string>();
        }

        // Add a new log entry with current timestamp
        public void Log(string action)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string entry     = "[" + timestamp + "] " + action;
            _log.Add(entry);

            // Keep the list from growing indefinitely
            if (_log.Count > MaxEntries)
                _log.RemoveAt(0);
        }

        // Return the last N entries (default 10) for display
        public List<string> GetRecentLog(int count = 10)
        {
            int start = Math.Max(0, _log.Count - count);
            return _log.GetRange(start, _log.Count - start);
        }

        // Return all entries
        public List<string> GetFullLog()
        {
            return new List<string>(_log);
        }

        // Format log as a numbered chat response string
        public string GetLogAsText(int count = 10)
        {
            List<string> recent = GetRecentLog(count);

            if (recent.Count == 0)
                return "No actions have been recorded yet. Start chatting, add tasks, or take the quiz!";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Here is a summary of recent actions:\n");

            for (int i = 0; i < recent.Count; i++)
                sb.AppendLine((i + 1) + ". " + recent[i]);

            return sb.ToString().TrimEnd();
        }

        public int Count => _log.Count;
    }
}
