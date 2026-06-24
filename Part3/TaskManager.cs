using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CybersecurityChatbot
{
    // Manages cybersecurity task persistence using a MySQL database.
    // To configure: update ConnectionString below with your MySQL server details.
    //
    // Required MySQL setup (run once):
    //   CREATE DATABASE IF NOT EXISTS cyberbot_db;
    //   USE cyberbot_db;
    //   CREATE TABLE IF NOT EXISTS tasks (
    //       id           INT AUTO_INCREMENT PRIMARY KEY,
    //       title        VARCHAR(255) NOT NULL,
    //       description  TEXT,
    //       reminder_date VARCHAR(150),
    //       is_completed  TINYINT(1) DEFAULT 0,
    //       created_at   DATETIME DEFAULT CURRENT_TIMESTAMP
    //   );
    //
    public class TaskManager
    {
        // ── Connection string — update if your MySQL uses a password ─────────
        private const string ConnectionString =
            "Server=localhost;Database=cyberbot_db;Uid=root;Pwd=;";

        private bool _dbAvailable;

        public TaskManager()
        {
            _dbAvailable = false;
            TryInitialiseDatabase();
        }

        // ── Database initialisation ──────────────────────────────────────────

        private void TryInitialiseDatabase()
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                {
                    conn.Open();

                    // Ensure the tasks table exists
                    string createTable = @"
                        CREATE TABLE IF NOT EXISTS tasks (
                            id            INT AUTO_INCREMENT PRIMARY KEY,
                            title         VARCHAR(255) NOT NULL,
                            description   TEXT,
                            reminder_date VARCHAR(150),
                            is_completed  TINYINT(1) DEFAULT 0,
                            created_at    DATETIME DEFAULT CURRENT_TIMESTAMP
                        );";

                    using (MySqlCommand cmd = new MySqlCommand(createTable, conn))
                        cmd.ExecuteNonQuery();

                    _dbAvailable = true;
                }
            }
            catch (Exception)
            {
                // MySQL not available — tasks will be managed in memory only
                _dbAvailable = false;
            }
        }

        public bool IsDatabaseAvailable => _dbAvailable;

        // ── In-memory fallback (used when MySQL is unavailable) ──────────────
        private List<TaskItem> _memoryTasks = new List<TaskItem>();
        private int            _nextId      = 1;

        // ── CRUD — Add ───────────────────────────────────────────────────────

        public TaskItem AddTask(string title, string description, string reminderDate)
        {
            TaskItem item = new TaskItem
            {
                Title        = title,
                Description  = description,
                ReminderDate = reminderDate,
                IsCompleted  = false,
                CreatedAt    = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            if (_dbAvailable)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();

                        string sql = @"
                            INSERT INTO tasks (title, description, reminder_date)
                            VALUES (@title, @desc, @reminder);
                            SELECT LAST_INSERT_ID();";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@title",   title);
                            cmd.Parameters.AddWithValue("@desc",    description);
                            cmd.Parameters.AddWithValue("@reminder", reminderDate ?? "");

                            item.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                    }
                    return item;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DB AddTask error: " + ex.Message);
                    // Fall through to memory store
                }
            }

            // Memory fallback
            item.Id = _nextId++;
            _memoryTasks.Add(item);
            return item;
        }

        // ── CRUD — Read All ──────────────────────────────────────────────────

        public List<TaskItem> GetAllTasks()
        {
            if (_dbAvailable)
            {
                try
                {
                    List<TaskItem> tasks = new List<TaskItem>();

                    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();

                        string sql = "SELECT id, title, description, reminder_date, is_completed, created_at FROM tasks ORDER BY id DESC;";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tasks.Add(new TaskItem
                                {
                                    Id           = reader.GetInt32(0),
                                    Title        = reader.GetString(1),
                                    Description  = reader.IsDBNull(2) ? "" : reader.GetString(2),
                                    ReminderDate = reader.IsDBNull(3) ? "" : reader.GetString(3),
                                    IsCompleted  = reader.GetInt32(4) == 1,
                                    CreatedAt    = reader.IsDBNull(5) ? "" : reader.GetString(5)
                                });
                            }
                        }
                    }
                    return tasks;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DB GetAllTasks error: " + ex.Message);
                }
            }

            return new List<TaskItem>(_memoryTasks);
        }

        // ── CRUD — Mark Completed ────────────────────────────────────────────

        public bool MarkCompleted(int id)
        {
            if (_dbAvailable)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();
                        string sql = "UPDATE tasks SET is_completed = 1 WHERE id = @id;";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DB MarkCompleted error: " + ex.Message);
                }
            }

            // Memory fallback
            TaskItem task = _memoryTasks.Find(t => t.Id == id);
            if (task != null) { task.IsCompleted = true; return true; }
            return false;
        }

        // ── CRUD — Delete ────────────────────────────────────────────────────

        public bool DeleteTask(int id)
        {
            if (_dbAvailable)
            {
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(ConnectionString))
                    {
                        conn.Open();
                        string sql = "DELETE FROM tasks WHERE id = @id;";

                        using (MySqlCommand cmd = new MySqlCommand(sql, conn))
                        {
                            cmd.Parameters.AddWithValue("@id", id);
                            return cmd.ExecuteNonQuery() > 0;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("DB DeleteTask error: " + ex.Message);
                }
            }

            // Memory fallback
            int removed = _memoryTasks.RemoveAll(t => t.Id == id);
            return removed > 0;
        }
    }
}
