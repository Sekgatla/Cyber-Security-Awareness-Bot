using System;
using System.Collections.Generic;

namespace CybersecurityChatbot
{
    // Represents a single quiz question — either multiple-choice or true/false
    public class QuizQuestion
    {
        public string       QuestionText  { get; set; }
        public List<string> Options       { get; set; }   // e.g. {"A","B","C","D"} or {"True","False"}
        public string       CorrectAnswer { get; set; }   // e.g. "C" or "False"
        public string       Explanation   { get; set; }
        public bool         IsMultiChoice { get; set; }

        public QuizQuestion(string question, List<string> options,
                            string correctAnswer, string explanation, bool isMultiChoice = true)
        {
            QuestionText  = question;
            Options       = options;
            CorrectAnswer = correctAnswer;
            Explanation   = explanation;
            IsMultiChoice = isMultiChoice;
        }
    }

    // Manages quiz state: questions, current index, score, and feedback
    public class QuizEngine
    {
        private List<QuizQuestion> _questions;
        private int                _currentIndex;
        private int                _score;
        private Random             _random;

        public bool   IsActive      { get; private set; }
        public bool   IsFinished    { get; private set; }
        public int    TotalQuestions => _questions.Count;
        public int    Score          => _score;
        public int    CurrentIndex   => _currentIndex;

        public QuizEngine()
        {
            _random = new Random();
            _questions = BuildQuestions();
            Reset();
        }

        private List<QuizQuestion> BuildQuestions()
        {
            return new List<QuizQuestion>
            {
                // 1 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: Using HTTPS means a website is completely safe and cannot be used for phishing.",
                    new List<string> { "True", "False" },
                    "False",
                    "HTTPS only encrypts the connection — it does NOT guarantee the site is legitimate. Phishing sites can and do use HTTPS.",
                    false),

                // 2 — Multiple Choice
                new QuizQuestion(
                    "What does 2FA stand for in cybersecurity?",
                    new List<string>
                    {
                        "A) Two-Factor Authentication",
                        "B) Two-Form Access",
                        "C) Twice-Forced Authentication",
                        "D) Two-File Authorisation"
                    },
                    "A",
                    "Two-Factor Authentication (2FA) adds a second verification step so that a stolen password alone cannot unlock your account."),

                // 3 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: You should use the same password for all accounts to make it easier to remember.",
                    new List<string> { "True", "False" },
                    "False",
                    "Reusing passwords is dangerous. If one account is breached, all accounts sharing that password are immediately at risk.",
                    false),

                // 4 — Multiple Choice
                new QuizQuestion(
                    "What is the BEST action to take when you receive a suspicious email asking for your password?",
                    new List<string>
                    {
                        "A) Reply with your password",
                        "B) Delete the email",
                        "C) Report the email as phishing",
                        "D) Ignore it and do nothing"
                    },
                    "C",
                    "Reporting phishing emails helps your email provider and security teams protect others. Simply deleting it leaves the threat active for everyone else."),

                // 5 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: It is safe to access your internet banking on a public Wi-Fi network without a VPN.",
                    new List<string> { "True", "False" },
                    "False",
                    "Public Wi-Fi is unencrypted. Attackers on the same network can intercept your traffic. Always use a VPN or mobile data for banking.",
                    false),

                // 6 — Multiple Choice
                new QuizQuestion(
                    "What is RANSOMWARE?",
                    new List<string>
                    {
                        "A) A type of antivirus software",
                        "B) Malware that encrypts your files and demands payment to restore them",
                        "C) A firewall that blocks hackers",
                        "D) A secure password manager"
                    },
                    "B",
                    "Ransomware encrypts your files and holds them hostage. The best defence is regular offline backups following the 3-2-1 rule."),

                // 7 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: Authenticator apps (like Google Authenticator) are more secure than SMS-based 2FA.",
                    new List<string> { "True", "False" },
                    "True",
                    "SMS can be intercepted via SIM-swap attacks. Authenticator apps generate codes locally and are not vulnerable to SIM-swap fraud.",
                    false),

                // 8 — Multiple Choice
                new QuizQuestion(
                    "What does VPN stand for?",
                    new List<string>
                    {
                        "A) Virtual Private Network",
                        "B) Verified Protected Node",
                        "C) Visual Privacy Network",
                        "D) Virtual Protocol Navigator"
                    },
                    "A",
                    "A Virtual Private Network (VPN) encrypts your internet traffic and masks your IP address, protecting your privacy on public or untrusted networks."),

                // 9 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: You should share your OTP (one-time PIN) with someone claiming to be from your bank if they call you.",
                    new List<string> { "True", "False" },
                    "False",
                    "Your bank will NEVER ask for your OTP over the phone. Anyone requesting your OTP is attempting fraud.",
                    false),

                // 10 — Multiple Choice
                new QuizQuestion(
                    "Which of the following passwords is the STRONGEST?",
                    new List<string>
                    {
                        "A) password123",
                        "B) MyDog2020",
                        "C) Tr0ub4dor&3_Rnd!",
                        "D) abc12345"
                    },
                    "C",
                    "A strong password is long, random, and uses mixed characters. 'Tr0ub4dor&3_Rnd!' is long, contains symbols, numbers, and uppercase — much harder to crack."),

                // 11 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: Social engineering attacks primarily exploit technical software vulnerabilities in operating systems.",
                    new List<string> { "True", "False" },
                    "False",
                    "Social engineering targets HUMAN psychology — not software bugs. Attackers manipulate people using fear, urgency, and trust rather than hacking systems directly.",
                    false),

                // 12 — Multiple Choice
                new QuizQuestion(
                    "What is PHISHING?",
                    new List<string>
                    {
                        "A) A type of antivirus malware",
                        "B) A fraudulent attempt to steal sensitive info through fake messages or websites",
                        "C) A tool used to scan networks for open ports",
                        "D) A method of encrypting files securely"
                    },
                    "B",
                    "Phishing tricks victims into revealing passwords or financial details by impersonating trusted organisations in emails, SMS, or fake websites."),

                // 13 — True/False
                new QuizQuestion(
                    "TRUE or FALSE: Plugging in a USB drive you found in a parking lot is generally safe.",
                    new List<string> { "True", "False" },
                    "False",
                    "Attackers deliberately leave infected USB drives in public places. Plugging one in can instantly install malware. This is called a USB drop attack.",
                    false),

                // 14 — Multiple Choice
                new QuizQuestion(
                    "Which organisation should South Africans report banking fraud to?",
                    new List<string>
                    {
                        "A) ICASA",
                        "B) SABRIC (South African Banking Risk Information Centre)",
                        "C) SARS",
                        "D) SABC"
                    },
                    "B",
                    "SABRIC (www.sabric.co.za) is the dedicated body for reporting banking-related fraud and cybercrime in South Africa."),
            };
        }

        public void Reset()
        {
            _currentIndex = 0;
            _score        = 0;
            IsActive      = false;
            IsFinished    = false;
            ShuffleQuestions();
        }

        private void ShuffleQuestions()
        {
            for (int i = _questions.Count - 1; i > 0; i--)
            {
                int j = _random.Next(0, i + 1);
                QuizQuestion temp = _questions[i];
                _questions[i]    = _questions[j];
                _questions[j]    = temp;
            }
        }

        public void Start()
        {
            Reset();
            IsActive   = true;
            IsFinished = false;
        }

        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex < _questions.Count)
                return _questions[_currentIndex];
            return null;
        }

        // Returns (isCorrect, explanation, isGameOver)
        public (bool isCorrect, string explanation, bool isGameOver) SubmitAnswer(string answer)
        {
            if (!IsActive || IsFinished) return (false, "No quiz is active.", true);

            QuizQuestion current = GetCurrentQuestion();
            if (current == null)         return (false, "No question found.", true);

            bool correct = answer.Trim().Equals(current.CorrectAnswer, StringComparison.OrdinalIgnoreCase);
            if (correct) _score++;

            _currentIndex++;
            bool done = _currentIndex >= _questions.Count;
            if (done) { IsActive = false; IsFinished = true; }

            return (correct, current.Explanation, done);
        }

        public string GetFinalFeedback()
        {
            double pct = (double)_score / TotalQuestions * 100;

            if (pct == 100)
                return "Perfect score! You are a true cybersecurity pro! " +
                       "Your knowledge will keep you and those around you safe online.";
            if (pct >= 80)
                return "Excellent! You have a strong grasp of cybersecurity fundamentals. " +
                       "Keep it up — staying informed is your best defence!";
            if (pct >= 60)
                return "Good effort! You know the basics, but there is room to grow. " +
                       "Review the topics you missed and try again.";
            if (pct >= 40)
                return "You are on your way! Spend some time exploring the chat topics " +
                       "to strengthen your cybersecurity knowledge.";
            return "Keep learning to stay safe online! Cybersecurity threats evolve daily — " +
                   "knowledge is your best weapon. Try the quiz again after exploring more topics.";
        }
    }
}
