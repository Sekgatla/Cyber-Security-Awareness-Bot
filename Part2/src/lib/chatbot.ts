export interface UserMemory {
  name: string;
  favouriteTopic: string | null;
  mentionedTopics: string[];
}

export interface ChatResponse {
  text: string;
  sentiment?: string;
  keyword?: string;
}

const keywordResponses: Record<string, string[]> = {
  password: [
    "Make sure to use strong, unique passwords for each account. Avoid using personal details in your passwords.",
    "A strong password should be at least 12 characters long and include uppercase letters, lowercase letters, numbers, and symbols.",
    "Consider using a password manager to generate and store complex passwords safely — you only need to remember one master password.",
  ],
  phishing: [
    "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
    "Always verify the sender's email address before clicking any links. Legitimate companies will never ask for your password via email.",
    "If an email seems too good to be true or creates urgency, it's likely a phishing attempt. When in doubt, go directly to the official website.",
  ],
  privacy: [
    "Review the privacy settings on your social media accounts regularly to control who can see your information.",
    "Limit the personal information you share online. The less data you expose, the smaller your attack surface.",
    "Use privacy-focused browsers and search engines, and consider a VPN when using public Wi-Fi to protect your data.",
  ],
  scam: [
    "Be wary of unsolicited calls or messages claiming you've won a prize or owe money. Verify directly with the official organisation.",
    "Scammers often create a sense of urgency — take a breath and verify independently before acting on any suspicious request.",
    "Never transfer money or share banking details with someone you haven't verified through official channels.",
  ],
  malware: [
    "Keep your antivirus software up to date and run regular scans to detect and remove malicious software.",
    "Avoid downloading files or software from untrusted sources. Malware is often hidden in free downloads.",
    "Be cautious with email attachments — even from people you know, as their accounts may have been compromised.",
  ],
  vpn: [
    "A VPN (Virtual Private Network) encrypts your internet traffic, making it harder for others to spy on your online activity.",
    "Use a reputable VPN, especially on public Wi-Fi networks like coffee shops or airports, to protect your data.",
    "A VPN hides your IP address and location, adding an extra layer of privacy to your browsing.",
  ],
  firewall: [
    "A firewall acts as a barrier between your trusted network and untrusted external networks — keep it enabled at all times.",
    "Both hardware and software firewalls are important. Ensure your operating system's built-in firewall is active.",
    "Configure your firewall to block inbound connections you don't need, reducing your exposure to attacks.",
  ],
  encryption: [
    "Encryption converts your data into an unreadable format, so only authorised parties with the key can access it.",
    "Use end-to-end encrypted messaging apps like Signal for sensitive communications.",
    "Ensure websites you visit use HTTPS — the 'S' stands for secure and means your connection is encrypted.",
  ],
  "two-factor": [
    "Two-factor authentication (2FA) adds an extra layer of security by requiring a second form of verification beyond just your password.",
    "Even if your password is stolen, 2FA can prevent attackers from accessing your account.",
    "Use an authenticator app like Google Authenticator instead of SMS-based 2FA where possible — it's more secure.",
  ],
  "safe browsing": [
    "Always look for HTTPS and a padlock icon in the address bar before entering sensitive information on a website.",
    "Avoid clicking on pop-up ads or suspicious links. Stick to reputable websites and keep your browser updated.",
    "Use browser extensions like ad-blockers and script blockers to reduce your exposure to malicious content.",
  ],
  ransomware: [
    "Ransomware encrypts your files and demands payment for the key. Keep regular backups so you can recover without paying.",
    "Never open attachments or links from unknown sources — ransomware is often delivered through phishing emails.",
    "Keep your operating system and software up to date to patch vulnerabilities that ransomware exploits.",
  ],
  "social engineering": [
    "Social engineering manipulates people into revealing confidential information. Always verify the identity of anyone requesting sensitive data.",
    "Be skeptical of unexpected requests — even from people who seem to know you. Attackers research their targets.",
    "Trust your instincts. If something feels off about a request, take time to verify through a separate, trusted channel.",
  ],
};

const sentimentPatterns: Record<string, string[]> = {
  worried: ["worried", "scared", "afraid", "anxious", "nervous", "fear", "terrified", "concerned", "panic"],
  frustrated: ["frustrated", "annoyed", "angry", "upset", "confused", "don't understand", "useless", "hate", "terrible"],
  curious: ["curious", "interested", "want to know", "tell me more", "how does", "what is", "explain", "learn", "understand"],
  overwhelmed: ["overwhelmed", "too much", "complicated", "hard", "difficult", "can't keep up", "lost", "stuck"],
};

const sentimentResponses: Record<string, string> = {
  worried: "It's completely understandable to feel that way. Cybersecurity can feel overwhelming, but small steps make a big difference. ",
  frustrated: "I hear you — this stuff can be tricky. Let me try to explain it more clearly. ",
  curious: "That's a great mindset to have! Curiosity is the first step to staying safe online. ",
  overwhelmed: "Don't worry — you don't have to tackle everything at once. Let's focus on one thing at a time. ",
};

const followUpTriggers = ["give me another tip", "tell me more", "explain more", "more info", "another one", "what else", "keep going", "go on"];

function detectSentiment(input: string): string | null {
  const lower = input.toLowerCase();
  for (const [sentiment, patterns] of Object.entries(sentimentPatterns)) {
    if (patterns.some((p) => lower.includes(p))) {
      return sentiment;
    }
  }
  return null;
}

function detectKeyword(input: string): string | null {
  const lower = input.toLowerCase();
  for (const keyword of Object.keys(keywordResponses)) {
    if (lower.includes(keyword)) {
      return keyword;
    }
  }
  return null;
}

function getRandomResponse(responses: string[]): string {
  return responses[Math.floor(Math.random() * responses.length)];
}

export function generateResponse(
  input: string,
  memory: UserMemory,
  lastKeyword: string | null
): { response: ChatResponse; newKeyword: string | null } {
  const lower = input.trim().toLowerCase();

  if (!lower) {
    return {
      response: { text: "Please type something so I can help you!" },
      newKeyword: lastKeyword,
    };
  }

  const sentiment = detectSentiment(lower);
  const sentimentPrefix = sentiment ? sentimentResponses[sentiment] : "";

  if (lower === "exit" || lower === "bye" || lower === "goodbye") {
    return {
      response: {
        text: `Goodbye${memory.name ? ", " + memory.name : ""}! Stay safe online. Remember — cybersecurity is everyone's responsibility.`,
      },
      newKeyword: null,
    };
  }

  if (lower.includes("how are you")) {
    return {
      response: {
        text: "I'm just a program, but I'm here to help you stay safe online! What cybersecurity topic can I help you with today?",
        sentiment: sentiment || undefined,
      },
      newKeyword: lastKeyword,
    };
  }

  if (lower.includes("purpose") || lower.includes("what do you do") || lower.includes("what can you do")) {
    return {
      response: {
        text: "My purpose is to educate you about cybersecurity threats and best practices. I can help you with topics like passwords, phishing, privacy, scams, malware, VPNs, encryption, and more!",
        sentiment: sentiment || undefined,
      },
      newKeyword: lastKeyword,
    };
  }

  if (lower.includes("my name is ") || lower.includes("i am ") || lower.includes("i'm ")) {
    const nameMatch = lower.match(/(?:my name is|i am|i'm)\s+([a-zA-Z]+)/);
    if (nameMatch) {
      return {
        response: {
          text: `Nice to meet you, ${nameMatch[1].charAt(0).toUpperCase() + nameMatch[1].slice(1)}! How can I help you stay safe online today?`,
          sentiment: sentiment || undefined,
        },
        newKeyword: lastKeyword,
      };
    }
  }

  if (lower.includes("i'm interested in") || lower.includes("i like") || lower.includes("i love")) {
    const topicMatch = lower.match(/(?:i'm interested in|i like|i love)\s+(.+)/);
    if (topicMatch) {
      const topic = topicMatch[1].trim();
      return {
        response: {
          text: `Great! I'll remember that you're interested in ${topic}. It's a crucial part of staying safe online. Would you like some tips on that topic?`,
          sentiment: sentiment || undefined,
        },
        newKeyword: lastKeyword,
      };
    }
  }

  if (followUpTriggers.some((t) => lower.includes(t))) {
    if (lastKeyword && keywordResponses[lastKeyword]) {
      const tip = getRandomResponse(keywordResponses[lastKeyword]);
      return {
        response: {
          text: sentimentPrefix + `Here's another tip on ${lastKeyword}: ${tip}`,
          keyword: lastKeyword,
          sentiment: sentiment || undefined,
        },
        newKeyword: lastKeyword,
      };
    }
    return {
      response: {
        text: "Sure! What cybersecurity topic would you like to explore? You can ask about passwords, phishing, privacy, scams, malware, VPNs, encryption, or firewalls.",
        sentiment: sentiment || undefined,
      },
      newKeyword: lastKeyword,
    };
  }

  if (memory.name && (lower.includes("remember me") || lower.includes("who am i") || lower.includes("my name"))) {
    return {
      response: {
        text: `Of course! Your name is ${memory.name}.${memory.favouriteTopic ? ` You mentioned you're interested in ${memory.favouriteTopic}.` : ""}`,
        sentiment: sentiment || undefined,
      },
      newKeyword: lastKeyword,
    };
  }

  if (memory.favouriteTopic && lower.includes(memory.favouriteTopic.toLowerCase())) {
    const keyword = detectKeyword(lower) || memory.favouriteTopic.toLowerCase();
    const responses = keywordResponses[keyword];
    if (responses) {
      const tip = getRandomResponse(responses);
      return {
        response: {
          text: `As someone interested in ${memory.favouriteTopic}, you might want to know: ${tip}`,
          keyword,
          sentiment: sentiment || undefined,
        },
        newKeyword: keyword,
      };
    }
  }

  const keyword = detectKeyword(lower);
  if (keyword && keywordResponses[keyword]) {
    const tip = getRandomResponse(keywordResponses[keyword]);
    return {
      response: {
        text: sentimentPrefix + tip,
        keyword,
        sentiment: sentiment || undefined,
      },
      newKeyword: keyword,
    };
  }

  if (lower.includes("help") || lower.includes("topics") || lower.includes("what can")) {
    return {
      response: {
        text: "I can help you with these cybersecurity topics: **passwords**, **phishing**, **privacy**, **scams**, **malware**, **VPNs**, **firewalls**, **encryption**, **two-factor authentication**, **ransomware**, and **social engineering**. What would you like to know?",
        sentiment: sentiment || undefined,
      },
      newKeyword: lastKeyword,
    };
  }

  return {
    response: {
      text: sentimentPrefix + "I'm not sure I understand that. Could you try rephrasing? You can type 'help' to see what topics I can assist with.",
      sentiment: sentiment || undefined,
    },
    newKeyword: lastKeyword,
  };
}
