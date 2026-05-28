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

  ## What Is This?

  The **Cybersecurity Awareness Bot** is a C# application that educates South African citizens about online threats in an interactive and engaging way. It ships in two parts:

  | | Part | Description |
  |---|---|---|
  | 🖥️ | **Part 1 — Console App** | Keyword recognition, colour-coded severity, voice greeting, session stats |
  | 🪟 | **Part 2 — WPF GUI** | Full Windows desktop app with sentiment detection, memory recall, and delegate usage |

  ---

  ## CI Build — Both Parts Passing

  Both projects build automatically on every push to `master` via GitHub Actions.

  ![CI Build Passing](assets/ci-screenshot.png)

  ---

  ## Part 1 — Console Application

  <details>
  <summary><strong>Click to expand — Project structure and features</strong></summary>

  ### File Structure

  ```
  ├── .github/workflows/dotnet.yml  ← CI: builds Part 1 AND Part 2 on every push
  ├── Program.cs                    ← Entry point
  ├── Chatbot.cs                    ← Main orchestrator + 9-topic response engine
  ├── ConsoleHelper.cs              ← UI utilities (colours, typing, borders)
  ├── AudioPlayer.cs                ← WAV greeting with graceful text fallback
  ├── User.cs                       ← Name, SessionId, timestamps, message count
  ├── CyberBot.csproj               ← .NET 8 Windows console project
  └── greeting.wav                  ← Voice greeting audio
  ```

  ### Features

  | Feature | Description |
  |---|---|
  | 🎨 **ASCII Art Logo** | CyberSec banner displayed on every launch |
  | 🔊 **Voice Greeting** | WAV audio on startup, graceful text fallback on Linux |
  | 👤 **Name Validation** | Checks empty, too short, too long, invalid characters |
  | 🛡️ **9 Cybersecurity Topics** | DANGER / WARNING / SAFE / INFO colour-coded severity |
  | ⌨️ **Typing Effect** | Character-by-character animation for realism |
  | 🔢 **Numbered Suggestions** | Quick-reply shortcuts after every response |
  | 🔄 **Follow-ups** | "Tell me more" continues the last topic |
  | 📊 **Session Stats** | Message count and duration shown on exit |

  ### Run Part 1

  ```bash
  git clone https://github.com/Sekgatla/Cyber-Security-Awareness-Bot.git
  cd Cyber-Security-Awareness-Bot
  dotnet run
  ```

  </details>

  ---

  ## Part 2 — WPF GUI Application

  <details>
  <summary><strong>Click to expand — Project structure and features</strong></summary>

  ### File Structure

  ```
  Part2/
  ├── App.xaml / App.xaml.cs         ← WPF application entry point
  ├── MainWindow.xaml                 ← Dark-themed GUI layout (navy, cyan, white)
  ├── MainWindow.xaml.cs              ← Thin event handler — only calls ChatBot
  ├── ChatBot.cs                      ← Central router: name → follow-up → sentiment → keyword
  ├── KeywordResponder.cs             ← Dictionary<string, List<string>> — 8 topics, random picks
  ├── SentimentDetector.cs            ← Enum + Dictionary — detects Worried/Curious/Frustrated/Happy
  ├── MemoryStore.cs                  ← Stores UserName, FavouriteTopic; personalises responses
  ├── CybersecurityChatbot.csproj     ← .NET 8 Windows WPF project
  └── greeting.wav                    ← Voice greeting (copied to output on build)
  ```

  ### Assessment Features

  | Feature | Marks | How It Is Implemented |
  |---|---|---|
  | 🖥️ **GUI Design** | 10 | Dark WPF layout — navy/cyan/white, rounded bubble chat, ASCII art header |
  | 🔑 **Keyword Recognition** | 15 | `KeywordResponder` — 8 keywords, `Dictionary<string, List<string>>` |
  | 🎲 **Random Responses** | 10 | `_random.Next()` picks one response from each keyword's list |
  | 🔄 **Conversation Flow** | 10 | "Tell me more" reuses `_lastTopic` without resetting the chat |
  | 🧠 **Memory and Recall** | 10 | `MemoryStore` saves name + favourite topic and personalises replies |
  | 😊 **Sentiment Detection** | 10 | 5-state enum; auto-prepends an empathetic opener before each tip |
  | 🏗️ **Code Optimisation** | 10 | 4 focused classes, no God class, delegate for response formatting |
  | 📦 **GitHub and Releases** | 10 | 6+ commits, 2 tagged releases (v1.0, v2.0) |

  ### Delegate Usage

  The assignment requires delegates. `ResponseTransformer` is declared in `ChatBot.cs`:

  ```csharp
  // Delegate type — transforms a raw response before displaying it
  public delegate string ResponseTransformer(string rawResponse);

  // Used in ProcessInput() to apply sentiment opener + personalised greeting:
  ResponseTransformer applyContext = delegate(string raw)
  {
      return sentimentOpener + _memory.GetPersonalisedOpener() + raw;
  };
  return applyContext(keywordResponse);
  ```

  ### Generic Collection Usage

  `KeywordResponder` uses `Dictionary<string, List<string>>` — the key is a cybersecurity keyword and the value is a list of multiple randomised responses, satisfying both the generic collection and random responses requirements.

  ### Run Part 2

  Open `Part2/CybersecurityChatbot.csproj` in **Visual Studio 2022** then press **F5**.

  > **Requirements:** .NET 8 SDK + Windows OS (WPF and SoundPlayer are Windows-only)

  </details>

  ---

  ## Cybersecurity Topics Covered

  | # | Topic | Severity | Keywords |
  |---|---|---|---|
  | 1 | 🔐 Password Safety | 🟡 WARNING | `password` |
  | 2 | 🎣 Phishing | 🔴 DANGER | `phishing` |
  | 3 | 💸 Online Scams | 🔴 DANGER | `scam` |
  | 4 | 🕵️ Privacy | 🟡 WARNING | `privacy` |
  | 5 | 🦠 Malware | 🔴 DANGER | `malware` |
  | 6 | 🔒 Two-Factor Auth | 🟢 SAFE | `2fa` |
  | 7 | 📶 Public Wi-Fi | 🟡 WARNING | `wifi` |
  | 8 | 🎭 Social Engineering | 🔴 DANGER | `social engineering` |
  | 9 | 🌐 Safe Browsing | 🟡 WARNING | `browse` |

  ---

  ## Releases

  | Release | What Is Included |
  |---|---|
  | [v1.0 — Part 1 Console App](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v1.0) | Fully working console chatbot |
  | [v2.0 — Part 2 WPF GUI](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v2.0) | Full WPF desktop application |

  ---

  ## Video Presentation

  Coming soon.

  ---

  ## References

  - Pieterse, H. 2021. *The Cyber Threat Landscape in South Africa: A 10-Year Review.* African Journal of Information and Communication, 28(28). https://doi.org/10.23962/10539/32213
  - South African Police Service Cybercrime: https://www.saps.gov.za
  - South African Banking Risk Information Centre: https://www.sabric.co.za

  ---

  <div align="center">

  **🛡️ Stay Safe Online 🛡️**

  *PROG6221 Programming 2A &nbsp;·&nbsp; IIE University &nbsp;·&nbsp; Sekgatla*

  </div>
  