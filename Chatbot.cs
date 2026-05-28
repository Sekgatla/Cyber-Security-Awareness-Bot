using System;

  namespace CyberSecurityChatbot
  {
      public class Chatbot
      {
          // Current user session
          private User _user = new User();

          // Track the last discussed topic for follow-up requests
          private string _lastTopic = string.Empty;

          // ── Entry point ──────────────────────────────────────────────

          public void Start()
          {
              DisplayHeader();
              GetUserName();
              RunChat();
              DisplayGoodbye();
          }

          // ── ASCII art header ─────────────────────────────────────────

          private void DisplayHeader()
          {
              Console.Clear();
              ConsoleHelper.DrawBorder();

              Console.ForegroundColor = ConsoleColor.Green;
              Console.WriteLine();
              Console.WriteLine(@"   ____      _               ____            ");
              Console.WriteLine(@"  / ___|   _| |__   ___ _ __/ ___|  ___  ___ ");
              Console.WriteLine(@" | |  | | | | '_ \ / _ \ '__\___ \ / _ \/ __|");
              Console.WriteLine(@" | |__| |_| | |_) |  __/ |   ___) |  __/ (__ ");
              Console.WriteLine(@"  \____\__, |_.__/ \___|_|  |____/ \___|\___|");
              Console.WriteLine(@"       |___/                                  ");
              Console.WriteLine();
              Console.ResetColor();

              Console.ForegroundColor = ConsoleColor.DarkCyan;
              Console.WriteLine("       CYBERSECURITY AWARENESS BOT");
              Console.ForegroundColor = ConsoleColor.DarkGray;
              Console.WriteLine("   [ Dept of Cybersecurity - South Africa ]");
              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.WriteLine("            Created by Sekgatla");
              Console.ResetColor();

              ConsoleHelper.DrawBorder();
              Console.WriteLine();
          }

          // ── Name input with full validation ──────────────────────────

          private void GetUserName()
          {
              ConsoleHelper.WriteInfo("  Please enter your name to begin:");
              ConsoleHelper.WriteMuted("  (For educational purposes only.)");

              while (true)
              {
                  string input = ConsoleHelper.Prompt("Your name");

                  if (string.IsNullOrWhiteSpace(input))
                  {
                      ConsoleHelper.WriteWarning("  [INVALID] Name cannot be empty. Please try again.");
                      continue;
                  }

                  input = input.Trim();

                  if (input.Length < 2)
                  {
                      ConsoleHelper.WriteWarning("  [INVALID] Name must be at least 2 characters.");
                      continue;
                  }

                  if (input.Length > 50)
                  {
                      ConsoleHelper.WriteWarning("  [INVALID] Name is too long (max 50 characters).");
                      continue;
                  }

                  bool valid = true;
                  foreach (char c in input)
                  {
                      if (!char.IsLetter(c) && c != ' ' && c != '-' && c != '\'')
                      {
                          valid = false;
                          break;
                      }
                  }

                  if (!valid)
                  {
                      ConsoleHelper.WriteWarning("  [INVALID] Name should only contain letters.");
                      continue;
                  }

                  string name = char.ToUpper(input[0]) + input.Substring(1).ToLower();
                  _user = new User(name);
                  break;
              }

              Console.WriteLine();
              ConsoleHelper.DrawDivider();
              ConsoleHelper.ShowThinking();

              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.Write("  [BOT] ");
              Console.ResetColor();
              ConsoleHelper.TypeText("Welcome, " + _user.Name + "! Session ID: " + _user.SessionId, ConsoleColor.Green);

              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.Write("  [BOT] ");
              Console.ResetColor();
              ConsoleHelper.TypeText("South Africa has seen a sharp rise in cyberattacks.", ConsoleColor.Green);

              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.Write("  [BOT] ");
              Console.ResetColor();
              ConsoleHelper.TypeText("Ask me anything or type 'help' to see available topics.", ConsoleColor.Green);
          }

          // ── Main chat loop ───────────────────────────────────────────

          private void RunChat()
          {
              string[] suggestions = new string[]
              {
                  "Tell me about passwords",
                  "What is phishing?",
                  "How do I stay safe online?"
              };

              while (true)
              {
                  ConsoleHelper.ShowSuggestions(suggestions);
                  ConsoleHelper.DrawDivider();

                  string input = ConsoleHelper.Prompt(_user.Name);

                  // Allow numbered shortcut for suggestions
                  if (input == "1") input = suggestions[0];
                  else if (input == "2") input = suggestions[1];
                  else if (input == "3") input = suggestions[2];

                  if (string.IsNullOrWhiteSpace(input))
                  {
                      ConsoleHelper.WriteWarning("  Please type something so I can help you.");
                      continue;
                  }

                  if (input.ToLower() == "exit" || input.ToLower() == "quit" || input.ToLower() == "bye")
                      break;

                  _user.MessageCount++;
                  ConsoleHelper.ShowThinking();
                  suggestions = Respond(input.ToLower());
              }
          }

          // ── Response engine ───────────────────────────────────────────

          private string[] Respond(string input)
          {
              ConsoleHelper.DrawDivider();

              // Follow-up request — continue last topic
              if (input.Contains("more") || input.Contains("explain") || input.Contains("another tip"))
              {
                  if (!string.IsNullOrEmpty(_lastTopic))
                  {
                      ConsoleHelper.PrintBotPrefix("INFO");
                      ConsoleHelper.TypeText("Here is more on " + _lastTopic + ":", ConsoleColor.Cyan);
                      return RespondToTopic(_lastTopic);
                  }
              }

              // How are you
              if (input.Contains("how are you"))
              {
                  _lastTopic = "";
                  ConsoleHelper.PrintBotPrefix("INFO");
                  ConsoleHelper.TypeText("I am running at full capacity - all systems green!", ConsoleColor.Cyan);
                  ConsoleHelper.TypeText("  Are YOU staying safe online, " + _user.Name + "?", ConsoleColor.Cyan);
                  return new string[] { "Tell me about passwords", "What is phishing?", "What topics can you cover?" };
              }

              // Help / purpose
              if (input.Contains("purpose") || input.Contains("help") || input.Contains("what can"))
              {
                  _lastTopic = "";
                  ConsoleHelper.PrintBotPrefix("INFO");
                  ConsoleHelper.TypeText("I can help with these cybersecurity topics:", ConsoleColor.Cyan);
                  Console.ForegroundColor = ConsoleColor.Gray;
                  Console.WriteLine("  * Passwords          * Phishing");
                  Console.WriteLine("  * Safe Browsing       * Malware");
                  Console.WriteLine("  * Social Engineering  * Two-Factor Auth (2FA)");
                  Console.WriteLine("  * Public Wi-Fi        * Privacy");
                  Console.WriteLine("  * Online Scams");
                  Console.ResetColor();
                  return new string[] { "Tell me about passwords", "What is malware?", "Tell me about phishing" };
              }

              // Route to cybersecurity topics
              if (input.Contains("password") || input.Contains("passphrase"))
              { _lastTopic = "password"; return RespondToTopic("password"); }

              if (input.Contains("phish") || input.Contains("fake email"))
              { _lastTopic = "phishing"; return RespondToTopic("phishing"); }

              if (input.Contains("brows") || input.Contains("https") || input.Contains("safe website"))
              { _lastTopic = "safe browsing"; return RespondToTopic("safe browsing"); }

              if (input.Contains("malware") || input.Contains("virus") || input.Contains("ransomware") || input.Contains("trojan"))
              { _lastTopic = "malware"; return RespondToTopic("malware"); }

              if (input.Contains("social engineer") || input.Contains("manipulat") || input.Contains("baiting"))
              { _lastTopic = "social engineering"; return RespondToTopic("social engineering"); }

              if (input.Contains("2fa") || input.Contains("two-factor") || input.Contains("two factor") || input.Contains("otp"))
              { _lastTopic = "2fa"; return RespondToTopic("2fa"); }

              if (input.Contains("wifi") || input.Contains("wi-fi") || input.Contains("hotspot"))
              { _lastTopic = "public wifi"; return RespondToTopic("public wifi"); }

              if (input.Contains("privacy") || input.Contains("personal data"))
              { _lastTopic = "privacy"; return RespondToTopic("privacy"); }

              if (input.Contains("scam") || input.Contains("fraud"))
              { _lastTopic = "scams"; return RespondToTopic("scams"); }

              // Fallback
              ConsoleHelper.PrintBotPrefix("INFO");
              ConsoleHelper.TypeText("I did not quite understand that - could you rephrase?", ConsoleColor.Cyan);
              ConsoleHelper.TypeText("  Type 'help' to see all topics I can assist with.", ConsoleColor.DarkGray);
              return new string[] { "What topics can you cover?", "Tell me about phishing", "Password safety tips" };
          }

          // ── Topic responses with severity colour-coding ───────────────

          private string[] RespondToTopic(string topic)
          {
              switch (topic)
              {
                  case "password":
                      ConsoleHelper.PrintBotPrefix("WARNING");
                      Console.ForegroundColor = ConsoleColor.Yellow;
                      Console.WriteLine("Strong passwords are your first line of defence!");
                      Console.WriteLine();
                      Console.WriteLine("  + Use at least 12 characters");
                      Console.WriteLine("  + Mix uppercase, lowercase, numbers and symbols");
                      Console.WriteLine("    e.g. P@ssw0rd!23  or  Coffee!Rain_Dog72");
                      Console.WriteLine("  + Use a different password for every account");
                      Console.WriteLine("  + Store passwords in a trusted password manager");
                      Console.WriteLine("  - NEVER use your name, birthday, or 'password123'");
                      Console.WriteLine("  - NEVER share your password with anyone");
                      Console.ResetColor();
                      return new string[] { "What is 2FA?", "Tell me about phishing", "Privacy tips" };

                  case "phishing":
                      ConsoleHelper.PrintBotPrefix("DANGER");
                      Console.ForegroundColor = ConsoleColor.Red;
                      Console.WriteLine("Phishing is one of the most common attacks in South Africa!");
                      Console.WriteLine();
                      Console.WriteLine("  Warning signs:");
                      Console.WriteLine("  ! Urgent subject lines: 'Your account will be closed!'");
                      Console.WriteLine("  ! Sender address does not match the real company");
                      Console.WriteLine("    e.g. support@amaz0n.fake.com");
                      Console.WriteLine("  ! Suspicious links - hover before you click");
                      Console.WriteLine("  ! Requests for passwords, bank details, or OTPs");
                      Console.WriteLine();
                      Console.WriteLine("  + Never click links in unexpected emails");
                      Console.WriteLine("  + Contact the company directly if unsure");
                      Console.WriteLine("  + Report phishing: www.saps.gov.za");
                      Console.ResetColor();
                      return new string[] { "What is malware?", "Safe browsing tips", "Social engineering" };

                  case "safe browsing":
                      ConsoleHelper.PrintBotPrefix("WARNING");
                      Console.ForegroundColor = ConsoleColor.Yellow;
                      Console.WriteLine("Safe browsing habits protect you every time you go online!");
                      Console.WriteLine();
                      Console.WriteLine("  + Look for the padlock icon and HTTPS in the URL");
                      Console.WriteLine("  + Keep your browser and OS updated at all times");
                      Console.WriteLine("  + Do not click pop-up ads or 'You have won!' banners");
                      Console.WriteLine("  + Check URLs carefully - goog1e.com is NOT Google!");
                      Console.WriteLine("  + Use reputable antivirus software");
                      Console.ResetColor();
                      return new string[] { "Public Wi-Fi risks", "What is malware?", "Tell me about phishing" };

                  case "malware":
                      ConsoleHelper.PrintBotPrefix("DANGER");
                      Console.ForegroundColor = ConsoleColor.Red;
                      Console.WriteLine("Malware is malicious software designed to cause damage.");
                      Console.WriteLine();
                      Console.WriteLine("  Types of malware:");
                      Console.WriteLine("  ~ Virus        - attaches to files and spreads");
                      Console.WriteLine("  ~ Ransomware   - encrypts files and demands payment");
                      Console.WriteLine("  ~ Spyware      - secretly monitors your activity");
                      Console.WriteLine("  ~ Trojan Horse - disguised as legitimate software");
                      Console.WriteLine();
                      Console.WriteLine("  + Install and update reputable antivirus software");
                      Console.WriteLine("  + Never download from untrusted sources");
                      Console.WriteLine("  + Back up your data regularly");
                      Console.WriteLine("  ! If infected - disconnect from the internet immediately!");
                      Console.ResetColor();
                      return new string[] { "Tell me about phishing", "Safe browsing tips", "Social engineering" };

                  case "social engineering":
                      ConsoleHelper.PrintBotPrefix("DANGER");
                      Console.ForegroundColor = ConsoleColor.Red;
                      Console.WriteLine("Social engineering attacks manipulate people, not systems.");
                      Console.WriteLine();
                      Console.WriteLine("  Common tactics:");
                      Console.WriteLine("  ~ Pretexting    - fake scenarios to gain your trust");
                      Console.WriteLine("  ~ Vishing       - phone calls pretending to be your bank");
                      Console.WriteLine("  ~ Baiting       - infected USB drives left in public places");
                      Console.WriteLine("  ~ Impersonation - pretending to be a colleague or IT");
                      Console.WriteLine();
                      Console.WriteLine("  + Your bank will NEVER ask for your PIN over the phone");
                      Console.WriteLine("  + If in doubt, hang up and call the official number");
                      Console.WriteLine("  + Do not plug in unknown USB devices");
                      Console.ResetColor();
                      return new string[] { "Tell me about phishing", "Password safety", "What is 2FA?" };

                  case "2fa":
                      ConsoleHelper.PrintBotPrefix("SAFE");
                      Console.ForegroundColor = ConsoleColor.Green;
                      Console.WriteLine("Two-Factor Authentication (2FA) adds a second layer of security!");
                      Console.WriteLine();
                      Console.WriteLine("  Even if someone steals your password, they still need:");
                      Console.WriteLine("  1. Something you KNOW  (your password)");
                      Console.WriteLine("  2. Something you HAVE  (phone or authenticator app)");
                      Console.WriteLine();
                      Console.WriteLine("  Best options (most to least secure):");
                      Console.WriteLine("  + Authenticator app  e.g. Google or Microsoft Auth");
                      Console.WriteLine("  + SMS OTP codes      - convenient but riskier");
                      Console.WriteLine();
                      Console.WriteLine("  Enable 2FA on: banking, Gmail, and all key accounts!");
                      Console.WriteLine("  NEVER share your OTP with anyone.");
                      Console.ResetColor();
                      return new string[] { "Password safety", "Tell me about phishing", "Privacy tips" };

                  case "public wifi":
                      ConsoleHelper.PrintBotPrefix("WARNING");
                      Console.ForegroundColor = ConsoleColor.Yellow;
                      Console.WriteLine("Public Wi-Fi at coffee shops and malls is risky!");
                      Console.WriteLine();
                      Console.WriteLine("  Risks:");
                      Console.WriteLine("  ! Man-in-the-Middle - hackers intercept your data");
                      Console.WriteLine("  ! Evil twin attacks - fake hotspots with real-sounding names");
                      Console.WriteLine("  ! Packet sniffing   - capturing unencrypted data");
                      Console.WriteLine();
                      Console.WriteLine("  + Use a VPN to encrypt your internet traffic");
                      Console.WriteLine("  + Avoid banking on public Wi-Fi - use mobile data");
                      Console.WriteLine("  + Only visit HTTPS websites on public networks");
                      Console.WriteLine("  + Turn off auto-connect to Wi-Fi on your phone");
                      Console.ResetColor();
                      return new string[] { "Safe browsing tips", "What is malware?", "Privacy tips" };

                  case "privacy":
                      ConsoleHelper.PrintBotPrefix("WARNING");
                      Console.ForegroundColor = ConsoleColor.Yellow;
                      Console.WriteLine("Protecting your privacy online is essential!");
                      Console.WriteLine();
                      Console.WriteLine("  + Review your social media privacy settings regularly");
                      Console.WriteLine("  + Avoid sharing your ID number or address publicly");
                      Console.WriteLine("  + Use a VPN when browsing on public Wi-Fi");
                      Console.WriteLine("  + Read privacy policies before using new apps");
                      Console.WriteLine("  + Enable 2FA on all accounts with personal data");
                      Console.ResetColor();
                      return new string[] { "What is 2FA?", "Public Wi-Fi risks", "Password safety" };

                  case "scams":
                      ConsoleHelper.PrintBotPrefix("DANGER");
                      Console.ForegroundColor = ConsoleColor.Red;
                      Console.WriteLine("Online scams are rising rapidly in South Africa!");
                      Console.WriteLine();
                      Console.WriteLine("  Common scams in SA:");
                      Console.WriteLine("  ~ Lottery scams    - 'You have won R1,000,000!'");
                      Console.WriteLine("  ~ Job offer scams  - 'Earn R500/hour from home!'");
                      Console.WriteLine("  ~ Romance scams    - fake relationships for money");
                      Console.WriteLine("  ~ Banking scams    - fake bank SMS or call");
                      Console.WriteLine();
                      Console.WriteLine("  + If it sounds too good to be true, it is a scam");
                      Console.WriteLine("  + Never send money to someone you met only online");
                      Console.WriteLine("  + Report fraud to SABRIC: www.sabric.co.za");
                      Console.ResetColor();
                      return new string[] { "Tell me about phishing", "Social engineering", "Privacy tips" };

                  default:
                      ConsoleHelper.PrintBotPrefix("INFO");
                      ConsoleHelper.TypeText("I did not understand that topic. Type 'help' to see all topics.", ConsoleColor.Cyan);
                      return new string[] { "What topics can you cover?", "Tell me about phishing", "Password safety" };
              }
          }

          // ── Goodbye screen with session stats ─────────────────────────

          private void DisplayGoodbye()
          {
              Console.WriteLine();
              ConsoleHelper.DrawBorder();

              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.Write("  [BOT] ");
              Console.ResetColor();
              ConsoleHelper.TypeText("Thank you for chatting, " + _user.Name + "!", ConsoleColor.Green);

              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.Write("  [BOT] ");
              Console.ResetColor();
              Console.ForegroundColor = ConsoleColor.DarkCyan;
              Console.WriteLine("Session: " + _user.MessageCount + " message(s)  |  Duration: " + _user.GetDuration());
              Console.ResetColor();

              Console.ForegroundColor = ConsoleColor.DarkGreen;
              Console.Write("  [BOT] ");
              Console.ResetColor();
              ConsoleHelper.TypeText("Stay alert, stay safe. Cybersecurity is everyone's responsibility.", ConsoleColor.Green);

              ConsoleHelper.DrawDivider();
              ConsoleHelper.WriteMuted("  Report cybercrime : www.saps.gov.za  |  Emergency: 10111");
              ConsoleHelper.WriteMuted("  Banking fraud (SABRIC): www.sabric.co.za");
              ConsoleHelper.DrawBorder();
              Console.WriteLine();
          }
      }
  }
  