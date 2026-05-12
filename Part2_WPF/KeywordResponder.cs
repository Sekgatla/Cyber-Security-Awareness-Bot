using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Handles keyword recognition and randomly selected responses
    // Uses a Dictionary where each key is a cybersecurity keyword
    // and the value is a list of possible responses
    public class KeywordResponder
    {
        private readonly Random _random = new();

        private readonly Dictionary<string, List<string>> _responses = new()
        {
            ["password"] = new List<string>
            {
                "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
                "A strong password should be at least 12 characters long with uppercase letters, lowercase letters, numbers, and symbols.",
                "Consider using a password manager to generate and store complex passwords — you only need to remember one master password."
            },
            ["phishing"] = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Always verify the sender's email address before clicking any links. Legitimate companies will never ask for your password via email.",
                "If an email seems too good to be true or creates urgency, it is likely a phishing attempt. Go directly to the official website instead."
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
                "Ensure websites you visit use HTTPS — the S stands for Secure and means your connection is encrypted."
            },
            ["ransomware"] = new List<string>
            {
                "Ransomware encrypts your files and demands payment for the key. Keep regular backups so you can recover without paying.",
                "Never open attachments or links from unknown sources — ransomware is often delivered through phishing emails.",
                "Keep your operating system and software up to date to patch vulnerabilities that ransomware exploits."
            },
            ["two-factor"] = new List<string>
            {
                "Two-factor authentication (2FA) adds an extra layer of security beyond just your password.",
                "Even if your password is stolen, 2FA can prevent attackers from accessing your account.",
                "Use an authenticator app like Google Authenticator instead of SMS-based 2FA where possible — it is more secure."
            }
        };

        // Loops through all keywords and returns a randomly selected response
        // if the user's input contains a matching keyword
        public string GetResponse(string input)
        {
            string lower = input.ToLower();

            foreach (var entry in _responses)
            {
                if (lower.Contains(entry.Key))
                {
                    List<string> options = entry.Value;
                    return options[_random.Next(options.Count)];
                }
            }

            return string.Empty;
        }

        // Returns the keyword that was matched in the input, or empty string
        public string GetMatchedKeyword(string input)
        {
            string lower = input.ToLower();
            foreach (string key in _responses.Keys)
            {
                if (lower.Contains(key))
                    return key;
            }
            return string.Empty;
        }

        // Returns a list of all supported keywords (used for 'what can I ask' response)
        public List<string> GetAllKeywords()
        {
            return new List<string>(_responses.Keys);
        }

        // Returns a random response for a specific keyword (used for follow-ups)
        public string GetResponseForKeyword(string keyword)
        {
            if (_responses.TryGetValue(keyword, out var list))
                return list[_random.Next(list.Count)];
            return string.Empty;
        }
    }
}
