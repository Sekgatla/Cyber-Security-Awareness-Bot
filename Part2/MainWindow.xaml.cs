using System;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;

        public MainWindow()
        {
            InitializeComponent();
            _chatBot = new ChatBot();

            LoadAsciiArt();
            PlayVoiceGreeting();
            AppendBotMessage(_chatBot.GetGreeting());
        }

        // ── ASCII art ─────────────────────────────────────────────────────

        private void LoadAsciiArt()
        {
            AsciiArtDisplay.Text =
                "   ______      __              _____                      _ __       \n" +
                "  / ____/_  __/ /_  ___  _____/ ___/___  _______  _______(_) /___  __\n" +
                " / /   / / / / __ \\/ _ \\/ ___/\\__ \\/ _ \\/ ___/ / / / ___/ / __/ / / /\n" +
                "/ /___/ /_/ / /_/ /  __/ /   ___/ /  __/ /__/ /_/ / /  / / /_/ /_/ / \n" +
                "\\____/\\__, /_.___/\\___/_/   /____/\\___/\\___/\\__,_/_/  /_/\\__/\\__, /  \n" +
                "     /____/                                                  /____/   ";
        }

        // ── Voice greeting ────────────────────────────────────────────────

        private void PlayVoiceGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("greeting.wav");
                player.Play();
            }
            catch (Exception)
            {
                // WAV file not found — continue silently
            }
        }

        // ── Button and keyboard events ────────────────────────────────────

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                SendMessage();
        }

        // ── Core send logic ───────────────────────────────────────────────

        private void SendMessage()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(input);
            UserInput.Clear();
            UserInput.Focus();

            string response = _chatBot.ProcessInput(input);
            AppendBotMessage(response);
        }

        // ── Chat display helpers ──────────────────────────────────────────

        private void AppendUserMessage(string text)
        {
            // Outer container
            Border bubble = new Border();
            bubble.HorizontalAlignment = HorizontalAlignment.Right;
            bubble.Background          = new SolidColorBrush(Color.FromRgb(21, 47, 72));
            bubble.CornerRadius        = new CornerRadius(10, 10, 2, 10);
            bubble.Padding             = new Thickness(12, 8, 12, 8);
            bubble.Margin              = new Thickness(60, 4, 4, 4);
            bubble.MaxWidth            = 580;

            // Label + message stacked vertically
            StackPanel stack = new StackPanel();

            TextBlock label = new TextBlock();
            label.Text       = "You";
            label.Foreground = new SolidColorBrush(Color.FromRgb(0, 217, 255));
            label.FontWeight = FontWeights.Bold;
            label.FontSize   = 11;
            label.FontFamily = new FontFamily("Segoe UI");
            label.Margin     = new Thickness(0, 0, 0, 3);

            TextBlock message = new TextBlock();
            message.Text        = text;
            message.Foreground  = new SolidColorBrush(Color.FromRgb(230, 237, 243));
            message.FontSize    = 13;
            message.FontFamily  = new FontFamily("Segoe UI");
            message.TextWrapping = TextWrapping.Wrap;

            stack.Children.Add(label);
            stack.Children.Add(message);
            bubble.Child = stack;

            ChatPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToBottom();
        }

        private void AppendBotMessage(string text)
        {
            // Outer container
            Border bubble = new Border();
            bubble.HorizontalAlignment = HorizontalAlignment.Left;
            bubble.Background          = new SolidColorBrush(Color.FromRgb(22, 27, 34));
            bubble.BorderBrush         = new SolidColorBrush(Color.FromRgb(48, 54, 61));
            bubble.BorderThickness     = new Thickness(1);
            bubble.CornerRadius        = new CornerRadius(10, 10, 10, 2);
            bubble.Padding             = new Thickness(12, 8, 12, 8);
            bubble.Margin              = new Thickness(4, 4, 60, 4);
            bubble.MaxWidth            = 580;

            StackPanel stack = new StackPanel();

            TextBlock label = new TextBlock();
            label.Text       = "[CYBER-BOT]";
            label.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 65));
            label.FontWeight = FontWeights.Bold;
            label.FontSize   = 11;
            label.FontFamily = new FontFamily("Courier New");
            label.Margin     = new Thickness(0, 0, 0, 4);

            TextBlock message = new TextBlock();
            message.Text         = text;
            message.Foreground   = new SolidColorBrush(Color.FromRgb(201, 209, 217));
            message.FontSize     = 13;
            message.FontFamily   = new FontFamily("Segoe UI");
            message.TextWrapping = TextWrapping.Wrap;

            stack.Children.Add(label);
            stack.Children.Add(message);
            bubble.Child = stack;

            ChatPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToBottom();
        }
    }
}
