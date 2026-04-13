using System;
using System.IO;
using System.Media;

namespace CyberSecurityChatbot
{
    public class AudioPlayer
    {
        // Path to audio file
        private readonly string _filePath = "greeting.wav";

        public void PlayGreeting()
        {
            try
            {
                // Check if file exists
                if (File.Exists(_filePath))
                {
                    // Create SoundPlayer with file path
                    SoundPlayer player = new SoundPlayer(_filePath);

                    // Play audio and wait until finished
                    player.PlaySync();
                }
                else
                {
                    // Display warning if file is missing
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Warning: greeting.wav not found.");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                // Handle any playback errors
                Console.WriteLine("Error playing audio: " + ex.Message);
            }
        }
    }
}
