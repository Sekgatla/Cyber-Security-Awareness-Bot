using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class ChatBot
    {
        private KeywordResponder _keywords;
        private SentimentDetector _sentiment;
        private MemoryStore _memory;

        private bool _awaitingName;
        private string _lastTopic;

        private Random _random;

        public ChatBot()
        {
            _keywords  = new KeywordResponder();
            _sentiment = new SentimentDetector();
            _memory    = new MemoryStore();

            _awaitingName = true;
            _lastTopic    = "";
            _random       = new Random();
        }

        // Opening message shown before the user types anything
        public string GetGreeting()
        {
            return "Hello! I am your Cybersecurity Awareness Assistant.\n"
                 + "I am here to help South African citizens stay safe online.\n\n"
                 + "Before we begin — what is your name?";
        }

        // Central routing method — called by MainWindow for every user message
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

                return "Welcome, " + _memory.UserName + "! Great to have you here.\n\n"
                     + "I can help you with these cybersecurity topics:\n"
                     + "  • Passwords          • Phishing\n"
                     + "  • Scams              • Privacy\n"
                     + "  • Malware            • 2FA\n"
                     + "  • Public Wi-Fi       • Social Engineering\n\n"
                     + "Just type a topic or ask me a question to get started!";
            }

            // ── Step 2: Detect and store favourite topic interest ─────────────
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
                        return "Great! I will remember that you are interested in " + kw + ". "
                             + "It is a crucial part of staying safe online.\n\n"
                             + topicResponse;
                    }
                }
            }

            // ── Step 3: Handle follow-up requests ────────────────────────────
            if (lower.Contains("tell me more") || lower.Contains("explain more")
                || lower.Contains("more info")  || lower.Contains("another tip")
                || lower.Contains("give me more") || lower.Contains("more detail")
                || lower.Contains("go on")      || lower.Contains("keep going"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    string followUp = _keywords.GetResponseByKeyword(_lastTopic);
                    if (followUp != null)
                        return _memory.GetPersonalisedOpener()
                             + "here is another tip on " + _lastTopic + ":\n\n"
                             + followUp;
                }
                return "Could you let me know which topic you would like more information on?";
            }

            // ── Step 4: Detect sentiment ─────────────────────────────────────
            Sentiment sentiment       = _sentiment.Detect(input);
            string    sentimentOpener = _sentiment.GetSentimentResponse(sentiment);

            // ── Step 5: Match a keyword and return a response ─────────────────
            string keywordResponse = _keywords.GetResponse(input);
            if (keywordResponse != null)
            {
                // Remember which topic was just discussed for follow-ups
                foreach (string kw in _keywords.GetAllKeywords())
                {
                    if (lower.Contains(kw))
                    {
                        _lastTopic = kw;
                        break;
                    }
                }

                string personalised = _memory.GetPersonalisedOpener();
                return sentimentOpener + personalised + keywordResponse;
            }

            // ── Step 6: General questions ────────────────────────────────────
            if (lower.Contains("how are you"))
            {
                return "I am running at full capacity — all systems green!\n"
                     + "More importantly, are YOU staying safe online, " + _memory.UserName + "?";
            }

            if (lower.Contains("what can you") || lower.Contains("what do you")
                || lower.Contains("purpose")   || lower.Contains("topics")
                || lower.Contains("help"))
            {
                return "Here are all the cybersecurity topics I can help you with:\n\n"
                     + "  • Passwords          • Phishing\n"
                     + "  • Scams              • Privacy\n"
                     + "  • Malware            • 2FA\n"
                     + "  • Public Wi-Fi       • Social Engineering\n\n"
                     + "Just type any topic or ask a question!";
            }

            if (lower.Contains("bye") || lower.Contains("goodbye")
                || lower.Contains("exit") || lower.Contains("quit"))
            {
                return "Thank you for chatting, " + _memory.UserName + "!\n"
                     + "Stay alert, stay safe. Cybersecurity is everyone's responsibility.\n\n"
                     + "Report cybercrime : www.saps.gov.za  |  Emergency: 10111\n"
                     + "Banking fraud (SABRIC): www.sabric.co.za";
            }

            // ── Step 7: Fallback for unrecognised input ───────────────────────
            string[] fallbacks = new string[]
            {
                "I am not sure I understand. Could you try rephrasing?",
                "I did not quite catch that. Try asking about passwords, phishing, or malware.",
                "Hmm, I am not sure about that. Type 'help' to see all topics I can assist with.",
                "I specialise in cybersecurity. Could you ask me about a cybersecurity topic?"
            };

            return fallbacks[_random.Next(0, fallbacks.Length)];
        }
    }
}
