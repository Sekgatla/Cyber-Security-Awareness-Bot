using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Central chatbot class. MainWindow.xaml.cs only calls ProcessInput().
    // All routing logic lives here — no logic should appear in the code-behind.
    public class ChatBot
    {
        private readonly KeywordResponder _keywords;
        private readonly SentimentDetector _sentiment;
        private readonly MemoryStore _memory;

        // Tracks whether the bot is still waiting for the user's name
        private bool _awaitingName = true;

        // Tracks the last cybersecurity topic discussed (for follow-up handling)
        private string _lastTopic = string.Empty;

        // Phrases that trigger a follow-up response without resetting the topic
        private readonly List<string> _followUpPhrases = new()
        {
            "tell me more", "explain more", "more info", "give me another tip",
            "another one", "what else", "keep going", "go on", "continue"
        };

        public ChatBot()
        {
            _keywords = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory    = new MemoryStore();
        }

        // Returns the opening message shown when the app starts
        public string GetGreeting()
        {
            return "Hello! Welcome to the Cybersecurity Awareness Bot.\n" +
                   "Before we begin, what is your name?";
        }

        // Main routing method — called by MainWindow.xaml.cs for every user message
        // Processes input in strict order as per the assessment requirements
        public string ProcessInput(string userInput)
        {
            string trimmed = userInput.Trim();

            if (string.IsNullOrWhiteSpace(trimmed))
                return "Please type something so I can help you.";

            string lower = trimmed.ToLower();

            // ── Step 1: Capture user name ──────────────────────────────────────
            if (_awaitingName)
            {
                _memory.UserName = trimmed;
                _memory.Store("name", trimmed);
                _awaitingName = false;
                return $"Hello {_memory.UserName}! It's great to meet you.\n" +
                       "I'm here to help you stay safe online. You can ask me about\n" +
                       "passwords, phishing, privacy, scams, malware, VPNs, and more.\n" +
                       "Type 'help' to see all available topics.";
            }

            // ── Step 2: Follow-up handling ─────────────────────────────────────
            foreach (string phrase in _followUpPhrases)
            {
                if (lower.Contains(phrase))
                {
                    if (!string.IsNullOrWhiteSpace(_lastTopic))
                        return $"Here is another tip on {_lastTopic}:\n" +
                               _keywords.GetResponseForKeyword(_lastTopic);

                    return "Sure! What cybersecurity topic would you like to explore?\n" +
                           "Type 'help' to see all available topics.";
                }
            }

            // ── Step 3: Sentiment detection ────────────────────────────────────
            Sentiment detectedSentiment = _sentiment.Detect(lower);
            string sentimentOpener = _sentiment.GetSentimentResponse(detectedSentiment);

            // ── Step 4: Keyword recognition ────────────────────────────────────
            string keywordResponse = _keywords.GetResponse(lower);
            if (!string.IsNullOrWhiteSpace(keywordResponse))
            {
                string matchedKeyword = _keywords.GetMatchedKeyword(lower);
                _lastTopic = matchedKeyword;

                // Store favourite topic if user expressed interest
                if (lower.Contains("interested in") || lower.Contains("i like"))
                    _memory.FavouriteTopic = matchedKeyword;

                string opener = !string.IsNullOrWhiteSpace(_memory.FavouriteTopic) && _memory.FavouriteTopic == matchedKeyword
                    ? _memory.GetPersonalisedOpener()
                    : sentimentOpener;

                return opener + keywordResponse;
            }

            // ── Step 5: Special phrase handling ───────────────────────────────
            if (lower.Contains("how are you"))
                return "I'm just a program, but I'm here to help you stay safe online!\n" +
                       "What cybersecurity topic can I help you with today?";

            if (lower.Contains("what can you do") || lower.Contains("purpose") || lower.Contains("help"))
            {
                string topics = string.Join(", ", _keywords.GetAllKeywords());
                return $"I can help you learn about these cybersecurity topics:\n{topics}\n\n" +
                       "Just ask me anything about them!";
            }

            if (lower.Contains("my name is") || lower.Contains("i am ") || lower.Contains("i'm "))
            {
                string[] parts = lower.Split(new[] { "my name is", "i am ", "i'm " },
                    System.StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    string name = trimmed.Substring(trimmed.LastIndexOf(' ') + 1);
                    _memory.UserName = name;
                    _memory.Store("name", name);
                    return $"Nice to meet you, {name}! How can I help you stay safe online today?";
                }
            }

            if (lower.Contains("i'm interested in") || lower.Contains("i am interested in") || lower.Contains("i like"))
            {
                string[] prefixes = { "i'm interested in", "i am interested in", "i like" };
                foreach (string prefix in prefixes)
                {
                    int idx = lower.IndexOf(prefix);
                    if (idx >= 0)
                    {
                        string topic = trimmed.Substring(idx + prefix.Length).Trim().TrimEnd('.');
                        _memory.FavouriteTopic = topic;
                        _memory.Store("favouriteTopic", topic);
                        return $"Great! I'll remember that you're interested in {topic}.\n" +
                               "It's a crucial part of staying safe online.\n" +
                               "Would you like some tips on that topic?";
                    }
                }
            }

            if (lower.Contains("remember") || lower.Contains("who am i"))
            {
                string recall = $"Your name is {_memory.UserName}.";
                if (!string.IsNullOrWhiteSpace(_memory.FavouriteTopic))
                    recall += $" You mentioned you're interested in {_memory.FavouriteTopic}.";
                return recall;
            }

            // ── Step 6: Fallback response ──────────────────────────────────────
            return sentimentOpener +
                   "I'm not sure I understand that. Could you try rephrasing?\n" +
                   "Type 'help' to see what topics I can assist with.";
        }
    }
}
