# Cyber-Security-Awareness-Bot
# 🛡️ Cyber-Security Awareness Bot

A lightweight, console-based interactive bot built in C# designed to teach fundamental cybersecurity concepts through simple keyword recognition.

![Build Status](https://github.com/Sekgatla/CyberBot/actions/workflows/dotnet.yml/badge.svg)
![C#](https://img.shields.io/badge/Language-C%23-blue)
![.NET](https://img.shields.io/badge/Framework-.NET%208.0-purple)

---

## 🚀 Overview
The **Cyber-Security Awareness Bot** is an educational tool designed to demonstrate the basics of console-based user interaction. It provides quick, accessible advice on digital safety without the complexity of a full AI engine.

### Key Features
* **Audio Greeting:** Welcomes users with a `.wav` voice message upon startup.
* **Interactive Q&A:** Responds to user queries regarding common security threats.
* **Color-Coded UI:** Uses distinct colors and typing effects for a better user experience.
* **Core Topics:**
    * **Passwords:** Best practices for credential safety.
    * **Phishing:** How to spot fraudulent attempts.
    * **Browsing:** Tips for staying safe while surfing the web.

---

## 🛠️ How It Works
This bot operates on a **keyword-matching logic**. It parses user input for specific terms and triggers predefined responses. 

> **Example:** If the input contains "password", the bot provides tips on creating strong, unique credentials.

### Tech Stack
* **Language:** C#
* **Framework:** .NET 8 / .NET 10
* **Audio Engine:** Windows Multimedia API (`winmm.dll`) for `.wav` playback.

---

## 📁 Project Structure
```text
CyberBot/
├── Program.cs        # Entry point and main loop
├── Chatbot.cs        # Logic for keyword matching and responses
├── AudioPlayer.cs    # Wrapper for Windows audio playback
├── User.cs           # Simple profile handling
├── greeting.wav      # Audio asset for the welcome message
└── CyberBot.csproj   # Project configuration

-----
⚙️ Installation & Running
1. Prerequisites: Ensure you have the .NET SDK installed.
2. Restore dependencies: run 'dotnet restore' in your terminal.
3. Run the bot: run 'dotnet run' to start the application.
------
Note: Ensure greeting.wav is in the root directory for audio features.
⚠️ Limitations & Future Scope
- Uses rigid keyword matching rather than NLP.
- No session memory (stateless interaction).
- Audio playback is currently limited to Windows environments.
----
📺 Video Presentation
You can watch the full code walkthrough, project logic explanation, 
and a live demonstration of the bot in action here:

Link: [https://youtu.be/esOEIdVb6EA?si=LkEAjmIFjGDrWwv2]
--------
🤝 Purpose
I created this project to strengthen my skills in:
- Organizing code across multiple files.
- Managing user input loops.
- Integrating external DLLs for hardware interaction (audio).
------
📄 References
Pieterse, H. 2021. The Cyber Threat Landscape in South Africa: A 10-Year Review. 
The African Journal of Information and Communication, 28(28).  
Source: [https://doi.org/10.23962/10539/32213](https://doi.org/10.23962/10539/32213)
