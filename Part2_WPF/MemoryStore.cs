using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Stores information the user shares during the conversation
    // Used to personalise responses throughout the session
    public class MemoryStore
    {
        public string UserName { get; set; } = string.Empty;
        public string FavouriteTopic { get; set; } = string.Empty;

        // General key-value store for any additional user data
        private readonly Dictionary<string, string> _store = new();

        // Save any key-value pair to memory
        public void Store(string key, string value)
        {
            _store[key.ToLower()] = value;
        }

        // Retrieve a stored value by key
        public string Recall(string key)
        {
            return _store.TryGetValue(key.ToLower(), out string? value) ? value : string.Empty;
        }

        // Build a personalised opening sentence using stored information
        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrWhiteSpace(FavouriteTopic))
                return $"As someone interested in {FavouriteTopic}, you might want to know: ";

            if (!string.IsNullOrWhiteSpace(UserName))
                return $"{UserName}, here is something useful: ";

            return string.Empty;
        }
    }
}
