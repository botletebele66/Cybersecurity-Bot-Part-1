using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace CybersecurityBot
{
    public class Conversation
    {
        private readonly Bot _bot = new();
        private string _userName = string.Empty;
        private string? _lastTopic;

        public void Start()
        {
            GetUserName();
            WriteBot($"\n*** Welcome, {_userName}! ***");
            WriteBot("I'm CyBrog – your friendly cyber guardian. 💙");
            WriteBot("Type /help anytime to see what I can do.\n");
            MainLoop();
        }

        private void GetUserName()
        {
            while (true)
            {
                Console.Write("Hi there! What should I call you? ");
                string input = Console.ReadLine()?.Trim() ?? "";
                if (string.IsNullOrWhiteSpace(input))
                {
                    WriteError("I didn't catch that – please tell me your name.");
                    continue;
                }

                // Remove common lead‑ins
                string lower = input.ToLower();
                if (lower.StartsWith("my name is ")) input = input[11..];
                else if (lower.StartsWith("i am ")) input = input[5..];
                else if (lower.StartsWith("im ")) input = input[3..];

                var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 0) { WriteError("Please enter a valid name."); continue; }

                for (int i = 0; i < parts.Length; i++)
                    parts[i] = char.ToUpper(parts[i][0]) + parts[i][1..].ToLower();

                _userName = string.Join(" ", parts);
                if (!string.IsNullOrWhiteSpace(_userName)) break;
                WriteError("That doesn't look like a name – please try again.");
            }
        }

        private void MainLoop()
        {
            while (true)
            {
                ShowMenu();
                string input = GetInput($"{_userName} > ");

                if (input.StartsWith("/"))
                {
                    if (!HandleCommand(input)) break;
                    continue;
                }

                ProcessInput(input);
                if (!AskToContinue()) break;
            }
            Farewell();
        }

        private void ShowMenu()
        {
            WriteBot("═══════════════════════════════\n      Cybersecurity Topics     \n═══════════════════════════════");
            var topics = _bot.Topics;
            for (int i = 0; i < topics.Count; i++)
                Console.WriteLine($"  {i + 1}. {topics[i].Name}");

            WriteBot("\nEnter a topic number or name, or try:");
            WriteBot("  scenario  •  fact  •  tip  •  more  •  /help  •  /exit\n");
        }

        private string GetInput(string prompt)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input)) return input;
                WriteError($"Please type something, {_userName} – I'm here to help 🙂");
            }
        }

        private int GetNumberChoice(int min, int max)
        {
            while (true)
            {
                string input = GetInput($"Your choice ({min}–{max}): ");
                if (int.TryParse(input, out int val) && val >= min && val <= max) return val;
                WriteError($"Please enter a number between {min} and {max}.");
            }
        }

        private static void WriteBot(string msg) { Console.ForegroundColor = ConsoleColor.Cyan; Console.WriteLine(msg); Console.ResetColor(); }
        private static void WriteSuccess(string msg) { Console.ForegroundColor = ConsoleColor.Green; Console.WriteLine(msg); Console.ResetColor(); }
        private static void WriteError(string msg) { Console.ForegroundColor = ConsoleColor.Red; Console.WriteLine(msg); Console.ResetColor(); }
        private static void WriteRef(string msg) { Console.ForegroundColor = ConsoleColor.Magenta; Console.WriteLine(msg); Console.ResetColor(); }

        private static void SimulateTyping()
        {
            Console.Write("CyBrog is thinking");
            for (int i = 0; i < 3; i++) { Thread.Sleep(400); Console.Write("."); }
            Console.WriteLine();
        }

        private void ShowTopic(Topic t)
        {
            SimulateTyping();
            WriteBot(_bot.BuildTopicResponse(t));
            WriteSuccess($"\n💡 {_bot.GetEncouragement()}");
            _lastTopic = t.Name;
        }

        private void RunScenario()
        {
            var s = _bot.GetRandomScenario();
            WriteBot($"\n⚠️  SCENARIO: {s.Description}\n");
            for (int i = 0; i < s.Options.Count; i++)
                Console.WriteLine($"  {i + 1}. {s.Options[i]}");
            Console.WriteLine();

            int choice = GetNumberChoice(1, s.Options.Count);
            if (choice - 1 == s.CorrectOptionIndex)
                WriteSuccess($"✅ Correct! {s.Feedback}");
            else
                WriteError($"❌ Not quite. {s.Feedback}");

            WriteSuccess(_bot.GetEncouragement());
        }

        private void ProcessInput(string input)
        {
            if (!TryProcessInput(input))
            {
                WriteError($"Hmm, I didn't quite get that, {_userName}. 🤔");
                WriteBot("Try a topic number, a name like 'phishing', or one of: scenario, fact, tip.");
                WriteBot("Type /help to see all options.\n");
            }
        }

        private bool TryProcessInput(string input)
        {
            SimulateTyping();
            string lower = input.ToLower();

            // Empathy & conversation
            if (lower.Contains("thank")) { WriteSuccess($"You're so welcome, {_userName}! 😊 That makes my day."); return true; }
            if (lower.Contains("sorry") || lower.Contains("stupid") || lower.Contains("dumb")) { WriteBot(_bot.GetComfortMessage()); return true; }
            if (lower.Contains("stuck") || lower.Contains("confused") || lower.Contains("help me")) { WriteBot($"{_bot.GetComfortMessage()} Try a topic number or name, or type 'scenario', 'fact', or 'tip'."); return true; }
            if (lower.Contains("how are you")) { WriteBot($"I'm wonderful now that you're here learning, {_userName}! 😄"); return true; }
            if (lower.Contains("purpose") || lower.Contains("what can you do")) { WriteBot("I cover: passwords, phishing, 2FA, malware, social engineering, updates, public Wi-Fi, and more. Pick a number or name from the menu!"); return true; }

            // Special keywords
            if (lower is "scenario" or "quiz" or "test") { RunScenario(); return true; }
            if (lower is "fact" or "did you know") { var f = _bot.GetRandomFact(); WriteRef($"📊 DID YOU KNOW?\n{f.Fact} {f.Citation}"); return true; }
            if (lower is "tip" or "random tip") { WriteBot($"✨ Random tip: {_bot.GetRandomTip()}"); return true; }

            if (lower == "more")
            {
                if (_lastTopic is null) WriteError("No previous topic to revisit – pick one from the menu first.");
                else
                {
                    var t = _bot.FindTopic(_lastTopic);
                    if (t != null) ShowTopic(t);
                    else WriteError("I can't recall the last topic – please pick one from the menu.");
                }
                return true;
            }

            // Numeric topic
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= _bot.Topics.Count)
            {
                ShowTopic(_bot.Topics[choice - 1]);
                return true;
            }

            // Name / alias topic
            var matched = FindTopicByName(input);
            if (matched != null) { ShowTopic(matched); return true; }

            return false;
        }

        private Topic? FindTopicByName(string input)
        {
            string lower = input.ToLowerInvariant();
            var exact = _bot.Topics.FirstOrDefault(t => t.Name.ToLowerInvariant() == lower);
            if (exact != null) return exact;

            var aliases = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["password"] = "Passwords",
                ["passwords"] = "Passwords",
                ["phish"] = "Phishing",
                ["phishing"] = "Phishing",
                ["2fa"] = "2FA & MFA",
                ["mfa"] = "2FA & MFA",
                ["two factor"] = "2FA & MFA",
                ["malware"] = "Malware",
                ["virus"] = "Malware",
                ["social engineering"] = "Social Engineering",
                ["updates"] = "Updates & Patches",
                ["patch"] = "Updates & Patches",
                ["wifi"] = "Public Wi-Fi",
                ["public wifi"] = "Public Wi-Fi"
            };

            return aliases.TryGetValue(lower, out string? name)
                ? _bot.Topics.FirstOrDefault(t => t.Name == name)
                : null;
        }

        private bool HandleCommand(string cmd)
        {
            switch (cmd.ToLower())
            {
                case "/exit": return false;
                case "/help":
                    WriteBot("\n📚 HELP");
                    WriteBot("  • Enter a topic number or name (e.g. 'passwords', '3')");
                    WriteBot("  • scenario – test yourself with a real-world situation");
                    WriteBot("  • fact     – get a random cybersecurity fact");
                    WriteBot("  • tip      – get a quick security tip");
                    WriteBot("  • more     – revisit the last topic");
                    WriteBot("  • /help    – show this menu");
                    WriteBot("  • /exit    – quit CyBrog\n");
                    return true;
                default:
                    WriteError($"Unknown command '{cmd}'. Type /help to see available commands.");
                    return true;
            }
        }

        private bool AskToContinue()
        {
            while (true)
            {
                Console.Write($"\nKeep learning, {_userName}? (yes / no): ");
                string ans = Console.ReadLine()?.Trim().ToLower() ?? "";
                if (ans is "yes" or "y") return true;
                if (ans is "no" or "n")
                {
                    WriteBot("\nPress Enter to close CyBrog.");
                    Console.ReadLine();
                    return false;
                }
                WriteError("Please answer yes or no.");
            }
        }

        private void Farewell()
        {
            Console.WriteLine();
            WriteSuccess("╔══════════════════════════════════════╗");
            WriteSuccess($"  Thanks for learning with me, {_userName}!");
            WriteSuccess("  Stay alert, stay safe. 🛡️");
            WriteSuccess("  CyBrog will always be here for you. 💙");
            WriteSuccess("╚══════════════════════════════════════╝");
            Console.WriteLine();
        }
    }
}