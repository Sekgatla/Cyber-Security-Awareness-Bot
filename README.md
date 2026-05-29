<div align="center">

  <img src="https://readme-typing-svg.demolab.com?font=Fira+Code&size=22&pause=1000&color=00D9FF&center=true&vCenter=true&width=600&lines=Cybersecurity+Awareness+Bot;Protecting+South+Africans+Online;PROG6221+%7C+IIE+University" alt="Typing SVG" />

  ```
    ______      _               ____
   / ___|   _| |__   ___ _ __/ ___|  ___  ___
  | |  | | | | '_ \ / _ \ '__\___ \ / _ \/ __|
  | |__| |_| | |_) |  __/ |   ___) |  __/ (__
   \____\__, |_.__/ \___|_|  |____/ \___|\___| v2.0
        |___/    Cybersecurity Awareness Bot
  ```

  [![Build](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions)
  [![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
  [![C#](https://img.shields.io/badge/C%23-WPF%20%7C%20Console-239120?style=flat-square&logo=csharp&logoColor=white)](https://docs.microsoft.com/dotnet/csharp/)
  [![Windows](https://img.shields.io/badge/Windows-Required-0078D6?style=flat-square&logo=windows&logoColor=white)](https://microsoft.com/windows)
  [![License](https://img.shields.io/badge/License-Academic-orange?style=flat-square)](LICENSE)

  > *"In a world where cyber threats grow every day — knowledge is your best defence."*

  **Created by Sekgatla &nbsp;·&nbsp; IIE University &nbsp;·&nbsp; PROG6221 Programming 2A**

  </div>

  ---

  ## Table of Contents

  - [Overview](#overview)
  - [CI Build — Both Parts Passing](#ci-build--both-parts-passing)
  - [Part 1 — Console Application](#part-1--console-application)
    - [Project Structure](#project-structure)
    - [Part 1 Features](#part-1-features)
    - [How to Run Part 1](#how-to-run-part-1)
  - [Part 2 — WPF GUI Application](#part-2--wpf-gui-application)
    - [Part 2 Project Structure](#part-2-project-structure)
    - [Part 2 Assessment Features](#part-2-assessment-features)
    - [Delegate Usage](#delegate-usage)
    - [Generic Collection Usage](#generic-collection-usage)
    - [Conversation Flow — How ProcessInput Works](#conversation-flow--how-processinput-works)
    - [How to Run Part 2](#how-to-run-part-2)
  - [Cybersecurity Topics Covered](#cybersecurity-topics-covered)
  - [GitHub Releases](#github-releases)
  - [Video Presentation](#video-presentation)
  - [References](#references)

  ---

  ## Overview

  The **Cybersecurity Awareness Bot** is a C# application designed to educate South African citizens about online threats in an interactive and engaging way. South Africa has one of the highest rates of cybercrime in Africa, and this project aims to raise awareness through an accessible chatbot experience.

  The project is submitted for PROG6221 Programming 2A at IIE University and is split into two parts:

  | | Part | Type | Description |
  |---|---|---|---|
  | 🖥️ | **Part 1** | C# Console Application | Keyword recognition, colour-coded severity levels, voice greeting, typing animation, session statistics, and 9 cybersecurity topics |
  | 🪟 | **Part 2** | C# WPF GUI Application | Full Windows Presentation Foundation desktop interface with sentiment detection, conversation memory, random responses, follow-up handling, and delegate usage |

  ---

  ## CI Build — Both Parts Passing

  Both projects — Part 1 (console) and Part 2 (WPF) — are built automatically on every push to the `master` branch using **GitHub Actions**. This ensures the code compiles correctly at all times and demonstrates professional software development practice.

  The workflow file is located at `.github/workflows/dotnet.yml` and runs two separate jobs:
  - **Build Part 1 - Console App** — builds `CyberBot.csproj` targeting `.NET 8 Windows`
  - **Build Part 2 - WPF GUI** — builds `Part2/CybersecurityChatbot.csproj` targeting `.NET 8 Windows`

  ![CI Build Passing](assets/ci-screenshot.png)

  ---

  ## Part 1 — Console Application

  Part 1 is a fully featured C# console application that runs in the Windows terminal. It was built using object-oriented programming principles across multiple class files to avoid a God class. The bot greets the user by name, detects cybersecurity keywords in their messages, and responds with colour-coded, severity-rated information.

  ### Project Structure

  ```
  ├── .github/
  │   └── workflows/
  │       └── dotnet.yml        ← GitHub Actions CI — builds both Part 1 and Part 2
  ├── Program.cs                ← Application entry point — starts the chatbot session
  ├── Chatbot.cs                ← Main orchestrator — routes input, manages 9 topics and responses
  ├── ConsoleHelper.cs          ← UI utilities — colour output, typing animation, borders, dividers
  ├── AudioPlayer.cs            ← WAV voice greeting using SoundPlayer, text fallback on Linux
  ├── User.cs                   ← Auto properties: Name, SessionId, SessionStart, MessageCount
  ├── CyberBot.csproj           ← .NET 8 Windows console project file
  └── greeting.wav              ← Recorded voice greeting played on startup
  ```

  ### Part 1 Features

  | Feature | Description |
  |---|---|
  | 🎨 **ASCII Art Logo** | A large CyberSec ASCII banner is displayed every time the application launches, giving it a professional cybersecurity tool appearance |
  | 🔊 **Voice Greeting** | The app plays a pre-recorded `greeting.wav` file using `System.Media.SoundPlayer` on startup. If the WAV file is unavailable (e.g., on Linux), a text greeting is displayed instead so the app never crashes |
  | 👤 **Name Input and Validation** | The user is prompted to enter their name before the chat begins. The input is validated — it must not be empty, must be at least 2 characters, must not exceed 30 characters, and must only contain letters and spaces. Invalid input shows a clear error message and asks again |
  | 🛡️ **9 Cybersecurity Topics** | The bot covers passwords, phishing, safe browsing, malware, social engineering, two-factor authentication, public Wi-Fi, privacy, and online scams. Each topic is colour-coded by severity: RED for DANGER, YELLOW for WARNING, GREEN for SAFE, and CYAN for INFO |
  | ⌨️ **Typing Animation Effect** | Bot responses are printed character by character with a small delay, simulating a real typing effect. This improves the user experience and makes the interaction feel more natural and engaging |
  | 🔢 **Numbered Quick-Reply Suggestions** | After every response, the bot displays numbered shortcuts (e.g., `1. Phishing  2. Malware  3. Passwords`) so the user can type a number instead of a full question to quickly explore topics |
  | 🔄 **Follow-up Handling** | If the user types phrases like "tell me more" or "explain more", the bot continues on the last topic discussed without requiring the user to repeat themselves. This demonstrates conversation flow and memory |
  | 📊 **Session Statistics on Exit** | When the user types "exit" or "quit", the app displays a summary of the session including the user's name, total messages sent, and how long the session lasted |

  ### How to Run Part 1

  ```bash
  git clone https://github.com/Sekgatla/Cyber-Security-Awareness-Bot.git
  cd Cyber-Security-Awareness-Bot
  dotnet run
  ```

  > **Requirements:** .NET 8 SDK and Windows OS (for `SoundPlayer` voice greeting)

  ---

  ## Part 2 — WPF GUI Application

  Part 2 extends the console chatbot into a full **Windows Presentation Foundation** desktop application with a modern dark-themed graphical interface. All Part 1 features are carried forward — voice greeting, ASCII art, personalised responses — and four new features are added: keyword recognition with random responses, sentiment detection, conversation memory and recall, and a delegate for response transformation.

  The GUI uses a navy, cyan, and white colour scheme to give it the feel of a real cybersecurity tool. Messages appear as styled chat bubbles, similar to a modern messaging app.

  ### Part 2 Project Structure

  ```
  Part2/
  ├── App.xaml                        ← WPF application definition
  ├── App.xaml.cs                     ← Application entry point and startup logic
  ├── MainWindow.xaml                 ← Full dark-themed GUI layout with chat bubbles,
  │                                      header, input box, and send button
  ├── MainWindow.xaml.cs              ← Thin code-behind — only handles UI events and
  │                                      calls ChatBot. No logic lives here
  ├── ChatBot.cs                      ← Central routing class — processes every user
  │                                      message through all features in the correct order
  ├── KeywordResponder.cs             ← Holds a Dictionary<string, List<string>> of
  │                                      8 keywords each with 5 randomised responses
  ├── SentimentDetector.cs            ← Detects emotional tone using an enum and a
  │                                      Dictionary of trigger words per sentiment
  ├── MemoryStore.cs                  ← Stores user name and favourite topic using auto
  │                                      properties and a private Dictionary
  ├── CybersecurityChatbot.csproj     ← .NET 8 Windows WPF project file
  └── greeting.wav                    ← Voice greeting WAV — copied to output on build
  ```

  ### Part 2 Assessment Features

  | Feature | Marks | Detailed Description |
  |---|---|---|
  | 🖥️ **GUI Design** | 10 | A fully designed WPF window with a dark background (`#0D1117`), a scrollable chat area that auto-scrolls to the latest message, a TextBox input with a cyan caret, and a rounded Send button. The ASCII art logo is displayed in the header on every launch. The voice greeting plays automatically when the window opens |
  | 🔑 **Keyword Recognition** | 15 | `KeywordResponder.cs` uses a `Dictionary<string, List<string>>` where each key is a cybersecurity keyword (password, phishing, scam, privacy, malware, 2fa, wifi, social engineering) and the value is a list of 5 different responses. The `GetResponse()` method loops through all keys and checks whether the user's input contains that keyword |
  | 🎲 **Random Responses** | 10 | When a keyword is matched, `_random.Next(0, list.Count)` selects a random response from that keyword's list. This means the same question will give a different answer each time, keeping the conversation fresh and educational |
  | 🔄 **Conversation Flow** | 10 | The bot tracks the last topic discussed in a private field called `_lastTopic`. If the user types a follow-up phrase such as "tell me more", "explain more", "another tip", or "go on", the bot fetches another response on the same topic without resetting the conversation or requiring the user to repeat the keyword |
  | 🧠 **Memory and Recall** | 10 | `MemoryStore.cs` stores the user's name and favourite topic using auto properties (`UserName`, `FavouriteTopic`). It also has a private `Dictionary<string, string>` for general key-value storage. If the user says "I am interested in privacy", the bot stores "privacy" as the favourite topic and prepends "As someone interested in privacy..." to later responses |
  | 😊 **Sentiment Detection** | 10 | `SentimentDetector.cs` defines a `Sentiment` enum with five values: Neutral, Worried, Curious, Frustrated, and Happy. A `Dictionary<Sentiment, List<string>>` maps each sentiment to its trigger words. When a user's message contains a trigger word, the bot prepends an empathetic opening sentence before the cybersecurity tip — without requiring the user to ask twice |
  | 🏗️ **Code Optimisation** | 10 | Logic is split across four dedicated classes (ChatBot, KeywordResponder, SentimentDetector, MemoryStore). `MainWindow.xaml.cs` only handles UI events. No God class. A `ResponseTransformer` delegate is declared and used to apply sentiment openers and personalised greetings to responses in a single step |
  | 📦 **GitHub and Releases** | 10 | Over 6 meaningful commits with descriptive messages, 2 tagged releases (`v1.0` for Part 1, `v2.0` for Part 2), and a complete README with CI badge and screenshot |

  ### Delegate Usage

  The assignment explicitly requires the use of delegates. A custom delegate type called `ResponseTransformer` is declared at the top of `ChatBot.cs` and used inside `ProcessInput()`:

  ```csharp
  // Delegate declaration — sits outside the class, inside the namespace
  // It represents any method that takes a raw string response and returns
  // a transformed string with context prepended to it
  public delegate string ResponseTransformer(string rawResponse);

  // Inside ProcessInput() — used when a keyword match is found:
  ResponseTransformer applyContext = delegate(string raw)
  {
      // Prepend the sentiment opener (e.g. "I understand this feels overwhelming...")
      // then the personalised greeting (e.g. "As someone interested in privacy, ...")
      // then the actual cybersecurity tip
      return sentimentOpener + _memory.GetPersonalisedOpener() + raw;
  };

  return applyContext(keywordResponse);
  ```

  This satisfies the delegate requirement because a named delegate type is declared, an anonymous method is assigned to it, and the delegate is then invoked to produce the final response.

  ### Generic Collection Usage

  `KeywordResponder.cs` uses `Dictionary<string, List<string>>` as its core data structure. This is a generic collection — the outer `Dictionary` maps each keyword string to a `List<string>` of multiple possible responses. This satisfies the generic collection requirement while also making keyword lookup and random response selection clean and efficient:

  ```csharp
  private Dictionary<string, List<string>> _responses;

  // Example entry:
  _responses["phishing"] = new List<string>
  {
      "Be cautious of emails asking for personal information...",
      "Always hover over links before clicking...",
      "Check the sender's email address carefully...",
      // ... 2 more responses
  };
  ```

  ### Conversation Flow — How ProcessInput Works

  `ChatBot.ProcessInput()` is the most important method in the project. Every user message flows through it in a strict order:

  | Step | What Is Checked | What Happens |
  |---|---|---|
  | 1 | Is `_awaitingName` true? | Capture the name, store it in `MemoryStore`, set `_awaitingName` to false, return a personalised welcome message |
  | 2 | Does input contain "interested in" / "I like"? | Store the keyword as `FavouriteTopic` in memory for later personalisation |
  | 3 | Is input a follow-up phrase? | Return another response on `_lastTopic` using the `ResponseTransformer` delegate |
  | 4 | Run `SentimentDetector.Detect()` | Get the empathetic opener string (empty string if Neutral) |
  | 5 | Run `KeywordResponder.GetResponse()` | If a keyword matches, update `_lastTopic`, apply delegate, return the response |
  | 6 | Check special phrases | Handle "how are you", "what can you do", "purpose", "help" |
  | 7 | Fall through | Return a random fallback response from a predefined list |

  ### How to Run Part 2

  1. Open **Visual Studio 2022**
  2. Click **File → Open → Project/Solution**
  3. Navigate to the `Part2` folder and open `CybersecurityChatbot.csproj`
  4. Press **F5** to build and run

  The voice greeting will play automatically when the window opens, and the ASCII art header will be displayed at the top of the chat interface.

  > **Requirements:** .NET 8 SDK + Windows OS (WPF and SoundPlayer are Windows-only technologies)

  ---

  ## Cybersecurity Topics Covered

  Both Part 1 and Part 2 cover the same nine cybersecurity topics. Each topic has five different responses that are selected randomly, so the user receives a different tip each time they ask about the same topic.

  | # | Topic | Severity | Keyword | What the Bot Covers |
  |---|---|---|---|---|
  | 1 | 🔐 Password Safety | 🟡 WARNING | `password` | Strong password rules, passphrases, password managers, avoiding reuse, changing compromised passwords |
  | 2 | 🎣 Phishing | 🔴 DANGER | `phishing` | Identifying fake emails, hovering over links, checking sender addresses, urgency tactics, going directly to websites |
  | 3 | 💸 Online Scams | 🔴 DANGER | `scam` | Too-good-to-be-true offers, never sending money to strangers, SABRIC reporting, lottery/romance/job scams, OTP fraud |
  | 4 | 🕵️ Privacy | 🟡 WARNING | `privacy` | Social media privacy settings, avoiding sharing ID numbers publicly, using a VPN, reading privacy policies, enabling 2FA |
  | 5 | 🦠 Malware | 🔴 DANGER | `malware` | Antivirus software, avoiding untrusted downloads, ransomware and backups, email attachment dangers, keeping software updated |
  | 6 | 🔒 Two-Factor Auth | 🟢 SAFE | `2fa` | What 2FA adds, authenticator apps vs SMS, SIM-swap risks, which accounts need 2FA, never sharing OTPs |
  | 7 | 📶 Public Wi-Fi | 🟡 WARNING | `wifi` | Avoiding banking on public Wi-Fi, using a VPN, fake hotspot dangers, disabling auto-connect, HTTPS requirement |
  | 8 | 🎭 Social Engineering | 🔴 DANGER | `social engineering` | Manipulation vs technical attacks, banks never asking for PINs, urgency tactics, infected USB drives, verifying identities |
  | 9 | 🌐 Safe Browsing | 🟡 WARNING | `browse` | Checking HTTPS and padlock icons, avoiding suspicious links, keeping browsers updated, clearing cookies, safe search habits |

  ---

  ## GitHub Releases

  Two tagged releases mark the completion of each submission milestone:

  | Release | Tag | What Is Included |
  |---|---|---|
  | [Part 1 — Console Application](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v1.0) | `v1.0` | Fully working C# console chatbot with 9 topics, voice greeting, typing animation, colour-coded severity, session stats, and GitHub Actions CI |
  | [Part 2 — WPF GUI Application](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v2.0) | `v2.0` | Full WPF desktop application with sentiment detection, memory recall, random responses, conversation flow, delegate usage, and decorated README |

  ---

  ## Video Presentation
  
part 1 - https://youtu.be/esOEIdVb6EA?si=0P01PccYEdY_jaEJ 
part 2 - https://youtu.be/vW_tzyP-inA?si=MMGAnaHtLS-43mrW

  

  ---

  ## References

  - Pieterse, H. 2021. *The Cyber Threat Landscape in South Africa: A 10-Year Review.* African Journal of Information and Communication, 28(28). https://doi.org/10.23962/10539/32213
  - South African Police Service — Cybercrime Division: https://www.saps.gov.za
  - South African Banking Risk Information Centre (SABRIC): https://www.sabric.co.za
  - Microsoft .NET 8 Documentation: https://learn.microsoft.com/dotnet/
  - Windows Presentation Foundation (WPF) Guide: https://learn.microsoft.com/dotnet/desktop/wpf/

  ---

  <div align="center">

  **🛡️ Stay Safe Online — Knowledge Is Your Best Defence 🛡️**

  *PROG6221 Programming 2A &nbsp;·&nbsp; IIE University &nbsp;·&nbsp; Sekgatla*

  </div>
  
