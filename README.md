<div align="center">

  ```
     ____      _               ____             
    / ___|   _| |__   ___ _ __/ ___|  ___  ___ 
   | |  | | | | '_ \/ _ \ '__\___ \ / _ \/ __|
   | |__| |_| | |_) |  __/ |   ___) |  __/ (__ 
    \____\__, |_.__/ \___|_|  |____/ \___|\___|
         |___/                                  
  ```

  # 🛡️ Cybersecurity Awareness Bot

  **Created by Sekgatla** &nbsp;·&nbsp; IIE University &nbsp;·&nbsp; PROG6221 Programming 2A

  [![Build](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions)
  [![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
  [![C#](https://img.shields.io/badge/C%23-Console-239120?style=flat-square&logo=csharp)](https://docs.microsoft.com/dotnet/csharp/)
  [![Windows](https://img.shields.io/badge/Windows-Required-0078D6?style=flat-square&logo=windows)](https://microsoft.com/windows)

  > *"In a world where cyber threats grow every day — knowledge is your best defence."*

  </div>

  ---

  ## 📖 Overview

  The **Cybersecurity Awareness Bot** is a C# .NET 8 console application that educates South African citizens about common online threats. It responds to natural language queries, uses colour-coded severity responses, and provides a typing-effect interface for an engaging experience.

  ---

  ## ✨ Features

  | Feature | Description |
  |---------|-------------|
  | 🎨 **ASCII Art Logo** | CyberSec logo displayed on every launch |
  | 🔊 **Voice Greeting** | WAV audio via `System.Media.SoundPlayer` with text fallback |
  | 👤 **Name Validation** | Checks empty · too short · too long · invalid characters |
  | 🛡️ **9 Cybersecurity Topics** | Full responses with severity colour-coding |
  | 🎨 **Colour-Coded Responses** | 🔴 DANGER &nbsp;·&nbsp; 🟡 WARNING &nbsp;·&nbsp; 🟢 SAFE &nbsp;·&nbsp; 🔵 INFO |
  | ⌨️ **Typing Effect** | Character-by-character animation on responses |
  | 🔢 **Quick-Reply Shortcuts** | Numbered suggestions after every response |
  | 🔄 **Follow-up Handling** | "Tell me more" continues the last topic without resetting |
  | 📊 **Session Stats** | Message count and session duration shown on exit |
  | 🏗️ **OOP Architecture** | One class per responsibility — no God class |
  | ✅ **CI/CD Pipeline** | GitHub Actions builds on every push |

  ---

  ## 🗂️ Project Structure

  ```
  Cyber-Security-Awareness-Bot/
  │
  ├── .github/
  │   └── workflows/
  │       └── dotnet.yml       ← CI — builds automatically on every push
  │
  ├── Program.cs               ← Entry point — wires audio and chatbot
  ├── Chatbot.cs               ← Main orchestrator and response engine
  ├── ConsoleHelper.cs         ← All UI utilities (colours, typing, borders, prompts)
  ├── AudioPlayer.cs           ← WAV greeting with graceful text fallback
  ├── User.cs                  ← Name, SessionId, timestamps, message count
  ├── CyberBot.csproj          ← .NET 8 Windows project file
  └── greeting.wav             ← Voice greeting audio asset
  ```

  ---

  ## 🛡️ Cybersecurity Topics Covered

  | # | Topic | Severity | What it covers |
  |---|-------|----------|----------------|
  | 1 | 🔐 Password Safety | 🟡 WARNING | Strong passwords, passphrases, password managers |
  | 2 | 🎣 Phishing | 🔴 DANGER | Spotting fake emails and suspicious links |
  | 3 | 🌐 Safe Browsing | 🟡 WARNING | HTTPS, browser safety, avoiding pop-ups |
  | 4 | 🦠 Malware | 🔴 DANGER | Virus, ransomware, spyware, trojans |
  | 5 | 🎭 Social Engineering | 🔴 DANGER | Pretexting, vishing, baiting, impersonation |
  | 6 | 🔒 Two-Factor Auth (2FA) | 🟢 SAFE | Authenticator apps, OTP safety |
  | 7 | 📶 Public Wi-Fi | 🟡 WARNING | MITM attacks, VPN, evil twin hotspots |
  | 8 | 🕵️ Privacy | 🟡 WARNING | Social media settings, data protection |
  | 9 | 💸 Online Scams | 🔴 DANGER | Lottery, romance, banking fraud |

  ---

  ## 🚀 Getting Started

  ### Prerequisites
  - [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
  - Windows OS (required for `System.Media.SoundPlayer`)
  - Visual Studio 2022 or any IDE

  ### Run the bot

  ```bash
  git clone https://github.com/Sekgatla/Cyber-Security-Awareness-Bot.git
  cd Cyber-Security-Awareness-Bot
  dotnet run
  ```

  > Place `greeting.wav` in the project root for voice greeting.  
  > If missing, a typed text greeting is shown automatically.

  ---

  ## 🏗️ Code Architecture

  ### Class Responsibilities

  | Class | Responsibility |
  |-------|----------------|
  | `Program` | Entry point — creates AudioPlayer and Chatbot |
  | `Chatbot` | Orchestrates session: header → name → chat loop → goodbye |
  | `ConsoleHelper` | All display helpers: colours, typing effect, borders, prompts |
  | `AudioPlayer` | Plays WAV greeting; graceful typed fallback if file missing |
  | `User` | Stores Name, SessionId, SessionStart, MessageCount, GetDuration() |

  ### Call Flow

  ```
  Program.Main()
     ├── AudioPlayer.PlayGreeting()      ← WAV or text fallback
     └── Chatbot.Start()
           ├── DisplayHeader()           ← ASCII art via ConsoleHelper
           ├── GetUserName()             ← Input with 4-level validation
           ├── RunChat()                 ← Loop: suggestions → input → respond
           │     └── Respond()          ← Routes to topic, handles follow-ups
           │           └── RespondToTopic()  ← Colour-coded responses by severity
           └── DisplayGoodbye()         ← Session stats + emergency contacts
  ```

  ---

  ## ⚙️ CI/CD — GitHub Actions

  Every push automatically triggers:

  ```yaml
  runs-on: windows-latest
  steps: checkout → setup .NET 8 → restore → build Release
  ```

  [![Build](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions)

  ---

  ## 📺 Video Presentation

  🎥 **[https://youtu.be/esOEIdVb6EA](https://youtu.be/esOEIdVb6EA)**

  ---

  ## 📚 References

  - Pieterse, H. 2021. *The Cyber Threat Landscape in South Africa: A 10-Year Review.* African Journal of Information and Communication, 28(28). https://doi.org/10.23962/10539/32213  
  - SAPS Cybercrime: [www.saps.gov.za](https://www.saps.gov.za)  
  - SABRIC (Banking fraud): [www.sabric.co.za](https://www.sabric.co.za)

  ---

  <div align="center">

  **Emergency: 10111** &nbsp;·&nbsp; **Cybercrime: [www.saps.gov.za](https://www.saps.gov.za)** &nbsp;·&nbsp; **SABRIC: [www.sabric.co.za](https://www.sabric.co.za)**

  *PROG6221 Programming 2A &nbsp;·&nbsp; IIE University &nbsp;·&nbsp; Sekgatla*

  </div>
  