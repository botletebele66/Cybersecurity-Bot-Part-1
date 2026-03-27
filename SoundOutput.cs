using System.Media;
using System.IO;

namespace CybersecurityBot
{
    public class SoundOutput
    {
        public static void PlayGreeting()
        {
            // The file will be copied to the output folder (bin/Debug/netX.X/)
            string audioPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Greeting AI.wav");

            if (File.Exists(audioPath))
            {
                using (SoundPlayer player = new SoundPlayer(audioPath))
                {
                    player.PlaySync(); // plays and waits until finished
                }
            }
            else
            {
                Console.WriteLine("Warning: greeting.wav not found.");
            }
        }
    }
}