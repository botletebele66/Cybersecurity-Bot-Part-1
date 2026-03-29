using System;
using System.Threading;

namespace CybersecurityBot
{
    public class Conversation
    {
        private Bot _bot;

        public Conversation()
        {
            _bot = new Bot();
        }

        public void Start()
        {
            // Borders and colours
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('=', 70));
            Console.ResetColor();

            _bot.GreetUser();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(new string('-', 70));
            Console.ResetColor();

            // Main conversation loop
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("You: ");
                Console.ResetColor();
                string userInput = Console.ReadLine();

                // Check for exit commands
                if (userInput != null && (userInput.ToLower() == "exit" || userInput.ToLower() == "quit"))
                {
                    TypeWriter("Goodbye! Stay safe online.", 30);
                    break;
                }

                // Get response from bot
                string response = _bot.GetResponse(userInput);

                // Display with typing effect
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Bot: ");
                Console.ResetColor();
                TypeWriter(response, 40);

                // Add a separator
                Console.WriteLine(new string('-', 70));
            }
        }

        // Typing effect helper
        private void TypeWriter(string message, int delay = 50)
        {
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(delay);
            }
            Console.WriteLine();
        }
    }
}
