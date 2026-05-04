using System;
using System.IO;
using System.Media;

namespace CyberSecurityChatbot
{
    public class AudioPlayer
    {
        private readonly string _filePath = "greeting.wav";

        public void PlayGreeting()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    SoundPlayer player = new SoundPlayer(_filePath);
                    player.Play();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing audio: " + ex.Message);
            }
        }
    }
}
