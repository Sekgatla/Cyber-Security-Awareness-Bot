using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Delegate: transforms a raw response by prepending context (sentiment + personalisation)
    public delegate string ResponseTransformer(string rawResponse);

    class ChatBot
    {
        private KeywordResponder  _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore       _memory;
        private NlpProcessor      _nlp;
        private ActivityLogger    _logger;
        private TaskManager       _taskManager;
        private QuizEngine        _quiz;

        private bool   _awaitingName;
        private string _lastTopic;
        private bool   _awaitingReminderForTask;
        private string _pendingTaskTitle;
        private string _pendingTaskDescription;
        private Random _random;

        public ActivityLogger Logger      => _logger;
        public TaskManager    TaskManager => _taskManager;
        public QuizEngine     Quiz        => _quiz;

        public ChatBot()
        {
            _keywords              = new KeywordResponder();
            _sentiment             = new SentimentDetector();
            _memory                = new MemoryStore();
            _nlp                   = new NlpProcessor();
            _logger                = new ActivityLogger();
            _taskManager           = new TaskManager();
            _quiz                  = new QuizEngine();
            _awaitingName          = true;
            _lastTopic             = "";
            _awaitingReminderForTask = false;
            _pendingTaskTitle      = "";
            _pendingTaskDescription = "";
            _random                = new Random();
        }

        public string GetGreeting()
        {
            return "Hello! I am your Cybersecurity Awareness Assistant v3.0.\n"
                 + "I am here to help South African citizens stay safe online.\n\n"
                 + "New in v3.0:\n"
                 + "  • Task Assistant  — manage cybersecurity tasks (Tasks tab)\n"
                 + "  • Mini Quiz       — test your knowledge (Quiz tab)\n"
                 + "  • Activity Log    — view your history (Log tab)\n"
                 + "  • NLP Recognition — talk naturally!\n\n"
                 + "Before we begin — what is your name?";
        }

        // Central routing for every user message typed in the Chat tab
        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type something so I can help you.";

            string lower = input.ToLower().Trim();

            // ── Step 1: Capture the user's name on first message ─────────────
            if (_awaitingName)
            {
                string name = input.Trim();
                if (name.Length < 2)
                    return "Please enter a valid name (at least 2 characters).";

                _memory.UserName = char.ToUpper(name[0]) + name.Substring(1).ToLower();
                _memory.Store("name", _memory.UserName);
                _awaitingName = false;

                _logger.Log("User registered as: " + _memory.UserName);

                return "Welcome, " + _memory.UserName + "! Great to have you here.\n\n"
                     + "I can help you with cybersecurity topics:\n"
                     + "  • Passwords  • Phishing  • Scams  • Privacy\n"
                     + "  • Malware    • 2FA       • Wi-Fi  • Social Engineering\n\n"
                     + "You can also:\n"
                     + "  — Add tasks  (e.g. \"Add task: Review privacy settings\")\n"
                     + "  — Take a quiz (type \"start quiz\")\n"
                     + "  — View your activity log (type \"show activity log\")\n\n"
                     + "Just type a topic or command to get started!";
            }

            // ── Step 2: Handle pending reminder prompt ────────────────────────
            if (_awaitingReminderForTask)
            {
                _awaitingReminderForTask = false;

                if (lower.Contains("yes") || lower.Contains("remind") || lower.Contains("sure"))
                {
                    // User wants a reminder — extract the timing
                    string reminderInfo = _nlp.ExtractReminderInfo(lower);

                    if (string.IsNullOrEmpty(reminderInfo))
                    {
                        // Ask for timeframe
                        return "When would you like to be reminded?\n"
                             + "Examples: \"in 3 days\", \"tomorrow\", \"in 1 week\", \"on Friday\"";
                    }

                    TaskItem task = _taskManager.AddTask(
                        _pendingTaskTitle,
                        _pendingTaskDescription,
                        reminderInfo);

                    _logger.Log("Task added with reminder: '" + _pendingTaskTitle + "' — Reminder: " + reminderInfo);

                    _pendingTaskTitle       = "";
                    _pendingTaskDescription = "";

                    return "Done! Task added:\n"
                         + "  Title:    " + task.Title + "\n"
                         + "  Reminder: " + reminderInfo + "\n\n"
                         + "I will remind you on " + reminderInfo + ". Stay on top of your cybersecurity!";
                }
                else if (lower.Contains("no") || lower.Contains("nope") || lower.Contains("don't") || lower.Contains("dont"))
                {
                    TaskItem task = _taskManager.AddTask(
                        _pendingTaskTitle,
                        _pendingTaskDescription,
                        "");

                    _logger.Log("Task added (no reminder): '" + _pendingTaskTitle + "'");

                    _pendingTaskTitle       = "";
                    _pendingTaskDescription = "";

                    return "Task added without a reminder:\n"
                         + "  Title: " + task.Title + "\n\n"
                         + "You can view all tasks in the Tasks tab or by typing \"show tasks\".";
                }
                else
                {
                    // They typed a time directly
                    string reminderInfo = _nlp.ExtractReminderInfo(lower);

                    if (string.IsNullOrEmpty(reminderInfo))
                        reminderInfo = input.Trim();

                    TaskItem task = _taskManager.AddTask(
                        _pendingTaskTitle,
                        _pendingTaskDescription,
                        reminderInfo);

                    _logger.Log("Task added with reminder: '" + _pendingTaskTitle + "' — Reminder: " + reminderInfo);

                    _pendingTaskTitle       = "";
                    _pendingTaskDescription = "";

                    return "Task added with reminder for " + reminderInfo + ".\n"
                         + "You can view all tasks in the Tasks tab or type \"show tasks\".";
                }
            }

            // ── Step 3: NLP intent detection ──────────────────────────────────
            NlpResult nlpResult = _nlp.Analyse(input);

            switch (nlpResult.Intent)
            {
                case NlpIntent.ShowActivityLog:
                    _logger.Log("User requested activity log");
                    return _logger.GetLogAsText();

                case NlpIntent.StartQuiz:
                    _quiz.Start();
                    _logger.Log("Quiz started by user");
                    QuizQuestion q = _quiz.GetCurrentQuestion();
                    return FormatQuestion(q, 1);

                case NlpIntent.ViewTasks:
                    _logger.Log("User viewed task list via chat");
                    return GetTasksText();

                case NlpIntent.AddTask:
                    return HandleAddTaskRequest(input, nlpResult);

                case NlpIntent.SetReminder:
                    return HandleSetReminderRequest(input, nlpResult);

                case NlpIntent.DeleteTask:
                    return "To delete a task, please use the Tasks tab where you can select and delete it.\n"
                         + "Type \"show tasks\" to see your task list.";

                case NlpIntent.CompleteTask:
                    return "To mark a task as complete, use the Tasks tab and click 'Mark Done'.\n"
                         + "Type \"show tasks\" to see your task list.";

                case NlpIntent.Farewell:
                    return "Thank you for chatting, " + _memory.UserName + "!\n"
                         + "Stay alert, stay safe. Cybersecurity is everyone's responsibility.\n\n"
                         + "Report cybercrime : www.saps.gov.za  |  Emergency: 10111\n"
                         + "Banking fraud (SABRIC): www.sabric.co.za";

                case NlpIntent.Greeting:
                    return "Hello again, " + _memory.UserName + "! How can I help you stay safe online today?\n"
                         + "Ask me about a cybersecurity topic, start the quiz, or manage your tasks!";

                case NlpIntent.HowAreYou:
                    return "I am running at full capacity — all systems green!\n"
                         + "More importantly, are YOU staying safe online, " + _memory.UserName + "?";

                case NlpIntent.ShowHelp:
                    _logger.Log("User requested help/topic list");
                    return "Here is everything I can help you with:\n\n"
                         + "CYBERSECURITY TOPICS:\n"
                         + "  Type any of: passwords, phishing, scams, privacy,\n"
                         + "  malware, 2FA, wifi, social engineering\n\n"
                         + "TASK ASSISTANT:\n"
                         + "  • \"Add task: [your task]\"    — add a new task\n"
                         + "  • \"Show tasks\" / \"My tasks\" — view all tasks\n"
                         + "  • \"Remind me to...\"         — add task with reminder\n\n"
                         + "QUIZ:\n"
                         + "  • \"Start quiz\"              — begin the cybersecurity quiz\n\n"
                         + "ACTIVITY LOG:\n"
                         + "  • \"Show activity log\"       — see recent bot actions\n"
                         + "  • \"What have you done?\"     — same as above";
            }

            // ── Step 4: Detect and store favourite topic interest ─────────────
            if (lower.Contains("interested in") || lower.Contains("i like") || lower.Contains("i love"))
            {
                List<string> keywords = _keywords.GetAllKeywords();
                foreach (string kw in keywords)
                {
                    if (lower.Contains(kw))
                    {
                        _memory.FavouriteTopic = kw;
                        _memory.Store("favourite_topic", kw);
                        _lastTopic = kw;

                        string topicResponse = _keywords.GetResponseByKeyword(kw);
                        _logger.Log("Favourite topic set: " + kw);
                        return "Great! I will remember that you are interested in " + kw + ".\n\n"
                             + topicResponse;
                    }
                }
            }

            // ── Step 5: Handle follow-up requests ─────────────────────────────
            if (lower.Contains("tell me more") || lower.Contains("explain more")
                || lower.Contains("more info")  || lower.Contains("another tip")
                || lower.Contains("give me more") || lower.Contains("more detail")
                || lower.Contains("go on")       || lower.Contains("keep going"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    string followUp = _keywords.GetResponseByKeyword(_lastTopic);
                    if (followUp != null)
                    {
                        ResponseTransformer addOpener = delegate(string raw)
                        {
                            return _memory.GetPersonalisedOpener()
                                 + "here is another tip on " + _lastTopic + ":\n\n"
                                 + raw;
                        };
                        _logger.Log("Follow-up tip provided on: " + _lastTopic);
                        return addOpener(followUp);
                    }
                }
                return "Could you let me know which topic you would like more information on?";
            }

            // ── Step 6: Detect sentiment ──────────────────────────────────────
            Sentiment sentiment       = _sentiment.Detect(input);
            string    sentimentOpener = _sentiment.GetSentimentResponse(sentiment);

            // ── Step 7: Match a keyword and apply ResponseTransformer delegate ─
            string keywordResponse = _keywords.GetResponse(input);
            if (keywordResponse != null)
            {
                foreach (string kw in _keywords.GetAllKeywords())
                {
                    if (lower.Contains(kw))
                    {
                        _lastTopic = kw;
                        break;
                    }
                }

                ResponseTransformer applyContext = delegate(string raw)
                {
                    return sentimentOpener + _memory.GetPersonalisedOpener() + raw;
                };

                _logger.Log("Cybersecurity tip provided: " + _lastTopic);
                return applyContext(keywordResponse);
            }

            // ── Step 8: Special phrases ───────────────────────────────────────
            if (lower.Contains("how are you"))
            {
                return "I am running at full capacity — all systems green!\n"
                     + "More importantly, are YOU staying safe online, " + _memory.UserName + "?";
            }

            // ── Step 9: Fallback ──────────────────────────────────────────────
            string[] fallbacks = new string[]
            {
                "I am not sure I understand — try rephrasing, or type 'help' to see all commands.",
                "I did not quite catch that. Try asking about passwords, phishing, or malware.",
                "Hmm, I did not recognise that. Type 'help' to see what I can do for you.",
                "I specialise in cybersecurity. You can also add tasks or take a quiz!",
                "Could you rephrase that? Or try 'start quiz' to test your knowledge!"
            };

            return fallbacks[_random.Next(0, fallbacks.Length)];
        }

        // ── Task handling ────────────────────────────────────────────────────

        private string HandleAddTaskRequest(string input, NlpResult nlp)
        {
            string title = nlp.TaskTitle;

            // Try to extract from "Add task: <title>" pattern
            if (string.IsNullOrEmpty(title))
            {
                int colonIdx = input.IndexOf(':');
                if (colonIdx >= 0 && colonIdx < input.Length - 1)
                    title = input.Substring(colonIdx + 1).Trim();
            }

            if (string.IsNullOrEmpty(title) || title.Length < 3)
            {
                return "What would you like the task to be called?\n"
                     + "Example: \"Add task: Review privacy settings\"";
            }

            // Build description based on common cybersecurity tasks
            string description = BuildTaskDescription(title.ToLower());

            _pendingTaskTitle       = title;
            _pendingTaskDescription = description;
            _awaitingReminderForTask = true;

            return "Task ready to add:\n"
                 + "  Title: " + title + "\n"
                 + "  Description: " + description + "\n\n"
                 + "Would you like a reminder for this task?\n"
                 + "Reply: \"Yes, remind me in 3 days\" / \"No reminder\" / \"Tomorrow\"";
        }

        private string HandleSetReminderRequest(string input, NlpResult nlp)
        {
            string title        = nlp.TaskTitle;
            string reminderInfo = nlp.ReminderInfo;

            if (string.IsNullOrEmpty(title) || title.Length < 3)
            {
                return "What task would you like the reminder for?\n"
                     + "Example: \"Remind me to update my password in 7 days\"";
            }

            if (string.IsNullOrEmpty(reminderInfo))
                reminderInfo = "Soon";

            string description = BuildTaskDescription(title.ToLower());

            TaskItem task = _taskManager.AddTask(title, description, reminderInfo);
            _logger.Log("Reminder set: '" + title + "' — " + reminderInfo);

            return "Reminder set!\n"
                 + "  Task: " + task.Title + "\n"
                 + "  Reminder: " + reminderInfo + "\n\n"
                 + "Your task has been saved. View it in the Tasks tab or type \"show tasks\".";
        }

        private string BuildTaskDescription(string titleLower)
        {
            if (titleLower.Contains("password") || titleLower.Contains("2fa") || titleLower.Contains("two-factor"))
                return "Enable or update authentication credentials to protect your accounts.";
            if (titleLower.Contains("privacy") || titleLower.Contains("settings"))
                return "Review account privacy settings to ensure your personal data is protected.";
            if (titleLower.Contains("antivirus") || titleLower.Contains("virus") || titleLower.Contains("malware"))
                return "Run a full system antivirus scan to detect and remove any malware.";
            if (titleLower.Contains("backup") || titleLower.Contains("back up"))
                return "Create a backup of important data following the 3-2-1 backup rule.";
            if (titleLower.Contains("update") || titleLower.Contains("patch"))
                return "Apply the latest software or firmware updates to close security vulnerabilities.";
            if (titleLower.Contains("vpn"))
                return "Set up or verify your VPN connection for secure browsing on public networks.";
            if (titleLower.Contains("phishing") || titleLower.Contains("email"))
                return "Review and secure your email settings to protect against phishing attacks.";

            return "Complete this cybersecurity task to stay protected online.";
        }

        // ── Task display helper ──────────────────────────────────────────────

        private string GetTasksText()
        {
            List<TaskItem> tasks = _taskManager.GetAllTasks();

            if (tasks.Count == 0)
                return "You have no tasks yet! Add some using the Tasks tab or type \"add task: [title]\".";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Your cybersecurity tasks:\n");

            foreach (TaskItem task in tasks)
            {
                string status  = task.IsCompleted ? "[DONE] " : "[TODO] ";
                string reminder = string.IsNullOrEmpty(task.ReminderDate)
                                  ? "" : "  Reminder: " + task.ReminderDate;
                sb.AppendLine(status + task.Title + reminder);
            }

            sb.AppendLine("\nUse the Tasks tab to delete or mark tasks as done.");
            return sb.ToString().TrimEnd();
        }

        // ── Quiz response helper (for chat-based quiz) ───────────────────────

        public string HandleQuizAnswer(string answer)
        {
            if (!_quiz.IsActive)
                return "No quiz is running. Type \"start quiz\" to begin!";

            var (correct, explanation, isGameOver) = _quiz.SubmitAnswer(answer);

            string feedback = correct
                ? "Correct! Well done.\n\n"
                : "Incorrect. The correct answer was: " + _quiz.GetCurrentQuestion()?.CorrectAnswer + "\n\n";

            // Note: after SubmitAnswer, GetCurrentQuestion returns the NEXT question
            // so we need to store the explanation before calling SubmitAnswer
            // (handled by returning explanation from SubmitAnswer)

            string resultText = feedback + "Explanation: " + explanation;

            if (isGameOver)
            {
                _logger.Log("Quiz completed — Score: " + _quiz.Score + "/" + _quiz.TotalQuestions);
                return resultText + "\n\n"
                     + "Quiz complete! Score: " + _quiz.Score + "/" + _quiz.TotalQuestions + "\n\n"
                     + _quiz.GetFinalFeedback();
            }

            _logger.Log("Quiz answer submitted — running score: " + _quiz.Score);
            QuizQuestion next = _quiz.GetCurrentQuestion();
            return resultText + "\n\n─────────────────────\n\n"
                 + FormatQuestion(next, _quiz.CurrentIndex);
        }

        private string FormatQuestion(QuizQuestion q, int number)
        {
            if (q == null) return "No more questions.";

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("Question " + number + " of " + _quiz.TotalQuestions + ":");
            sb.AppendLine();
            sb.AppendLine(q.QuestionText);
            sb.AppendLine();

            foreach (string opt in q.Options)
                sb.AppendLine("  " + opt);

            sb.AppendLine();

            if (q.IsMultiChoice)
                sb.Append("Type the letter of your answer (A, B, C, or D).");
            else
                sb.Append("Type True or False.");

            return sb.ToString();
        }
    }
}
