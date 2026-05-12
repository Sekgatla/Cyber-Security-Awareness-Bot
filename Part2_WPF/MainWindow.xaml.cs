using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    // Code-behind: handles UI events only.
    // All chatbot logic is delegated to ChatBot.cs
    public partial class MainWindow : Window
    {
        private readonly ChatBot _chatBot;

        public MainWindow()
        {
            InitializeComponent();

            _chatBot = new ChatBot();

            // Play voice greeting on startup
            PlayVoiceGreeting();

            // Load ASCII art into the header
            LoadAsciiArt();

            // Show the opening bot message
            string greeting = _chatBot.GetGreeting();
            AppendBotMessage(greeting);
        }

        // ── Startup tasks ──────────────────────────────────────────────────

        private void PlayVoiceGreeting()
        {
            try
            {
                string wavPath = "greeting.wav";
                if (File.Exists(wavPath))
                {
                    SoundPlayer player = new SoundPlayer(wavPath);
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                // Non-fatal: log to debug output but do not crash
                System.Diagnostics.Debug.WriteLine("Audio error: " + ex.Message);
            }
        }

        private void LoadAsciiArt()
        {
            AsciiArtBlock.Text =
                "  ____            _               ____        _  \r\n" +
                " / ___|_   _ ___ | |_ ___ _ __   | __ ) ___ | |_ \r\n" +
                "| |   | | | / __|| __/ _ \\ '__|  |  _ \\/ _ \\| __|\r\n" +
                "| |___| |_| \\__ \\| ||  __/ |     | |_)| (_) | |_ \r\n" +
                " \\____\\__, |___/ \\__\\___|_|     |____/\\___/ \\__|\r\n" +
                "       |___/";
        }

        // ── UI event handlers ──────────────────────────────────────────────

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        private void QuickTopic_Click(object sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.Button btn)
                SendMessage($"Tell me about {btn.Content}");
        }

        // ── Message handling ───────────────────────────────────────────────

        private void SendMessage(string? overrideText = null)
        {
            string input = (overrideText ?? UserInputBox.Text).Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            UserInputBox.Clear();
            AppendUserMessage(input);

            string response = _chatBot.ProcessInput(input);
            AppendBotMessage(response);
        }

        // ── Chat display helpers ───────────────────────────────────────────

        private void AppendUserMessage(string text)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");

            // Sender label
            Run label = new Run($"You  {timestamp}\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(125, 133, 144)),
                FontSize = 9
            };

            // Message text
            Run message = new Run($"  {text}\n\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(230, 237, 243)),
                FontSize = 11
            };

            ChatDisplay.Inlines.Add(label);
            ChatDisplay.Inlines.Add(message);
            ScrollToBottom();
        }

        private void AppendBotMessage(string text)
        {
            string timestamp = DateTime.Now.ToString("HH:mm");

            // Bot label
            Run label = new Run($"CyberBot  {timestamp}\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(63, 185, 80)),
                FontSize = 9,
                FontWeight = FontWeights.Bold
            };

            // Bot message text
            Run message = new Run($"  {text}\n\n")
            {
                Foreground = new SolidColorBrush(Color.FromRgb(230, 237, 243)),
                FontSize = 11
            };

            ChatDisplay.Inlines.Add(label);
            ChatDisplay.Inlines.Add(message);
            ScrollToBottom();
        }

        private void ScrollToBottom()
        {
            ChatScrollViewer.ScrollToBottom();
        }
    }
}
