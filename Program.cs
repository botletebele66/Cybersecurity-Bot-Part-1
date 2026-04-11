using System;

namespace CybersecurityBot
{
    /// <summary>
    /// Entry point. Orchestrates startup sequence:
    /// 1. Play voice greeting (non-blocking)
    /// 2. Display ASCII header
    /// 3. Hand off to Conversation loop
    /// </summary>
    class Program
    {
        static void Main()
        {
            Console.Title = "CyBrog – Cybersecurity Awareness Bot";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Play greeting audio while ASCII art is rendering
            SoundOutput.PlayGreetingAsync();

            // Show animated header
            AsciiArt.Display(animate: true);

            // Run the interactive chatbot
            new Conversation().Start();
        }
    }
}
