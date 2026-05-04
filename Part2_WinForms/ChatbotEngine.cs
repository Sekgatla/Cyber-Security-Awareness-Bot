using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    // Handles all chatbot logic: keyword recognition, sentiment detection,
    // random responses, conversation flow, and memory recall
    public class ChatbotEngine
    {
        private readonly Random _random = new Random();

        // Keyword responses - each keyword has multiple responses for variety
        private readonly Dictionary<string, List<string>> _keywordResponses = new()
        {
            ["password"] = new List<string>
            {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                "A strong password should be at least 12 characters long and include uppercase letters, lowercase letters, numbers, and symbols.",
                "Consider using a password manager to generate and store complex passwords safely — you only need to remember one master password."
            },
            ["phishing"] = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Always verify the sender's email address before clicking any links. Legitimate companies will never ask for your password via email.",
                "If an email seems too good to be true or creates urgency, it is likely a phishing attempt. When in doubt, go directly to the official website."
            },
            ["privacy"] = new List<string>
            {
                "Review the privacy settings on your social media accounts regularly to control who can see your information.",
                "Limit the personal information you share online. The less data you expose, the smaller your attack surface.",
                "Use privacy-focused browsers and search engines, and consider a VPN when using public Wi-Fi to protect your data."
            },
            ["scam"] = new List<string>
            {
                "Be wary of unsolicited calls or messages claiming you have won a prize or owe money. Verify directly with the official organisation.",
                "Scammers often create a sense of urgency — take a breath and verify independently before acting on any suspicious request.",
                "Never transfer money or share banking details with someone you have not verified through official channels."
            },
            ["malware"] = new List<string>
            {
                "Keep your antivirus software up to date and run regular scans to detect and remove malicious software.",
                "Avoid downloading files or software from untrusted sources. Malware is often hidden in free downloads.",
                "Be cautious with email attachments — even from people you know, as their accounts may have been compromised."
            },
            ["vpn"] = new List<string>
            {
                "A VPN (Virtual Private Network) encrypts your internet traffic, making it harder for others to spy on your online activity.",
                "Use a reputable VPN, especially on public Wi-Fi networks like coffee shops or airports, to protect your data.",
                "A VPN hides your IP address and location, adding an extra layer of privacy to your browsing."
            },
            ["firewall"] = new List<string>
            {
                "A firewall acts as a barrier between your trusted network and untrusted external networks — keep it enabled at all times.",
                "Both hardware and software firewalls are important. Ensure your operating system's built-in firewall is active.",
                "Configure your firewall to block inbound connections you do not need, reducing your exposure to attacks."
            },
            ["encryption"] = new List<string>
            {
                "Encryption converts your data into an unreadable format, so only authorised parties with the key can access it.",
                "Use end-to-end encrypted messaging apps like Signal for sensitive communications.",
                "Ensure websites you visit use HTTPS — the S stands for secure and means your connection is encrypted."
            },
            ["ransomware"] = new List<string>
            {
                "Ransomware encrypts your files and demands payment for the key. Keep regular backups so you can recover without paying.",
                "Never open attachments or links from unknown sources — ransomware is often delivered through phishing emails.",
                "Keep your operating system and software up to date to patch vulnerabilities that ransomware exploits."
            },
            ["safe browsing"] = new List<string>
            {
                "Always look for HTTPS and a padlock icon in the address bar before entering sensitive information on a website.",
                "Avoid clicking on pop-up ads or suspicious links. Stick to reputable websites and keep your browser updated.",
                "Use browser extensions like ad-blockers and script blockers to reduce your exposure to malicious content."
            }
        };

        // Sentiment keyword patterns
        private readonly Dictionary<string, List<string>> _sentimentPatterns = new()
        {
            ["worried"]     = new List<string> { "worried", "scared", "afraid", "anxious", "nervous", "fear", "terrified", "concerned" },
            ["frustrated"]  = new List<string> { "frustrated", "annoyed", "angry", "upset", "confused", "don't understand", "useless" },
            ["curious"]     = new List<string> { "curious", "interested", "want to know", "tell me more", "how does", "what is", "learn" },
            ["overwhelmed"] = new List<string> { "overwhelmed", "too much", "complicated", "difficult", "can't keep up", "lost", "stuck" }
        };

        // Empathetic prefix responses based on detected sentiment
        private readonly Dictionary<string, string> _sentimentResponses = new()
        {
            ["worried"]     = "It's completely understandable to feel that way. Cybersecurity can feel overwhelming, but small steps make a big difference. ",
            ["frustrated"]  = "I hear you — this stuff can be tricky. Let me try to explain it more clearly. ",
            ["curious"]     = "That's a great mindset to have! Curiosity is the first step to staying safe online. ",
            ["overwhelmed"] = "Don't worry — you don't have to tackle everything at once. Let's focus on one thing at a time. "
        };

        // Phrases that trigger a follow-up response on the same topic
        private readonly List<string> _followUpTriggers = new()
        {
            "give me another tip", "tell me more", "explain more", "more info",
            "another one", "what else", "keep going", "go on", "continue"
        };

        // Detects sentiment from user input
        public string? DetectSentiment(string input)
        {
            string lower = input.ToLower();
            foreach (var entry in _sentimentPatterns)
            {
                foreach (string pattern in entry.Value)
                {
                    if (lower.Contains(pattern))
                        return entry.Key;
                }
            }
            return null;
        }

        // Detects cybersecurity keyword from user input
        public string? DetectKeyword(string input)
        {
            string lower = input.ToLower();
            foreach (string keyword in _keywordResponses.Keys)
            {
                if (lower.Contains(keyword))
                    return keyword;
            }
            return null;
        }

        // Checks whether input is a follow-up request
        public bool IsFollowUp(string input)
        {
            string lower = input.ToLower();
            foreach (string trigger in _followUpTriggers)
            {
                if (lower.Contains(trigger))
                    return true;
            }
            return false;
        }

        // Returns a random response for a given keyword
        public string GetKeywordResponse(string keyword)
        {
            if (_keywordResponses.TryGetValue(keyword, out var responses))
                return responses[_random.Next(responses.Count)];
            return string.Empty;
        }

        // Generates a full response based on input, user memory, and last keyword
        public string GenerateResponse(string input, User user, ref string? lastKeyword)
        {
            string lower = input.Trim().ToLower();

            if (string.IsNullOrWhiteSpace(lower))
                return "Please type something so I can help you!";

            string? sentiment = DetectSentiment(lower);
            string sentimentPrefix = sentiment != null ? _sentimentResponses[sentiment] : string.Empty;

            // Exit
            if (lower == "exit" || lower == "bye" || lower == "goodbye")
                return $"Goodbye{(user.Name.Length > 0 ? ", " + user.Name : string.Empty)}! Stay safe online. Remember — cybersecurity is everyone's responsibility.";

            // Greeting
            if (lower.Contains("how are you"))
                return "I'm just a program, but I'm here to help you stay safe online! What cybersecurity topic can I help you with today?";

            // Purpose
            if (lower.Contains("purpose") || lower.Contains("what do you do") || lower.Contains("what can you do"))
                return "My purpose is to educate you about cybersecurity threats and best practices. Ask me about passwords, phishing, privacy, scams, malware, VPNs, encryption, and more!";

            // Help / topics list
            if (lower.Contains("help") || lower.Contains("topics"))
                return "I can help you with: passwords, phishing, privacy, scams, malware, VPN, firewall, encryption, ransomware, and safe browsing. What would you like to know?";

            // Memory: user shares interest
            if (lower.Contains("i'm interested in") || lower.Contains("i am interested in") || lower.Contains("i like"))
            {
                string[] prefixes = { "i'm interested in", "i am interested in", "i like" };
                foreach (string prefix in prefixes)
                {
                    int idx = lower.IndexOf(prefix);
                    if (idx >= 0)
                    {
                        string topic = input.Substring(idx + prefix.Length).Trim().TrimEnd('.');
                        user.FavouriteTopic = topic;
                        if (!user.MentionedTopics.Contains(topic))
                            user.MentionedTopics.Add(topic);
                        return $"Great! I'll remember that you're interested in {topic}. It's a crucial part of staying safe online. Would you like some tips on that?";
                    }
                }
            }

            // Memory: user asks what bot remembers
            if (lower.Contains("remember me") || lower.Contains("who am i") || (lower.Contains("my name") && lower.Contains("?")))
            {
                string memory = $"Your name is {user.Name}.";
                if (user.FavouriteTopic != null)
                    memory += $" You mentioned you're interested in {user.FavouriteTopic}.";
                return memory;
            }

            // Follow-up on last topic
            if (IsFollowUp(lower))
            {
                if (lastKeyword != null)
                    return sentimentPrefix + $"Here's another tip on {lastKeyword}: {GetKeywordResponse(lastKeyword)}";
                return "Sure! What cybersecurity topic would you like to explore? Type 'help' to see all topics.";
            }

            // Keyword recognition
            string? keyword = DetectKeyword(lower);
            if (keyword != null)
            {
                lastKeyword = keyword;
                if (!user.MentionedTopics.Contains(keyword))
                    user.MentionedTopics.Add(keyword);
                return sentimentPrefix + GetKeywordResponse(keyword);
            }

            // Memory: reference favourite topic in response
            if (user.FavouriteTopic != null && lower.Length > 2)
            {
                string fav = user.FavouriteTopic.ToLower();
                string? favKeyword = DetectKeyword(fav);
                if (favKeyword != null)
                    return $"As someone interested in {user.FavouriteTopic}, you might want to know: {GetKeywordResponse(favKeyword)}";
            }

            // Default / error handling
            return sentimentPrefix + "I'm not sure I understand that. Could you try rephrasing? Type 'help' to see what topics I can assist with.";
        }
    }
}
