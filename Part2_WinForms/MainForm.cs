using System;
using System.Drawing;
using System.Windows.Forms;

namespace CyberSecurityChatbot
{
    public class MainForm : Form
    {
        // Core components
        private readonly ChatbotEngine _engine = new ChatbotEngine();
        private readonly AudioPlayer _audio = new AudioPlayer();
        private User _user = new User();
        private string? _lastKeyword = null;

        // UI Controls
        private Panel _namePanel = null!;
        private Panel _chatPanel = null!;
        private RichTextBox _chatBox = null!;
        private TextBox _inputBox = null!;
        private Button _sendButton = null!;
        private TextBox _nameBox = null!;
        private Button _startButton = null!;
        private Label _nameError = null!;

        // Colours
        private readonly Color _bgDark      = Color.FromArgb(13, 17, 23);
        private readonly Color _bgMid       = Color.FromArgb(22, 27, 34);
        private readonly Color _bgPanel     = Color.FromArgb(30, 38, 50);
        private readonly Color _green       = Color.FromArgb(63, 185, 80);
        private readonly Color _greenDim    = Color.FromArgb(35, 134, 54);
        private readonly Color _textLight   = Color.FromArgb(230, 237, 243);
        private readonly Color _textMuted   = Color.FromArgb(125, 133, 144);
        private readonly Color _userBubble  = Color.FromArgb(31, 111, 235);
        private readonly Color _botBubble   = Color.FromArgb(33, 41, 54);
        private readonly Color _inputBg     = Color.FromArgb(13, 17, 23);
        private readonly Color _borderColor = Color.FromArgb(48, 54, 61);

        private readonly string[] _quickTopics = {
            "Passwords", "Phishing", "Privacy", "Scams", "VPN", "Malware"
        };

        public MainForm()
        {
            InitialiseForm();
            BuildNamePanel();
            BuildChatPanel();
            ShowNamePanel();
        }

        private void InitialiseForm()
        {
            this.Text = "Cybersecurity Awareness Bot";
            this.Size = new Size(820, 680);
            this.MinimumSize = new Size(640, 520);
            this.BackColor = _bgDark;
            this.ForeColor = _textLight;
            this.Font = new Font("Segoe UI", 10f);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Header panel
            Panel header = new Panel
            {
                Dock = DockStyle.Top,
                Height = 110,
                BackColor = _bgMid,
                Padding = new Padding(16, 8, 16, 8)
            };

            // Title label
            Label titleLabel = new Label
            {
                Text = "🛡  CyberBot — Cybersecurity Awareness Bot",
                ForeColor = _green,
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(16, 10)
            };

            // ASCII art label
            Label asciiLabel = new Label
            {
                Text =
                    "  ____            _               ____        _  \r\n" +
                    " / ___|_   _ ___ | |_ ___ _ __   | __ ) ___ | |_ \r\n" +
                    "| |   | | | / __|| __/ _ \\ '__|  |  _ \\/ _ \\| __|\r\n" +
                    "| |___| |_| \\__ \\| ||  __/ |     | |_)| (_) | |_ \r\n" +
                    " \\____\\__, |___/ \\__\\___|_|     |____/\\___/ \\__|\r\n" +
                    "       |___/",
                ForeColor = _greenDim,
                Font = new Font("Courier New", 7f),
                AutoSize = true,
                Location = new Point(16, 36)
            };

            header.Controls.Add(titleLabel);
            header.Controls.Add(asciiLabel);

            // Bottom border on header
            Panel headerBorder = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = _borderColor
            };

            this.Controls.Add(headerBorder);
            this.Controls.Add(header);
        }

        // ── Name Entry Panel ───────────────────────────────────────────────────

        private void BuildNamePanel()
        {
            _namePanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bgDark,
                Padding = new Padding(80, 40, 80, 40)
            };

            Label prompt = new Label
            {
                Text = "Welcome! I'm here to help you stay safe online.\r\nBefore we begin, what's your name?",
                ForeColor = _textLight,
                Font = new Font("Segoe UI", 11f),
                AutoSize = false,
                Size = new Size(460, 52),
                Location = new Point(0, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            Label nameLabel = new Label
            {
                Text = "YOUR NAME",
                ForeColor = _textMuted,
                Font = new Font("Segoe UI", 8f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(0, 90)
            };

            _nameBox = new TextBox
            {
                Location = new Point(0, 112),
                Size = new Size(460, 36),
                BackColor = _bgMid,
                ForeColor = _textLight,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11f),
                PlaceholderText = "Enter your name..."
            };
            _nameBox.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) StartChat(); };

            _nameError = new Label
            {
                Text = "",
                ForeColor = Color.Tomato,
                Font = new Font("Segoe UI", 9f),
                AutoSize = true,
                Location = new Point(0, 154)
            };

            _startButton = new Button
            {
                Text = "Start Chat",
                Location = new Point(0, 176),
                Size = new Size(460, 42),
                BackColor = _green,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _startButton.FlatAppearance.BorderSize = 0;
            _startButton.Click += (s, e) => StartChat();

            _namePanel.Controls.AddRange(new Control[] { prompt, nameLabel, _nameBox, _nameError, _startButton });
            this.Controls.Add(_namePanel);
        }

        private void StartChat()
        {
            string name = _nameBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                _nameError.Text = "⚠  Name cannot be empty. Please enter your name.";
                return;
            }

            _user = new User { Name = name };
            _audio.PlayGreeting();
            ShowChatPanel();

            string welcome = $"Hello {_user.Name}! Welcome to the Cybersecurity Awareness Bot. " +
                             "I'm here to help you stay safe online. You can ask me about passwords, " +
                             "phishing, privacy, scams, malware, and much more. Type 'help' to see all topics.";
            AppendBotMessage(welcome);
        }

        // ── Chat Panel ─────────────────────────────────────────────────────────

        private void BuildChatPanel()
        {
            _chatPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bgDark,
                Visible = false
            };

            // Chat display area
            _chatBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                BackColor = _bgDark,
                ForeColor = _textLight,
                Font = new Font("Segoe UI", 10f),
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical,
                Padding = new Padding(12),
                WordWrap = true
            };

            // Bottom input area
            Panel inputArea = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 110,
                BackColor = _bgMid,
                Padding = new Padding(12, 8, 12, 8)
            };

            // Top border on input area
            Panel inputBorder = new Panel
            {
                Dock = DockStyle.Top,
                Height = 1,
                BackColor = _borderColor
            };

            // Quick topic buttons
            FlowLayoutPanel quickPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 36,
                BackColor = _bgMid,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = false,
                Padding = new Padding(0, 0, 0, 0)
            };

            foreach (string topic in _quickTopics)
            {
                Button btn = new Button
                {
                    Text = topic,
                    AutoSize = true,
                    BackColor = _bgPanel,
                    ForeColor = _textMuted,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 8.5f),
                    Cursor = Cursors.Hand,
                    Margin = new Padding(0, 4, 6, 4)
                };
                btn.FlatAppearance.BorderColor = _borderColor;
                btn.FlatAppearance.BorderSize = 1;
                string captured = topic;
                btn.Click += (s, e) => SendMessage($"Tell me about {captured}");
                btn.MouseEnter += (s, e) => { btn.ForeColor = _green; btn.FlatAppearance.BorderColor = _greenDim; };
                btn.MouseLeave += (s, e) => { btn.ForeColor = _textMuted; btn.FlatAppearance.BorderColor = _borderColor; };
                quickPanel.Controls.Add(btn);
            }

            // Input row
            Panel inputRow = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = _bgMid,
                Padding = new Padding(0, 6, 0, 0)
            };

            _inputBox = new TextBox
            {
                BackColor = _inputBg,
                ForeColor = _textLight,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 11f),
                PlaceholderText = "Ask a cybersecurity question...",
                Dock = DockStyle.Fill
            };
            _inputBox.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; SendMessage(); }
            };

            _sendButton = new Button
            {
                Text = "Send",
                Dock = DockStyle.Right,
                Width = 80,
                BackColor = _green,
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            _sendButton.FlatAppearance.BorderSize = 0;
            _sendButton.Click += (s, e) => SendMessage();

            inputRow.Controls.Add(_inputBox);
            inputRow.Controls.Add(_sendButton);

            inputArea.Controls.Add(inputRow);
            inputArea.Controls.Add(quickPanel);
            inputArea.Controls.Add(inputBorder);

            _chatPanel.Controls.Add(_chatBox);
            _chatPanel.Controls.Add(inputArea);
            this.Controls.Add(_chatPanel);
        }

        private void SendMessage(string? text = null)
        {
            string userInput = (text ?? _inputBox.Text).Trim();
            if (string.IsNullOrWhiteSpace(userInput)) return;

            _inputBox.Clear();
            AppendUserMessage(userInput);

            // Check if user is sharing their interest (update memory)
            string lower = userInput.ToLower();
            foreach (string prefix in new[] { "i'm interested in", "i am interested in", "i like" })
            {
                int idx = lower.IndexOf(prefix);
                if (idx >= 0)
                {
                    string topic = userInput.Substring(idx + prefix.Length).Trim().TrimEnd('.');
                    _user.FavouriteTopic = topic;
                    break;
                }
            }

            string response = _engine.GenerateResponse(userInput, _user, ref _lastKeyword);
            AppendBotMessage(response);
        }

        // ── Message rendering ──────────────────────────────────────────────────

        private void AppendUserMessage(string text)
        {
            _chatBox.SelectionStart = _chatBox.TextLength;
            _chatBox.SelectionLength = 0;

            // Timestamp
            _chatBox.SelectionColor = _textMuted;
            _chatBox.SelectionFont = new Font("Segoe UI", 8f);
            _chatBox.AppendText($"  {_user.Name}  {DateTime.Now:HH:mm}\n");

            // Message bubble
            _chatBox.SelectionColor = _textLight;
            _chatBox.SelectionFont = new Font("Segoe UI", 10f);
            _chatBox.SelectionBackColor = _userBubble;
            _chatBox.AppendText($"  {text}  \n");
            _chatBox.SelectionBackColor = _bgDark;
            _chatBox.AppendText("\n");

            _chatBox.ScrollToCaret();
        }

        private void AppendBotMessage(string text)
        {
            _chatBox.SelectionStart = _chatBox.TextLength;
            _chatBox.SelectionLength = 0;

            // Timestamp
            _chatBox.SelectionColor = _textMuted;
            _chatBox.SelectionFont = new Font("Segoe UI", 8f);
            _chatBox.AppendText($"  CyberBot  {DateTime.Now:HH:mm}\n");

            // Message bubble
            _chatBox.SelectionColor = _textLight;
            _chatBox.SelectionFont = new Font("Segoe UI", 10f);
            _chatBox.SelectionBackColor = _botBubble;
            _chatBox.AppendText($"  {text}  \n");
            _chatBox.SelectionBackColor = _bgDark;
            _chatBox.AppendText("\n");

            _chatBox.ScrollToCaret();
        }

        // ── Panel switching ────────────────────────────────────────────────────

        private void ShowNamePanel()
        {
            _namePanel.Visible = true;
            _chatPanel.Visible = false;
            _nameBox.Focus();
        }

        private void ShowChatPanel()
        {
            _namePanel.Visible = false;
            _chatPanel.Visible = true;
            _inputBox.Focus();
        }
    }
}
