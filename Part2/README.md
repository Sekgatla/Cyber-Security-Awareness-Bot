# Cybersecurity Awareness Bot - Part 2

## GUI Interface, Dynamic Responses, Sentiment Detection, and Memory

### Description
Part 2 expands the Cybersecurity Awareness Chatbot with a full Graphical User Interface, dynamic keyword responses, sentiment detection, memory and recall, and conversation flow.

### Features Implemented

1. **GUI Design** - Dark-themed chat interface with ASCII art header, colour-coded messages, and user-friendly layout
2. **Keyword Recognition** - Recognises 12 cybersecurity keywords: password, phishing, privacy, scam, malware, VPN, firewall, encryption, two-factor authentication, safe browsing, ransomware, social engineering
3. **Random Responses** - Each topic has 3 different responses selected randomly for variety
4. **Conversation Flow** - Handles follow-up phrases like "tell me more", "give me another tip", "explain more"
5. **Memory and Recall** - Stores user name and favourite topic, references them throughout the conversation
6. **Sentiment Detection** - Detects worried, frustrated, curious, and overwhelmed sentiments and adjusts responses accordingly
7. **Error Handling** - Default response for unknown inputs with helpful suggestions
8. **Code Optimisation** - Clean separation of logic (chatbot.ts engine) and UI (ChatPage.tsx) using dictionaries, arrays, and OOP practices

### Technologies Used
- React + TypeScript (GUI equivalent of WPF/WinForm)
- Vite (build tool)
- Tailwind CSS (styling)

### Project Structure
```
Part2/
├── src/
│   ├── lib/
│   │   └── chatbot.ts       # Chatbot engine: keywords, sentiment, memory, responses
│   ├── pages/
│   │   └── ChatPage.tsx     # Main GUI component
│   ├── App.tsx              # App entry point
│   └── index.css            # Styling and theme
└── package.json
```
