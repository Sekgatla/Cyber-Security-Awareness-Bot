using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CybersecurityChatbot
{
    // NLP intent types recognised by the chatbot
    public enum NlpIntent
    {
        Unknown,
        AddTask,
        SetReminder,
        ViewTasks,
        DeleteTask,
        CompleteTask,
        StartQuiz,
        ShowActivityLog,
        CybersecurityTopic,
        Greeting,
        Farewell,
        HowAreYou,
        ShowHelp
    }

    // Extracted data from user input after NLP processing
    public class NlpResult
    {
        public NlpIntent Intent       { get; set; }
        public string    TaskTitle    { get; set; }
        public string    ReminderInfo { get; set; }
        public string    RawInput     { get; set; }

        public NlpResult()
        {
            Intent       = NlpIntent.Unknown;
            TaskTitle    = "";
            ReminderInfo = "";
            RawInput     = "";
        }
    }

    // Simulates Natural Language Processing using keyword detection,
    // regular expressions, and string manipulation to understand user intent.
    // Satisfies Task 3 — NLP Simulation requirement.
    public class NlpProcessor
    {
        // ── Intent pattern dictionaries ──────────────────────────────────────

        private static readonly List<string> AddTaskPatterns = new List<string>
        {
            "add task", "create task", "new task", "add a task",
            "set task", "make task", "i need to", "add to my list",
            "add a reminder task", "schedule task", "log task",
            "enable two-factor", "enable 2fa", "set up 2fa",
            "review privacy", "change password", "update password",
            "run antivirus", "check my accounts", "backup my"
        };

        private static readonly List<string> ReminderPatterns = new List<string>
        {
            "remind me", "set a reminder", "reminder for", "set reminder",
            "remind me to", "can you remind", "i want a reminder",
            "remind me in", "notify me", "alert me", "don't let me forget"
        };

        private static readonly List<string> ViewTaskPatterns = new List<string>
        {
            "show tasks", "view tasks", "my tasks", "list tasks",
            "show my tasks", "what tasks", "all tasks", "pending tasks",
            "what do i need to do", "show todo", "show to-do",
            "view my list", "task list"
        };

        private static readonly List<string> DeleteTaskPatterns = new List<string>
        {
            "delete task", "remove task", "cancel task",
            "get rid of task", "erase task", "drop task"
        };

        private static readonly List<string> CompleteTaskPatterns = new List<string>
        {
            "complete task", "mark done", "mark as done", "finish task",
            "task done", "completed task", "mark complete", "i finished",
            "tick off", "check off", "done with task"
        };

        private static readonly List<string> QuizPatterns = new List<string>
        {
            "start quiz", "quiz me", "take quiz", "play quiz",
            "quiz", "test me", "cybersecurity quiz", "test my knowledge",
            "security quiz", "quiz time", "begin quiz", "launch quiz",
            "i want to do the quiz", "can i do the quiz"
        };

        private static readonly List<string> ActivityLogPatterns = new List<string>
        {
            "show activity log", "activity log", "show log", "view log",
            "what have you done", "recent actions", "action log",
            "show history", "bot history", "what did you do",
            "show what you've done", "log history", "chat history"
        };

        private static readonly List<string> GreetingPatterns = new List<string>
        {
            "hello", "hi", "hey", "good morning", "good afternoon",
            "good evening", "howzit", "heya", "greetings", "sup"
        };

        private static readonly List<string> FarewellPatterns = new List<string>
        {
            "bye", "goodbye", "farewell", "see you", "later",
            "take care", "exit", "quit", "cya", "ttyl"
        };

        private static readonly List<string> HelpPatterns = new List<string>
        {
            "help", "what can you do", "what do you do", "capabilities",
            "features", "topics", "commands", "how do i", "guide", "menu"
        };

        // ── Reminder extraction patterns ─────────────────────────────────────

        private static readonly Dictionary<string, string> ReminderDayMap =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "today",     "today"          },
            { "tomorrow",  "tomorrow"       },
            { "monday",    "this Monday"    },
            { "tuesday",   "this Tuesday"   },
            { "wednesday", "this Wednesday" },
            { "thursday",  "this Thursday"  },
            { "friday",    "this Friday"    },
            { "saturday",  "this Saturday"  },
            { "sunday",    "this Sunday"    }
        };

        // ── Public API ───────────────────────────────────────────────────────

        // Main NLP entry point — analyses user input and returns structured result
        public NlpResult Analyse(string input)
        {
            NlpResult result = new NlpResult();
            result.RawInput  = input;

            string lower = input.ToLower().Trim();

            // Priority order matters — check specific intents before generic ones

            if (MatchesAny(lower, ActivityLogPatterns))
            {
                result.Intent = NlpIntent.ShowActivityLog;
                return result;
            }

            if (MatchesAny(lower, CompleteTaskPatterns))
            {
                result.Intent = NlpIntent.CompleteTask;
                return result;
            }

            if (MatchesAny(lower, DeleteTaskPatterns))
            {
                result.Intent = NlpIntent.DeleteTask;
                return result;
            }

            if (MatchesAny(lower, ViewTaskPatterns))
            {
                result.Intent = NlpIntent.ViewTasks;
                return result;
            }

            if (MatchesAny(lower, ReminderPatterns))
            {
                result.Intent       = NlpIntent.SetReminder;
                result.ReminderInfo = ExtractReminderInfo(lower);
                result.TaskTitle    = ExtractTaskTitle(lower);
                return result;
            }

            if (MatchesAny(lower, AddTaskPatterns))
            {
                result.Intent    = NlpIntent.AddTask;
                result.TaskTitle = ExtractTaskTitle(lower);
                return result;
            }

            if (MatchesAny(lower, QuizPatterns))
            {
                result.Intent = NlpIntent.StartQuiz;
                return result;
            }

            if (MatchesAny(lower, FarewellPatterns))
            {
                result.Intent = NlpIntent.Farewell;
                return result;
            }

            if (MatchesAny(lower, HelpPatterns))
            {
                result.Intent = NlpIntent.ShowHelp;
                return result;
            }

            if (MatchesAny(lower, GreetingPatterns) && lower.Length < 20)
            {
                result.Intent = NlpIntent.Greeting;
                return result;
            }

            result.Intent = NlpIntent.Unknown;
            return result;
        }

        // Extract a task title from phrases like "add task to enable 2FA"
        // or "remind me to update my password tomorrow"
        public string ExtractTaskTitle(string lower)
        {
            string[] skipPhrases = new string[]
            {
                "add task", "create task", "new task", "add a task",
                "set task", "make task", "remind me to", "remind me",
                "set a reminder", "i need to", "can you remind me to",
                "add a reminder to", "add a task to", "log a task to",
                "schedule a task to"
            };

            string cleaned = lower;
            foreach (string phrase in skipPhrases)
                cleaned = cleaned.Replace(phrase, "").Trim();

            // Remove trailing day references like "tomorrow", "on friday"
            foreach (string day in ReminderDayMap.Keys)
                cleaned = Regex.Replace(cleaned, @"\b" + day + @"\b", "", RegexOptions.IgnoreCase).Trim();

            cleaned = cleaned.TrimEnd('-', ',', '.', ' ');

            if (cleaned.Length < 3) return "";

            // Title-case the result
            return char.ToUpper(cleaned[0]) + cleaned.Substring(1);
        }

        // Extract reminder timing from phrases like "in 3 days", "tomorrow", "on Friday"
        public string ExtractReminderInfo(string lower)
        {
            // "in X day(s)"
            Match daysMatch = Regex.Match(lower, @"in\s+(\d+)\s+day", RegexOptions.IgnoreCase);
            if (daysMatch.Success)
            {
                int days = int.Parse(daysMatch.Groups[1].Value);
                DateTime reminder = DateTime.Today.AddDays(days);
                return reminder.ToString("dd MMMM yyyy") + " (" + days + " day" + (days == 1 ? "" : "s") + " from now)";
            }

            // "in X week(s)"
            Match weeksMatch = Regex.Match(lower, @"in\s+(\d+)\s+week", RegexOptions.IgnoreCase);
            if (weeksMatch.Success)
            {
                int weeks = int.Parse(weeksMatch.Groups[1].Value);
                DateTime reminder = DateTime.Today.AddDays(weeks * 7);
                return reminder.ToString("dd MMMM yyyy") + " (" + weeks + " week" + (weeks == 1 ? "" : "s") + " from now)";
            }

            // Named days
            foreach (string day in ReminderDayMap.Keys)
            {
                if (Regex.IsMatch(lower, @"\b" + day + @"\b", RegexOptions.IgnoreCase))
                    return ReminderDayMap[day];
            }

            return "";
        }

        // Check if input contains any of the given patterns
        private bool MatchesAny(string lower, List<string> patterns)
        {
            foreach (string pattern in patterns)
            {
                if (lower.Contains(pattern))
                    return true;
            }
            return false;
        }
    }
}
