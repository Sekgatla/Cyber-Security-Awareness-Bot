using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    // Stores information about the current user for memory and recall
    public class User
    {
        public string Name { get; set; } = string.Empty;
        public string? FavouriteTopic { get; set; }
        public List<string> MentionedTopics { get; set; } = new List<string>();
    }
}
