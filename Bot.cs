using System;
using System.Collections.Generic;

namespace CybersecurityBot
{
    public class Bot
    {
        public string UserName { get; private set; }

        public Bot()
        {
            UserName = string.Empty;
        }

        public void GreetUser()
        {
            Console.Write("What is your name? ");
            string input = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(input))
            {
                Console.Write("I didn't catch that. Please tell me your name: ");
                input = Console.ReadLine();
            }

            UserName = input.Trim();
            Console.WriteLine($"Nice to meet you, {UserName}! I'm your Cybersecurity Assistant.");
            Console.WriteLine("You can ask me about passwords, phishing, safe browsing, or just say hello.");
        }

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
                return "I didn't catch that. Could you please say something?";

            string input = userInput.ToLower();

            // Exit commands are handled in the main loop, not here
            if (input.Contains("help"))
            {
                return "You can ask me about:\n- passwords\n- phishing\n- safe browsing\nOr just say 'hello'!";
            }
            if (input.Contains("hello") || input.Contains("hi"))
            {
                return $"Hello, {UserName}! How can I help you stay safe online today?";
            }
            if (input.Contains("how are you"))
            {
                return "I'm a bot, so I'm always running smoothly! How can I help you stay safe online?";
            }
            if (input.Contains("purpose") || input.Contains("what's your purpose"))
            {
                return "My purpose is to help you recognise and avoid cyber threats like phishing, weak passwords, and unsafe websites.";
            }
            if (input.Contains("password"))
            {
                return "Use strong, unique passwords for each account. Consider a password manager and enable two‑factor authentication.";
            }
            if (input.Contains("phishing"))
            {
                return "Phishing emails try to trick you into clicking malicious links. Always check the sender's address and never share personal information via email.";
            }
            if (input.Contains("safe browsing") || input.Contains("browsing"))
            {
                return "Look for 'https://' and a padlock icon in the address bar. Avoid clicking on pop‑ups and be cautious with downloads.";
            }

            // Default response for unknown queries
            return "I'm not sure I understand. Could you rephrase? Try asking about passwords, phishing, or safe browsing.";
        }
    }
}