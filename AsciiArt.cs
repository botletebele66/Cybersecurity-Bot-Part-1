using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace CybersecurityBot
{
    public static class AsciiArt
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int n);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr h, out int mode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr h, int mode);

        private static void EnableAnsi()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return;
            IntPtr h = GetStdHandle(-11);
            if (GetConsoleMode(h, out int mode))
                SetConsoleMode(h, mode | 0x0004);
        }

        private static readonly List<string> Content = new()
        {
            "    ████████╗██╗  ██╗███████╗     ██████╗██╗   ██╗██████╗  ██████╗ ██████╗  ██████╗ ",
            "    ╚══██╔══╝██║  ██║██╔════╝    ██╔════╝╚██╗ ██╔╝██╔══██╗██╔═══██╗██╔══██╗██╔════╝ ",
            "       ██║   ███████║█████╗      ██║      ╚████╔╝ ██████╔╝██║   ██║██████╔╝██║  ███╗ ",
            "       ██║   ██╔══██║██╔══╝      ██║       ╚██╔╝  ██╔══██╗██║   ██║██╔══██╗██║   ██║ ",
            "       ██║   ██║  ██║███████╗    ╚██████╗   ██║   ██████╔╝╚██████╔╝██║  ██║╚██████╔╝ ",
            "       ╚═╝   ╚═╝  ╚═╝╚══════╝    ╚═════╝   ╚═╝   ╚═════╝  ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ",
            "",
            "                     🛡️  CYBERSECURITY AWARENESS BOT  🛡️                  ",
            "                   Your Digital Guardian Against Threats                  ",
            "",
            "            Powered by CyBorg AI  |  South African Cybersecurity Initiative"
        };

        public static void Display(bool animate = true) => DisplayAsync(animate).GetAwaiter().GetResult();

        public static async Task DisplayAsync(bool animate = true, double totalSeconds = 2.5)
        {
            EnableAnsi();
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int width = Content.Max(l => l.Length) + 6;
            var lines = new List<string>();
            lines.Add($"╔{new string('═', width - 2)}╗");
            foreach (string line in Content)
                lines.Add($"║  {line.PadRight(width - 4)}  ║");
            lines.Add($"╚{new string('═', width - 2)}╝");
            lines.Add(new string('─', width));

            int screenWidth = Console.WindowWidth;
            int verticalPadding = Math.Max(0, (Console.WindowHeight - lines.Count) / 2);
            for (int i = 0; i < verticalPadding; i++)
                Console.WriteLine();

            if (!animate)
            {
                for (int i = 0; i < lines.Count; i++)
                    PrintLine(lines[i], i, lines.Count);
                return;
            }

            int delay = Math.Max(10, (int)(totalSeconds * 1000 / lines.Count));
            int barWidth = Math.Min(60, screenWidth - 10);
            int top = Console.CursorTop;

            // Reserve space
            for (int i = 0; i < lines.Count + 2; i++)
                Console.WriteLine();

            for (int i = 0; i < lines.Count; i++)
            {
                Console.SetCursorPosition(0, top + i);
                int padding = Math.Max(0, (screenWidth - lines[i].Length) / 2);
                Console.Write(new string(' ', padding));
                PrintLine(lines[i], i, lines.Count);

                int filled = (int)((float)(i + 1) / lines.Count * barWidth);
                string progress = $" ⚡ LOADING [{new string('█', filled)}{new string('░', barWidth - filled)}] {(i + 1) * 100 / lines.Count}%";
                Console.SetCursorPosition(0, top + lines.Count);
                int progressPadding = Math.Max(0, (screenWidth - progress.Length) / 2);
                Console.Write(new string(' ', progressPadding) + progress);
                await Task.Delay(delay);
            }

            await Task.Delay(500);
            Console.SetCursorPosition(0, top + lines.Count);
            Console.Write(new string(' ', screenWidth));
            Console.SetCursorPosition(0, top + lines.Count + 1);
            Console.Write(new string(' ', screenWidth));
            Console.SetCursorPosition(0, top + lines.Count);
        }

        private static void PrintLine(string line, int index, int total)
        {
            Console.ResetColor();
            if (line.StartsWith("╔") || line.StartsWith("╚") || line.Contains("═╗") || line.Contains("═╝"))
                Console.Write($"\x1b[1;33m{line}\x1b[22m");
            else if (line.Contains("║") && (line.Contains("█") || line.Contains("╔")))
                WriteGradient(line);
            else if (line.Contains("🛡️"))
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(line);
            }
            else if (line.Contains("Powered by"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(line);
            }
            else if (line.Contains("─"))
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write(line);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(line);
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void WriteGradient(string line)
        {
            if (Console.IsOutputRedirected)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write(line);
                return;
            }

            for (int i = 0; i < line.Length; i++)
            {
                float ratio = (float)i / line.Length;
                int r = (int)(ratio * 255);
                int b = (int)((1 - ratio) * 255);
                Console.Write($"\x1b[38;2;{r};255;{b}m{line[i]}");
            }
            Console.Write("\x1b[0m");
        }
    }
}
