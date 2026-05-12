using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Enum representing the emotional tone detected in user input
    public enum Sentiment
    {
        Neutral,
        Worried,
        Curious,
        Frustrated,
        Happy
    }

    // Detects the user's sentiment from their message and returns
    // an empathetic opening sentence before the cybersecurity tip
    public class SentimentDetector
    {
        // Maps each sentiment to a list of trigger words
        private readonly Dictionary<Sentiment, List<string>> _triggerWords = new()
        {
            [Sentiment.Worried] = new List<string>
            {
                "worried", "scared", "afraid", "anxious", "nervous",
                "unsafe", "fear", "terrified", "concerned", "panic"
            },
            [Sentiment.Curious] = new List<string>
            {
                "curious", "wondering", "interested", "want to know",
                "how does", "what is", "tell me about", "explain", "learn"
            },
            [Sentiment.Frustrated] = new List<string>
            {
                "frustrated", "annoyed", "confused", "don't understand",
                "angry", "upset", "useless", "terrible", "hate"
            },
            [Sentiment.Happy] = new List<string>
            {
                "great", "thanks", "thank you", "helpful", "awesome",
                "love it", "amazing", "excellent", "perfect"
            }
        };

        // Empathetic opening sentences per sentiment
        private readonly Dictionary<Sentiment, string> _sentimentResponses = new()
        {
            [Sentiment.Worried]    = "It's completely understandable to feel that way. Cybersecurity can feel overwhelming, but small steps make a big difference. ",
            [Sentiment.Curious]    = "That's a great mindset to have! Curiosity is the first step to staying safe online. ",
            [Sentiment.Frustrated] = "I hear you — this stuff can be tricky. Let me try to explain it more clearly. ",
            [Sentiment.Happy]      = "I'm glad to hear that! Let's keep the good energy going. ",
            [Sentiment.Neutral]    = string.Empty
        };

        // Detects which sentiment matches the user's input
        // Returns Neutral if no sentiment trigger is found
        public Sentiment Detect(string input)
        {
            string lower = input.ToLower();

            foreach (var entry in _triggerWords)
            {
                foreach (string word in entry.Value)
                {
                    if (lower.Contains(word))
                        return entry.Key;
                }
            }

            return Sentiment.Neutral;
        }

        // Returns the empathetic opening sentence for a given sentiment
        public string GetSentimentResponse(Sentiment sentiment)
        {
            return _sentimentResponses.TryGetValue(sentiment, out string? response)
                ? response
                : string.Empty;
        }
    }
}
