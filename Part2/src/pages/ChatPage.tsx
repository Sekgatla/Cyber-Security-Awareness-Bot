import { useState, useRef, useEffect } from "react";
import { generateResponse, UserMemory } from "@/lib/chatbot";
import { Shield, Send, Bot, User, AlertCircle } from "lucide-react";

interface Message {
  id: number;
  role: "user" | "bot";
  text: string;
  sentiment?: string;
  keyword?: string;
  timestamp: Date;
}

type AppState = "name-entry" | "chat";

const ASCII_ART = `
  ____            _               ____        _   
 / ___|_   _ ___ | |_ ___ _ __   | __ ) ___ | |_ 
| |   | | | / __|| __/ _ \\ '__|  |  _ \\/ _ \\| __|
| |___| |_| \\__ \\| ||  __/ |     | |_)| (_) | |_ 
 \\____|\\__, |___/ \\__\\___|_|     |____/\\___/ \\__|
       |___/                                      `;

const SUGGESTED_TOPICS = [
  "Tell me about passwords",
  "What is phishing?",
  "Privacy tips",
  "How to avoid scams",
  "What is a VPN?",
  "Tell me about malware",
];

const sentimentColors: Record<string, string> = {
  worried: "bg-amber-500/10 border-amber-500/30 text-amber-300",
  frustrated: "bg-red-500/10 border-red-500/30 text-red-300",
  curious: "bg-blue-500/10 border-blue-500/30 text-blue-300",
  overwhelmed: "bg-purple-500/10 border-purple-500/30 text-purple-300",
};

const sentimentLabels: Record<string, string> = {
  worried: "😟 Worried detected — offering reassurance",
  frustrated: "😤 Frustration detected — simplifying explanation",
  curious: "🤔 Curiosity detected — sharing more detail",
  overwhelmed: "😵 Overwhelm detected — breaking it down",
};

function formatText(text: string) {
  const parts = text.split(/\*\*(.*?)\*\*/g);
  return parts.map((part, i) =>
    i % 2 === 1 ? <strong key={i} className="text-green-300 font-semibold">{part}</strong> : part
  );
}

export default function ChatPage() {
  const [appState, setAppState] = useState<AppState>("name-entry");
  const [nameInput, setNameInput] = useState("");
  const [nameError, setNameError] = useState("");
  const [memory, setMemory] = useState<UserMemory>({ name: "", favouriteTopic: null, mentionedTopics: [] });
  const [messages, setMessages] = useState<Message[]>([]);
  const [input, setInput] = useState("");
  const [lastKeyword, setLastKeyword] = useState<string | null>(null);
  const [isTyping, setIsTyping] = useState(false);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const nameInputRef = useRef<HTMLInputElement>(null);
  const msgId = useRef(0);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages, isTyping]);

  useEffect(() => {
    if (appState === "name-entry") {
      nameInputRef.current?.focus();
    } else {
      inputRef.current?.focus();
    }
  }, [appState]);

  function handleNameSubmit(e: React.FormEvent) {
    e.preventDefault();
    const trimmed = nameInput.trim();
    if (!trimmed) {
      setNameError("Name cannot be empty. Please enter your name.");
      return;
    }
    const newMemory: UserMemory = { name: trimmed, favouriteTopic: null, mentionedTopics: [] };
    setMemory(newMemory);
    setAppState("chat");
    const welcomeMsg: Message = {
      id: msgId.current++,
      role: "bot",
      text: `Hello ${trimmed}! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online. You can ask me about passwords, phishing, privacy, scams, malware, and much more. Type 'help' to see all topics.`,
      timestamp: new Date(),
    };
    setMessages([welcomeMsg]);
  }

  function addBotMessage(text: string, sentiment?: string, keyword?: string) {
    setIsTyping(true);
    const delay = Math.min(600 + text.length * 8, 2000);
    setTimeout(() => {
      setIsTyping(false);
      setMessages((prev) => [
        ...prev,
        { id: msgId.current++, role: "bot", text, sentiment, keyword, timestamp: new Date() },
      ]);
    }, delay);
  }

  function handleSend(text?: string) {
    const userText = (text ?? input).trim();
    if (!userText) return;
    setInput("");

    const userMsg: Message = {
      id: msgId.current++,
      role: "user",
      text: userText,
      timestamp: new Date(),
    };
    setMessages((prev) => [...prev, userMsg]);

    const { response, newKeyword } = generateResponse(userText, memory, lastKeyword);
    setLastKeyword(newKeyword);

    const lower = userText.toLowerCase();
    const interestMatch = lower.match(/(?:i'm interested in|i like|i love)\s+([a-zA-Z\s]+)/);
    if (interestMatch) {
      setMemory((prev) => ({ ...prev, favouriteTopic: interestMatch[1].trim() }));
    }

    addBotMessage(response.text, response.sentiment, response.keyword);
  }

  function handleKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  }

  if (appState === "name-entry") {
    return (
      <div className="min-h-screen bg-gray-950 flex items-center justify-center p-4">
        <div className="w-full max-w-lg">
          <div className="bg-gray-900 border border-green-500/30 rounded-2xl overflow-hidden shadow-2xl shadow-green-900/20">
            <div className="bg-gray-950 border-b border-green-500/20 p-6">
              <div className="flex items-center gap-3 mb-4">
                <div className="w-10 h-10 rounded-full bg-green-500/20 border border-green-500/40 flex items-center justify-center">
                  <Shield className="w-5 h-5 text-green-400" />
                </div>
                <div>
                  <h1 className="text-green-400 font-bold text-lg">CyberBot</h1>
                  <p className="text-gray-500 text-xs">Cybersecurity Awareness Bot</p>
                </div>
              </div>
              <pre className="text-green-400 text-[7px] sm:text-[9px] leading-[1.15] font-mono overflow-x-auto whitespace-pre select-none">
                {ASCII_ART}
              </pre>
            </div>
            <div className="p-6">
              <p className="text-gray-300 text-sm mb-6 leading-relaxed">
                Welcome! I'm here to help you stay safe online. Before we begin, what's your name?
              </p>
              <form onSubmit={handleNameSubmit} className="space-y-4">
                <div>
                  <label className="block text-gray-400 text-xs font-medium mb-2 uppercase tracking-wider">
                    Your Name
                  </label>
                  <input
                    ref={nameInputRef}
                    type="text"
                    value={nameInput}
                    onChange={(e) => { setNameInput(e.target.value); setNameError(""); }}
                    placeholder="Enter your name..."
                    className="w-full bg-gray-800 border border-gray-700 focus:border-green-500 rounded-lg px-4 py-3 text-white placeholder-gray-500 outline-none transition-colors text-sm"
                  />
                  {nameError && (
                    <p className="mt-2 text-red-400 text-xs flex items-center gap-1">
                      <AlertCircle className="w-3 h-3" />
                      {nameError}
                    </p>
                  )}
                </div>
                <button
                  type="submit"
                  className="w-full bg-green-600 hover:bg-green-500 text-white font-semibold py-3 rounded-lg transition-colors text-sm flex items-center justify-center gap-2"
                >
                  <Shield className="w-4 h-4" />
                  Start Chat
                </button>
              </form>
            </div>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="min-h-screen bg-gray-950 flex flex-col">
      <header className="bg-gray-900 border-b border-green-500/20 px-4 py-3 flex items-center gap-3 shadow-lg">
        <div className="w-9 h-9 rounded-full bg-green-500/20 border border-green-500/40 flex items-center justify-center flex-shrink-0">
          <Shield className="w-4 h-4 text-green-400" />
        </div>
        <div className="flex-1 min-w-0">
          <h1 className="text-green-400 font-bold text-sm">CyberBot</h1>
          <p className="text-gray-500 text-xs truncate">Cybersecurity Awareness Bot • Chatting with {memory.name}</p>
        </div>
        <div className="flex items-center gap-1.5">
          <div className="w-2 h-2 rounded-full bg-green-400 animate-pulse" />
          <span className="text-green-400 text-xs">Online</span>
        </div>
      </header>

      <div className="flex-1 overflow-y-auto px-4 py-4 space-y-4">
        {messages.map((msg) => (
          <div key={msg.id} className={`flex gap-3 ${msg.role === "user" ? "flex-row-reverse" : "flex-row"}`}>
            <div className={`w-8 h-8 rounded-full flex-shrink-0 flex items-center justify-center border ${
              msg.role === "bot"
                ? "bg-green-500/20 border-green-500/40"
                : "bg-blue-500/20 border-blue-500/40"
            }`}>
              {msg.role === "bot"
                ? <Bot className="w-4 h-4 text-green-400" />
                : <User className="w-4 h-4 text-blue-400" />}
            </div>
            <div className={`max-w-[75%] space-y-1 ${msg.role === "user" ? "items-end" : "items-start"} flex flex-col`}>
              {msg.sentiment && sentimentLabels[msg.sentiment] && (
                <div className={`text-xs px-2 py-0.5 rounded-full border ${sentimentColors[msg.sentiment]}`}>
                  {sentimentLabels[msg.sentiment]}
                </div>
              )}
              {msg.keyword && (
                <div className="text-xs px-2 py-0.5 rounded-full bg-green-500/10 border border-green-500/20 text-green-400">
                  🔑 Topic: {msg.keyword}
                </div>
              )}
              <div className={`px-4 py-3 rounded-2xl text-sm leading-relaxed ${
                msg.role === "bot"
                  ? "bg-gray-800 text-gray-100 border border-gray-700 rounded-tl-sm"
                  : "bg-blue-600 text-white rounded-tr-sm"
              }`}>
                {formatText(msg.text)}
              </div>
              <span className="text-gray-600 text-xs px-1">
                {msg.timestamp.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" })}
              </span>
            </div>
          </div>
        ))}

        {isTyping && (
          <div className="flex gap-3">
            <div className="w-8 h-8 rounded-full flex-shrink-0 flex items-center justify-center border bg-green-500/20 border-green-500/40">
              <Bot className="w-4 h-4 text-green-400" />
            </div>
            <div className="bg-gray-800 border border-gray-700 rounded-2xl rounded-tl-sm px-4 py-3">
              <div className="flex gap-1 items-center h-4">
                <span className="w-2 h-2 rounded-full bg-green-400 animate-bounce [animation-delay:0ms]" />
                <span className="w-2 h-2 rounded-full bg-green-400 animate-bounce [animation-delay:150ms]" />
                <span className="w-2 h-2 rounded-full bg-green-400 animate-bounce [animation-delay:300ms]" />
              </div>
            </div>
          </div>
        )}
        <div ref={messagesEndRef} />
      </div>

      <div className="border-t border-gray-800 bg-gray-900 px-4 py-3 space-y-3">
        <div className="flex gap-2 overflow-x-auto pb-1 scrollbar-hide">
          {SUGGESTED_TOPICS.map((topic) => (
            <button
              key={topic}
              onClick={() => handleSend(topic)}
              disabled={isTyping}
              className="flex-shrink-0 text-xs bg-gray-800 hover:bg-gray-700 border border-gray-700 hover:border-green-500/50 text-gray-300 hover:text-green-300 px-3 py-1.5 rounded-full transition-all disabled:opacity-40"
            >
              {topic}
            </button>
          ))}
        </div>
        <div className="flex gap-2 items-center">
          <input
            ref={inputRef}
            type="text"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            onKeyDown={handleKeyDown}
            placeholder="Ask a cybersecurity question..."
            disabled={isTyping}
            className="flex-1 bg-gray-800 border border-gray-700 focus:border-green-500 rounded-xl px-4 py-3 text-white placeholder-gray-500 outline-none transition-colors text-sm disabled:opacity-50"
          />
          <button
            onClick={() => handleSend()}
            disabled={!input.trim() || isTyping}
            className="w-11 h-11 rounded-xl bg-green-600 hover:bg-green-500 disabled:bg-gray-700 disabled:text-gray-500 text-white flex items-center justify-center transition-all flex-shrink-0"
          >
            <Send className="w-4 h-4" />
          </button>
        </div>
      </div>
    </div>
  );
}
