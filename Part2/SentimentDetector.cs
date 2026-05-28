using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    class SentimentDetector
    {
        private Dictionary<Sentiment, List<string>> _triggerWords;

        public SentimentDetector()
        {
            _triggerWords = new Dictionary<Sentiment, List<string>>();

            _triggerWords[Sentiment.Worried] = new List<string>
            {
                "worried", "scared", "afraid", "anxious", "nervous",
                "unsafe", "fear", "terrified", "concerned", "uneasy"
            };

            _triggerWords[Sentiment.Curious] = new List<string>
            {
                "curious", "wondering", "interested", "want to know",
                "how does", "what is", "tell me about", "explain", "learn"
            };

            _triggerWords[Sentiment.Frustrated] = new List<string>
            {
                "frustrated", "annoyed", "confused", "dont understand",
                "don't understand", "difficult", "hard", "complicated", "hate", "angry"
            };

            _triggerWords[Sentiment.Happy] = new List<string>
            {
                "great", "thanks", "thank you", "helpful",
                "awesome", "love it", "amazing", "perfect", "excellent"
            };
        }

        // Detect the sentiment of the user's input
        public Sentiment Detect(string input)
        {
            string lower = input.ToLower();

            foreach (Sentiment sentiment in _triggerWords.Keys)
            {
                foreach (string word in _triggerWords[sentiment])
                {
                    if (lower.Contains(word))
                        return sentiment;
                }
            }

            return Sentiment.Neutral;
        }

        // Return an empathetic opening sentence based on the detected sentiment
        public string GetSentimentResponse(Sentiment sentiment)
        {
            switch (sentiment)
            {
                case Sentiment.Worried:
                    return "It is completely understandable to feel that way. "
                         + "You are already taking the right step by learning about it.\n\n";

                case Sentiment.Curious:
                    return "Great question! Curiosity is the first step to staying safe online.\n\n";

                case Sentiment.Frustrated:
                    return "I understand this can feel overwhelming. "
                         + "Let me explain it as simply as possible.\n\n";

                case Sentiment.Happy:
                    return "Glad to hear that! Let us keep the momentum going.\n\n";

                default:
                    return "";
            }
        }
    }
}
