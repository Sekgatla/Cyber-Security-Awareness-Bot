<div align="center">

    <img src="https://readme-typing-svg.demolab.com?font=Fira+Code&size=22&pause=1000&color=00D9FF&center=true&vCenter=true&width=600&lines=Cybersecurity+Awareness+Bot;Protecting+South+Africans+Online;PROG6221+%7C+IIE+University" alt="Typing SVG" />

    ```
      ______      _               ____
     / ___|   _| |__   ___ _ __/ ___|  ___  ___
    | |  | | | | '_ \ / _ \ '__\___ \ / _ \/ __|
    | |__| |_| | |_) |  __/ |   ___) |  __/ (__
     \____\__, |_.__/ \___|_|  |____/ \___|\___|  v3.0
          |___/    Cybersecurity Awareness Bot
    ```

    [![Build](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/actions)
    [![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
    [![C#](https://img.shields.io/badge/C%23-WPF%20%7C%20Console-239120?style=flat-square&logo=csharp&logoColor=white)](https://docs.microsoft.com/dotnet/csharp/)
    [![Windows](https://img.shields.io/badge/Windows-Required-0078D6?style=flat-square&logo=windows&logoColor=white)](https://microsoft.com/windows)
    [![MySQL](https://img.shields.io/badge/MySQL-8.0-4479A1?style=flat-square&logo=mysql&logoColor=white)](https://mysql.com/)
    [![License](https://img.shields.io/badge/License-Academic-orange?style=flat-square)](LICENSE)

    > *"In a world where cyber threats grow every day — knowledge is your best defence."*

    **Created by Sekgatla · IIE University · PROG6221 Programming 2A**

  </div>

  ---

  ## Table of Contents

  - [Overview](#overview)
  - [Part 1 — Console Application](#part-1--console-application)
  - [Part 2 — WPF GUI Application](#part-2--wpf-gui-application)
  - [Part 3 — Enhanced WPF GUI (POE)](#part-3--enhanced-wpf-gui-poe)
    - [Task 1: Task Assistant with MySQL](#task-1-task-assistant-with-mysql)
    - [Task 2: Cybersecurity Mini-Game Quiz](#task-2-cybersecurity-mini-game-quiz)
    - [Task 3: NLP Simulation](#task-3-nlp-simulation)
    - [Task 4: Activity Log Feature](#task-4-activity-log-feature)
    - [Part 3 Project Structure](#part-3-project-structure)
    - [How to Run Part 3](#how-to-run-part-3)
  - [GitHub Releases](#github-releases)
  - [Video Presentation](#video-presentation)
  - [References](#references)

  ---

  ## Overview

  The **Cybersecurity Awareness Bot** is a C# application designed to educate South African citizens about online threats in an interactive and engaging way.

  | | Part | Type | Description |
  |---|---|---|---|
  | 🖥️ | **Part 1** | C# Console Application | Keyword recognition, colour-coded severity, voice greeting, typing animation, session statistics |
  | 🪟 | **Part 2** | C# WPF GUI | Sentiment detection, conversation memory, random responses, delegate usage |
  | 🚀 | **Part 3** | C# WPF GUI (POE) | Task Assistant (MySQL), Quiz Game, NLP Simulation, Activity Log |

  ---

  ## Part 1 — Console Application

  Part 1 is a fully featured C# console application. It greets the user by name, detects cybersecurity keywords, and responds with colour-coded severity-rated information across 9 topics.

  **How to Run:**
  ```bash
  git clone https://github.com/Sekgatla/Cyber-Security-Awareness-Bot.git
  cd Cyber-Security-Awareness-Bot
  dotnet run
  ```

  ---

  ## Part 2 — WPF GUI Application

  Part 2 extends the console chatbot into a full WPF desktop application with a modern dark-themed interface. Features keyword recognition with random responses, sentiment detection, conversation memory, follow-up handling, and a `ResponseTransformer` delegate.

  **How to Run:**
  1. Open Visual Studio 2022
  2. Open `Part2/CybersecurityChatbot.csproj`
  3. Press **F5**

  ---

  ## Part 3 — Enhanced WPF GUI (POE)

  Part 3 is the final submission for PROG6221. It builds on Parts 1 and 2 by adding four major features through a tabbed WPF interface.

  ### Task 1: Task Assistant with MySQL

  **Files:** `TaskItem.cs`, `TaskManager.cs`

  Users can manage cybersecurity-related tasks stored in a **MySQL database** (`cyberbot_db`).

  | Feature | Implementation |
  |---|---|
  | Add task with details | `TaskManager.AddTask(title, description, reminderDate)` inserts into MySQL `tasks` table |
  | Reminder support | NLP extracts timing ("in 3 days", "tomorrow") and stores as human-readable date |
  | View all tasks | `TaskManager.GetAllTasks()` — displayed in a `ListView` with columns |
  | Mark as completed | `TaskManager.MarkCompleted(id)` — `UPDATE tasks SET is_completed = 1` |
  | Delete tasks | `TaskManager.DeleteTask(id)` — `DELETE FROM tasks WHERE id = @id` |
  | Memory fallback | If MySQL is unavailable, tasks are stored in-memory — app never crashes |

  **MySQL Setup (run once):**
  ```sql
  CREATE DATABASE IF NOT EXISTS cyberbot_db;
  ```
  The `tasks` table is created automatically by `TaskManager` on startup.

  **Example interaction (via chat):**
  ```
  User:    Add task: Review privacy settings
  Bot:     Task ready to add: Review privacy settings
           Would you like a reminder?
  User:    Yes, remind me in 3 days
  Bot:     Done! Reminder: 27 June 2026 (3 days from now)
  ```

  ---

  ### Task 2: Cybersecurity Mini-Game Quiz

  **File:** `QuizEngine.cs`

  A 14-question cybersecurity quiz with multiple-choice and true/false formats. Questions are shuffled on every run.

  | Feature | Detail |
  |---|---|
  | Question types | Multiple-choice (A/B/C/D) and True/False |
  | Topics covered | Phishing, 2FA, passwords, ransomware, VPN, public Wi-Fi, HTTPS, social engineering, USB attacks, SABRIC |
  | Immediate feedback | Correct/incorrect shown after each answer with a full explanation |
  | Score tracking | Live score display; final score shown at end |
  | Final feedback | 5-tier message based on score percentage (0–40%, 40–60%, 60–80%, 80–100%, 100%) |

  **Example question:**
  ```
  Question 4 of 14:
  What is the BEST action when you receive a suspicious email asking for your password?
    A) Reply with your password
    B) Delete the email
    C) Report the email as phishing
    D) Ignore it and do nothing

  → Correct! Reporting phishing emails helps your provider protect others.
  ```

  ---

  ### Task 3: NLP Simulation

  **File:** `NlpProcessor.cs`

  The chatbot recognises user intent from natural language using keyword lists, string manipulation, and regular expressions.

  | NLP Intent | Example user input |
  |---|---|
  | `AddTask` | "Add task to enable 2FA", "I need to update my password" |
  | `SetReminder` | "Remind me to check privacy settings in 7 days" |
  | `ViewTasks` | "Show my tasks", "What do I need to do?" |
  | `StartQuiz` | "Quiz me", "Test my knowledge", "Start quiz" |
  | `ShowActivityLog` | "Show activity log", "What have you done?" |
  | `Greeting` | "Hello", "Hey", "Howzit" |
  | `Farewell` | "Bye", "Exit", "See you" |

  **Reminder extraction using regex:**
  - `"in 3 days"` → calculates real date → `"27 June 2026 (3 days from now)"`
  - `"in 2 weeks"` → `"08 July 2026 (2 weeks from now)"`
  - `"on Friday"` → `"this Friday"`
  - `"tomorrow"` → `"tomorrow"`

  ---

  ### Task 4: Activity Log Feature

  **File:** `ActivityLogger.cs`

  Every significant bot action is recorded with a timestamp and displayed in the Activity Log tab.

  **Logged events:**
  - User name registered
  - Cybersecurity tip provided (topic name)
  - Task added / marked done / deleted
  - Reminder set
  - Quiz started / completed (with final score)
  - Favourite topic stored
  - Activity log viewed
  - Help/topics requested

  **Commands (in chat):**
  ```
  User: Show activity log
  User: What have you done?
  Bot:  Here is a summary of recent actions:
        1. [09:14:22] User registered as: Sekgatla
        2. [09:15:03] Cybersecurity tip provided: phishing
        3. [09:16:44] Task added with reminder: 'Enable 2FA' — 3 days from now
        4. [09:18:10] Quiz started by user
        5. [09:19:55] Quiz finished — Score: 11/14
  ```

  ---

  ### Part 3 Project Structure

  ```
  Part3/
  ├── CybersecurityChatbotPart3.csproj  ← .NET 8 WPF + MySql.Data NuGet reference
  ├── App.xaml / App.xaml.cs            ← Application entry point
  ├── MainWindow.xaml                   ← 4-tab dark-themed WPF GUI
  │   (Chat · Tasks · Quiz · Activity Log)
  ├── MainWindow.xaml.cs                ← Thin code-behind, all UI events
  ├── ChatBot.cs                        ← Extended routing: NLP → quiz → task → log → tips
  ├── NlpProcessor.cs                   ← NEW: Intent detection + regex reminder extraction
  ├── TaskItem.cs                       ← NEW: Task data model (Id, Title, Desc, Reminder, Done)
  ├── TaskManager.cs                    ← NEW: MySQL CRUD with in-memory fallback
  ├── ActivityLogger.cs                 ← NEW: Timestamped action log (up to 50 entries)
  ├── QuizEngine.cs                     ← NEW: 14-question quiz, shuffle, score, feedback
  ├── KeywordResponder.cs               ← Carried forward from Part 2 (8 topics × 5 responses)
  ├── SentimentDetector.cs              ← Carried forward from Part 2 (5 sentiment types)
  ├── MemoryStore.cs                    ← Carried forward from Part 2 (name, favourite topic)
  └── greeting.wav                      ← Voice greeting played on startup
  ```

  ### How to Run Part 3

  **Requirements:** .NET 8 SDK · Visual Studio 2022 · Windows · MySQL Server (optional)

  ```bash
  # MySQL setup (optional — app works without it using memory fallback)
  mysql -u root -e "CREATE DATABASE IF NOT EXISTS cyberbot_db;"
  ```

  1. Open Visual Studio 2022
  2. **File → Open → Project/Solution**
  3. Navigate to `Part3/` and open `CybersecurityChatbotPart3.csproj`
  4. Press **F5** to build and run

  The voice greeting plays on startup. The tabbed interface opens with Chat, Tasks, Quiz, and Activity Log.

  ---

  ## GitHub Releases

  | Release | Tag | Contents |
  |---|---|---|
  | [Part 1 — Console App](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v1.0) | `v1.0` | Console chatbot with 9 topics, voice greeting, typing animation |
  | [Part 2 — WPF GUI](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v2.0) | `v2.0` | WPF desktop app with sentiment detection, memory, delegate usage |
  | [Part 3 — POE Complete](https://github.com/Sekgatla/Cyber-Security-Awareness-Bot/releases/tag/v3.0) | `v3.0` | Task Assistant (MySQL), Quiz Game, NLP Simulation, Activity Log |

  ---

  ## Video Presentation

  - Part 1: https://youtu.be/esOEIdVb6EA?si=0P01PccYEdY_jaEJ
  - Part 2: https://youtu.be/vW_tzyP-inA?si=MMGAnaHtLS-43mrW
  - Part 3: _(add YouTube unlisted link here before submission)_

  ---

  ## References

  - Pieterse, H. 2021. *The Cyber Threat Landscape in South Africa: A 10-Year Review.* African Journal of Information and Communication, 28(28). https://doi.org/10.23962/10539/32213
  - South African Police Service — Cybercrime Division: https://www.saps.gov.za
  - South African Banking Risk Information Centre (SABRIC): https://www.sabric.co.za
  - Microsoft .NET 8 Documentation: https://learn.microsoft.com/dotnet/
  - Windows Presentation Foundation (WPF) Guide: https://learn.microsoft.com/dotnet/desktop/wpf/
  - MySQL .NET Connector (MySql.Data): https://dev.mysql.com/doc/connector-net/en/

  ---

  <div align="center">

  **🛡️ Stay Safe Online — Knowledge Is Your Best Defence 🛡️**

  *PROG6221 Programming 2A · IIE University · Sekgatla*

  </div>
  