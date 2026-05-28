using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class MemoryStore
    {
        public string UserName { get; set; }
        public string FavouriteTopic { get; set; }

        private Dictionary<string, string> _memory;

        public MemoryStore()
        {
            UserName = "";
            FavouriteTopic = "";
            _memory = new Dictionary<string, string>();
        }

        // Store any key-value pair
        public void Store(string key, string value)
        {
            if (_memory.ContainsKey(key))
                _memory[key] = value;
            else
                _memory.Add(key, value);
        }

        // Retrieve a stored value by key
        public string Recall(string key)
        {
            if (_memory.ContainsKey(key))
                return _memory[key];
            return "";
        }

        // Build a personalised opening sentence using stored info
        public string GetPersonalisedOpener()
        {
            if (!string.IsNullOrEmpty(FavouriteTopic))
                return "As someone interested in " + FavouriteTopic + ", here is something useful: ";

            if (!string.IsNullOrEmpty(UserName))
                return UserName + ", ";

            return "";
        }
    }
}
