using System;

namespace CybersecurityChatbot
{
    // Data model for a cybersecurity task stored in MySQL
    public class TaskItem
    {
        public int    Id           { get; set; }
        public string Title        { get; set; }
        public string Description  { get; set; }
        public string ReminderDate { get; set; }
        public bool   IsCompleted  { get; set; }
        public string CreatedAt    { get; set; }

        public TaskItem()
        {
            Title        = "";
            Description  = "";
            ReminderDate = "";
            IsCompleted  = false;
            CreatedAt    = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        // Display string used in the ListView
        public override string ToString()
        {
            string status    = IsCompleted ? "[DONE] " : "[TODO] ";
            string reminder  = string.IsNullOrEmpty(ReminderDate)
                               ? ""
                               : "  |  Reminder: " + ReminderDate;
            return status + Title + reminder;
        }
    }
}
