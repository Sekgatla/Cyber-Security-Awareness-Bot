using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace CybersecurityChatbot
{
    // View-model wrapper so ListView can bind to TaskItem properties directly
    public class TaskViewModel
    {
        private TaskItem _item;

        public TaskViewModel(TaskItem item) { _item = item; }

        public int    Id           => _item.Id;
        public string Title        => _item.Title;
        public string Description  => _item.Description;
        public string ReminderDate => _item.ReminderDate;
        public string CreatedAt    => _item.CreatedAt;
        public string StatusText   => _item.IsCompleted ? "✓ Done" : "○ Todo";
        public bool   IsCompleted  => _item.IsCompleted;
    }

    public partial class MainWindow : Window
    {
        private ChatBot _chatBot;
        private bool    _showingFullLog;

        public MainWindow()
        {
            InitializeComponent();
            _chatBot       = new ChatBot();
            _showingFullLog = false;

            PlayVoiceGreeting();
            LoadAsciiArt();

            AppendBotMessage(_chatBot.GetGreeting());

            // Reflect DB status in header
            UpdateDbStatus();

            // Load initial task list
            RefreshTaskList();
        }

        // ── Voice greeting ─────────────────────────────────────────────────

        private void PlayVoiceGreeting()
        {
            try
            {
                string wavPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (File.Exists(wavPath))
                {
                    SoundPlayer player = new SoundPlayer(wavPath);
                    player.Play();
                }
            }
            catch { /* WAV unavailable — continue silently */ }
        }

        // ── ASCII art ──────────────────────────────────────────────────────

        private void LoadAsciiArt()
        {
            AsciiArtDisplay.Text =
                "  ______      _               ____             \n" +
                " / ___|   _| |__   ___ _ __/ ___|  ___  ___ \n" +
                "| |  | | | | '_ \\ / _ \\ '__\\___ \\ / _ \\/ __|\n" +
                "| |__| |_| | |_) |  __/ |   ___) |  __/ (__ \n" +
                " \\____\\__, |_.__/ \\___|_|  |____/ \\___|\\___| v3.0\n" +
                "       |___/   Task Assistant · Quiz · NLP · Activity Log";
        }

        // ── DB status display ──────────────────────────────────────────────

        private void UpdateDbStatus()
        {
            if (_chatBot.TaskManager.IsDatabaseAvailable)
            {
                DbStatusDot.Fill  = new SolidColorBrush(Color.FromRgb(0, 255, 65));
                DbStatusText.Text = "MySQL CONNECTED";
                DbStatusText.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 65));
            }
            else
            {
                DbStatusDot.Fill  = new SolidColorBrush(Color.FromRgb(240, 136, 62));
                DbStatusText.Text = "DB: MEMORY MODE";
                DbStatusText.Foreground = new SolidColorBrush(Color.FromRgb(240, 136, 62));
            }
        }

        // ── Helpers: update user label ─────────────────────────────────────

        private void UpdateUserLabel(string name)
        {
            if (!string.IsNullOrEmpty(name))
                UserLabel.Text = "User: " + name;
        }

        // ══════════════════════════════════════════════════════════════════
        // TAB 1 — CHAT
        // ══════════════════════════════════════════════════════════════════

        private void SendButton_Click(object sender, RoutedEventArgs e) => SendChatMessage();
        private void UserInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) SendChatMessage();
        }

        private void SendChatMessage()
        {
            string input = UserInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(input)) return;

            AppendUserMessage(input);
            UserInput.Clear();
            UserInput.Focus();

            // Check if the quiz is active — route answers through the quiz engine
            string response;
            if (_chatBot.Quiz.IsActive)
                response = _chatBot.HandleQuizAnswer(input);
            else
                response = _chatBot.ProcessInput(input);

            AppendBotMessage(response);

            // Refresh activity log count
            RefreshLogCount();

            // If name was just set, update the user label in the header
            if (!string.IsNullOrEmpty(_chatBot.TaskManager.GetAllTasks().Count.ToString()))
                UpdateUserLabel(GetUserNameFromResponse(response));
        }

        private string GetUserNameFromResponse(string response)
        {
            if (response.StartsWith("Welcome, "))
            {
                int end = response.IndexOf('!');
                if (end > 9) return response.Substring(9, end - 9);
            }
            return "";
        }

        private void AppendUserMessage(string text)
        {
            Border bubble = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                Background          = new SolidColorBrush(Color.FromRgb(21, 47, 72)),
                CornerRadius        = new CornerRadius(10, 10, 2, 10),
                Padding             = new Thickness(12, 8, 12, 8),
                Margin              = new Thickness(60, 4, 4, 4),
                MaxWidth            = 600
            };

            StackPanel stack = new StackPanel();

            TextBlock label = new TextBlock
            {
                Text       = "You",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 217, 255)),
                FontWeight = FontWeights.Bold,
                FontSize   = 11,
                FontFamily = new FontFamily("Segoe UI"),
                Margin     = new Thickness(0, 0, 0, 3)
            };

            TextBlock message = new TextBlock
            {
                Text         = text,
                Foreground   = new SolidColorBrush(Color.FromRgb(230, 237, 243)),
                FontSize     = 13,
                FontFamily   = new FontFamily("Segoe UI"),
                TextWrapping = TextWrapping.Wrap
            };

            stack.Children.Add(label);
            stack.Children.Add(message);
            bubble.Child = stack;

            ChatPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToBottom();
        }

        private void AppendBotMessage(string text)
        {
            Border bubble = new Border
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Background          = new SolidColorBrush(Color.FromRgb(22, 27, 34)),
                BorderBrush         = new SolidColorBrush(Color.FromRgb(48, 54, 61)),
                BorderThickness     = new Thickness(1),
                CornerRadius        = new CornerRadius(10, 10, 10, 2),
                Padding             = new Thickness(12, 8, 12, 8),
                Margin              = new Thickness(4, 4, 60, 4),
                MaxWidth            = 600
            };

            StackPanel stack = new StackPanel();

            TextBlock label = new TextBlock
            {
                Text       = "[CYBER-BOT v3.0]",
                Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 65)),
                FontWeight = FontWeights.Bold,
                FontSize   = 11,
                FontFamily = new FontFamily("Courier New"),
                Margin     = new Thickness(0, 0, 0, 4)
            };

            TextBlock message = new TextBlock
            {
                Text         = text,
                Foreground   = new SolidColorBrush(Color.FromRgb(201, 209, 217)),
                FontSize     = 13,
                FontFamily   = new FontFamily("Segoe UI"),
                TextWrapping = TextWrapping.Wrap
            };

            stack.Children.Add(label);
            stack.Children.Add(message);
            bubble.Child = stack;

            ChatPanel.Children.Add(bubble);
            ChatScrollViewer.ScrollToBottom();
        }

        // ══════════════════════════════════════════════════════════════════
        // TAB 2 — TASK ASSISTANT
        // ══════════════════════════════════════════════════════════════════

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title    = TaskTitleInput.Text.Trim();
            string desc     = TaskDescInput.Text.Trim();
            string reminder = TaskReminderInput.Text.Trim();

            if (reminder.Equals("e.g. in 7 days / tomorrow", StringComparison.OrdinalIgnoreCase))
                reminder = "";

            if (string.IsNullOrEmpty(title))
            {
                TaskStatusMsg.Text       = "Please enter a task title.";
                TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(218, 54, 51));
                return;
            }

            // If the description is empty, auto-generate a cybersecurity description
            if (string.IsNullOrEmpty(desc))
            {
                desc = "Cybersecurity task: " + title + ". Stay protected online.";
            }

            // Resolve "in X days" reminder text to a real date
            string resolvedReminder = ResolveReminder(reminder);

            TaskItem task = _chatBot.TaskManager.AddTask(title, desc, resolvedReminder);
            _chatBot.Logger.Log("Task added via Tasks tab: '" + title + "'"
                                + (string.IsNullOrEmpty(resolvedReminder) ? "" : " — Reminder: " + resolvedReminder));

            // Clear inputs
            TaskTitleInput.Text    = "";
            TaskDescInput.Text     = "";
            TaskReminderInput.Text = "e.g. in 7 days / tomorrow";

            TaskStatusMsg.Text       = "Task added! (ID: " + task.Id + ")";
            TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(63, 185, 80));

            RefreshTaskList();
            RefreshLogCount();
        }

        private string ResolveReminder(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";

            NlpProcessor nlp = new NlpProcessor();
            string resolved  = nlp.ExtractReminderInfo(input.ToLower());
            return string.IsNullOrEmpty(resolved) ? input : resolved;
        }

        private void MarkDoneButton_Click(object sender, RoutedEventArgs e)
        {
            TaskViewModel selected = TaskListView.SelectedItem as TaskViewModel;
            if (selected == null)
            {
                TaskStatusMsg.Text       = "Please select a task first.";
                TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(218, 54, 51));
                return;
            }

            bool ok = _chatBot.TaskManager.MarkCompleted(selected.Id);
            if (ok)
            {
                _chatBot.Logger.Log("Task marked as done: '" + selected.Title + "' (ID: " + selected.Id + ")");
                TaskStatusMsg.Text       = "Task marked as complete!";
                TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(63, 185, 80));
                RefreshTaskList();
                RefreshLogCount();
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            TaskViewModel selected = TaskListView.SelectedItem as TaskViewModel;
            if (selected == null)
            {
                TaskStatusMsg.Text       = "Please select a task to delete.";
                TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(218, 54, 51));
                return;
            }

            MessageBoxResult confirm = MessageBox.Show(
                "Delete task: \"" + selected.Title + "\"?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm == MessageBoxResult.Yes)
            {
                bool ok = _chatBot.TaskManager.DeleteTask(selected.Id);
                if (ok)
                {
                    _chatBot.Logger.Log("Task deleted: '" + selected.Title + "' (ID: " + selected.Id + ")");
                    TaskStatusMsg.Text       = "Task deleted.";
                    TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(218, 54, 51));
                    RefreshTaskList();
                    RefreshLogCount();
                }
            }
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshTaskList();
            TaskStatusMsg.Text       = "Task list refreshed.";
            TaskStatusMsg.Foreground = new SolidColorBrush(Color.FromRgb(63, 185, 80));
        }

        private void RefreshTaskList()
        {
            List<TaskItem> tasks = _chatBot.TaskManager.GetAllTasks();
            TaskListView.Items.Clear();

            foreach (TaskItem task in tasks)
                TaskListView.Items.Add(new TaskViewModel(task));

            TaskCountLabel.Text = "  (" + tasks.Count + " task" + (tasks.Count == 1 ? "" : "s") + ")";
        }

        // ══════════════════════════════════════════════════════════════════
        // TAB 3 — QUIZ
        // ══════════════════════════════════════════════════════════════════

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            _chatBot.Quiz.Start();
            _chatBot.Logger.Log("Quiz started via Quiz tab");

            QuizFeedback.Text      = "";
            QuizStartPrompt.Visibility = Visibility.Collapsed;
            ShowCurrentQuestion();
            RefreshLogCount();
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_chatBot.Quiz.IsActive) return;

            Button btn    = (Button)sender;
            string answer = btn.Tag.ToString();

            QuizQuestion currentQuestion = _chatBot.Quiz.GetCurrentQuestion();
            bool isMultiChoice = currentQuestion?.IsMultiChoice ?? true;

            var (correct, explanation, isGameOver) = _chatBot.Quiz.SubmitAnswer(answer);

            string correctMark = correct ? "✓  CORRECT!" : "✗  Incorrect";
            Color  feedbackClr = correct
                                 ? Color.FromRgb(63, 185, 80)
                                 : Color.FromRgb(218, 54, 51);

            QuizFeedback.Foreground = new SolidColorBrush(feedbackClr);
            QuizFeedback.Text       = correctMark + "\n\n" + explanation;

            UpdateScore();

            if (isGameOver)
            {
                _chatBot.Logger.Log("Quiz finished — Score: " + _chatBot.Quiz.Score + "/" + _chatBot.Quiz.TotalQuestions);

                QuestionNumberLabel.Text = "Quiz Complete!";
                QuestionText.Text        = _chatBot.Quiz.GetFinalFeedback();
                McPanel.Visibility       = Visibility.Collapsed;
                TfPanel.Visibility       = Visibility.Collapsed;
                QuizStartPrompt.Text     = "Click START QUIZ to try again!";
                QuizStartPrompt.Visibility = Visibility.Visible;
                UpdateProgressBar(1.0);
                RefreshLogCount();
            }
            else
            {
                ShowCurrentQuestion();
            }
        }

        private void ShowCurrentQuestion()
        {
            QuizQuestion q = _chatBot.Quiz.GetCurrentQuestion();
            if (q == null) return;

            int total   = _chatBot.Quiz.TotalQuestions;
            int current = _chatBot.Quiz.CurrentIndex + 1;

            QuestionNumberLabel.Text = "Question " + current + " of " + total;
            QuestionText.Text        = q.QuestionText;

            if (q.IsMultiChoice)
            {
                McPanel.Visibility = Visibility.Visible;
                TfPanel.Visibility = Visibility.Collapsed;

                // Update button labels with option text
                if (q.Options.Count >= 4)
                {
                    BtnA.Content = q.Options[0];
                    BtnB.Content = q.Options[1];
                    BtnC.Content = q.Options[2];
                    BtnD.Content = q.Options[3];
                }
            }
            else
            {
                McPanel.Visibility = Visibility.Collapsed;
                TfPanel.Visibility = Visibility.Visible;
            }

            double progress = (double)(_chatBot.Quiz.CurrentIndex) / total;
            UpdateProgressBar(progress);
        }

        private void UpdateScore()
        {
            ScoreLabel.Text = _chatBot.Quiz.Score + " / " + _chatBot.Quiz.TotalQuestions;
        }

        private void UpdateProgressBar(double ratio)
        {
            double maxWidth = 860;
            ProgressBar.Width = ratio * maxWidth;
        }

        // ══════════════════════════════════════════════════════════════════
        // TAB 4 — ACTIVITY LOG
        // ══════════════════════════════════════════════════════════════════

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            _showingFullLog = false;
            RenderLog(_chatBot.Logger.GetRecentLog(10));
        }

        private void ShowFullLogButton_Click(object sender, RoutedEventArgs e)
        {
            _showingFullLog = true;
            RenderLog(_chatBot.Logger.GetFullLog());
        }

        private void RenderLog(List<string> entries)
        {
            LogPanel.Children.Clear();

            if (entries.Count == 0)
            {
                TextBlock empty = new TextBlock
                {
                    Text         = "No activity recorded yet — start chatting, add tasks, or take the quiz!",
                    FontFamily   = new FontFamily("Segoe UI"),
                    FontSize     = 12,
                    Foreground   = new SolidColorBrush(Color.FromRgb(72, 79, 88)),
                    Margin       = new Thickness(0, 4, 0, 4)
                };
                LogPanel.Children.Add(empty);
                return;
            }

            for (int i = 0; i < entries.Count; i++)
            {
                Border entryBorder = new Border
                {
                    Background     = i % 2 == 0
                                     ? new SolidColorBrush(Color.FromRgb(13, 17, 23))
                                     : new SolidColorBrush(Color.FromRgb(22, 27, 34)),
                    CornerRadius   = new CornerRadius(4),
                    Padding        = new Thickness(10, 6, 10, 6),
                    Margin         = new Thickness(0, 2, 0, 2)
                };

                StackPanel row = new StackPanel { Orientation = Orientation.Horizontal };

                TextBlock num = new TextBlock
                {
                    Text       = (i + 1) + ". ",
                    FontFamily = new FontFamily("Courier New"),
                    FontSize   = 11,
                    Foreground = new SolidColorBrush(Color.FromRgb(0, 217, 255)),
                    Width      = 30
                };

                TextBlock entry = new TextBlock
                {
                    Text         = entries[i],
                    FontFamily   = new FontFamily("Segoe UI"),
                    FontSize     = 12,
                    Foreground   = new SolidColorBrush(Color.FromRgb(201, 209, 217)),
                    TextWrapping = TextWrapping.Wrap
                };

                row.Children.Add(num);
                row.Children.Add(entry);
                entryBorder.Child = row;

                LogPanel.Children.Add(entryBorder);
            }

            LogScrollViewer.ScrollToBottom();
            RefreshLogCount();
        }

        private void RefreshLogCount()
        {
            int count      = _chatBot.Logger.Count;
            LogCountLabel.Text = count + " action" + (count == 1 ? "" : "s") + " recorded";
        }
    }
}
