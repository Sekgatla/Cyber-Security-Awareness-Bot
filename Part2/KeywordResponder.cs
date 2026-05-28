using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    class KeywordResponder
    {
        private Dictionary<string, List<string>> _responses;
        private Random _random;

        public KeywordResponder()
        {
            _random = new Random();
            _responses = new Dictionary<string, List<string>>();

            _responses["password"] = new List<string>
            {
                "Use at least 12 characters with a mix of uppercase, lowercase, numbers, and symbols.\nExample: P@ssw0rd!23",
                "Never reuse the same password across different accounts. Each account deserves a unique password.",
                "Use a passphrase — it is longer and easier to remember.\nExample: Coffee!Rain_Dog72",
                "Store your passwords in a trusted password manager like Bitwarden or LastPass — never in a notebook.",
                "Change your passwords immediately if you suspect they have been compromised or leaked."
            };

            _responses["phishing"] = new List<string>
            {
                "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
                "Always hover over links before clicking. A legitimate company will never ask for your password via email.",
                "Check the sender's address carefully. Scammers use addresses like 'support@amaz0n.fake.com'.",
                "If an email creates urgency — 'Your account will be closed!' — slow down. It is likely a phishing attempt.",
                "Never click links in unexpected emails. Go directly to the company's website by typing the URL yourself."
            };

            _responses["scam"] = new List<string>
            {
                "If an offer sounds too good to be true, it almost certainly is. Trust your instincts.",
                "Never send money or share banking details with someone you have only met online.",
                "South African banking fraud is rising. Report scams to SABRIC at www.sabric.co.za",
                "Common scams in SA include lottery scams, job offer scams, and romance scams. Stay alert.",
                "Never share your OTP with anyone — not even someone claiming to be from your bank."
            };

            _responses["privacy"] = new List<string>
            {
                "Review your social media privacy settings regularly. Limit who can see your personal information.",
                "Avoid sharing your ID number, address, or date of birth publicly online.",
                "Use a VPN (Virtual Private Network) when browsing on public Wi-Fi to protect your privacy.",
                "Read privacy policies before using apps — understand exactly what data they collect and share.",
                "Enable two-factor authentication to add an extra layer of protection to your private accounts."
            };

            _responses["malware"] = new List<string>
            {
                "Install reputable antivirus software and keep it updated to protect against all forms of malware.",
                "Never download software from untrusted websites. Always use official and verified sources.",
                "Ransomware encrypts your files and demands payment. Back up your data regularly — follow the 3-2-1 rule.",
                "Do not open email attachments from unknown senders — they are a common delivery method for malware.",
                "Keep your operating system and apps updated. Updates patch the security vulnerabilities attackers exploit."
            };

            _responses["2fa"] = new List<string>
            {
                "Two-factor authentication adds a second layer of security. Even if your password is stolen, your account stays safe.",
                "Use an authenticator app like Google Authenticator or Microsoft Authenticator — it is stronger than SMS.",
                "SMS-based 2FA is convenient, but authenticator apps are significantly more secure against SIM-swap attacks.",
                "Enable 2FA on all important accounts: banking, email, and social media should be your starting point.",
                "Never share your OTP (one-time PIN) with anyone — your bank will never ask for it over the phone."
            };

            _responses["wifi"] = new List<string>
            {
                "Avoid accessing banking or sensitive accounts on public Wi-Fi. Use your mobile data instead.",
                "Always use a VPN when connecting to public Wi-Fi — it encrypts your internet traffic.",
                "Hackers create fake hotspots with familiar names like 'CoffeShop_Free'. Always verify the network first.",
                "Turn off auto-connect to Wi-Fi on your phone so it does not silently join rogue networks.",
                "Only visit HTTPS websites when on public Wi-Fi — the padlock icon means the connection is encrypted."
            };

            _responses["social engineering"] = new List<string>
            {
                "Social engineering attacks manipulate people rather than systems. Always verify who you are speaking to.",
                "Your bank will NEVER ask for your PIN, full password, or card number over the phone or via email.",
                "If someone pressures you to act quickly, slow down. Urgency and fear are classic manipulation tactics.",
                "Do not plug in unknown USB drives — attackers leave infected USBs in public places on purpose.",
                "Always verify the identity of anyone requesting sensitive info, even if they claim to be a colleague."
            };
        }

        // Return a randomly selected response for any keyword found in the input
        public string GetResponse(string input)
        {
            string lower = input.ToLower();

            foreach (string key in _responses.Keys)
            {
                if (lower.Contains(key))
                {
                    List<string> list = _responses[key];
                    int index = _random.Next(0, list.Count);
                    return list[index];
                }
            }

            return null;
        }

        // Return a random response for a specific keyword (used for follow-ups and memory recall)
        public string GetResponseByKeyword(string keyword)
        {
            if (_responses.ContainsKey(keyword))
            {
                List<string> list = _responses[keyword];
                int index = _random.Next(0, list.Count);
                return list[index];
            }

            return null;
        }

        // Return a list of all recognised keywords (used for the help/topics response)
        public List<string> GetAllKeywords()
        {
            return new List<string>(_responses.Keys);
        }
    }
}
