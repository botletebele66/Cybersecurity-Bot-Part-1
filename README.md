# Cybersecurity Bot Part 1 – CyBrog

A C# console chatbot that educates South African citizens about cybersecurity threats (phishing, passwords, 2FA, malware, etc.) with voice greeting, ASCII art, and interactive scenarios.

## Features
- 🎤 Voice greeting on startup (WAV)
- 🖼️ Animated ASCII art logo
- 🧑‍💻 Personalised conversation (asks for your name)
- 📚 8 cybersecurity topics with detailed prevention advice
- 🧠 Real‑life scenario quizzes
- 💡 Random facts & tips
- 🎨 Coloured console UI with borders and typing simulation
- ❤️ Empathetic error handling

## Requirements
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) (or .NET 10 preview if you keep net10.0)
- Windows (for `System.Media.SoundPlayer`; Linux/macOS may need alternative)

## How to Run
1. Clone the repository  
   `git clone https://github.com/botletebele66/Cybersecurity-Bot-Part-1.git`
2. Navigate into the folder  
   `cd Cybersecurity-Bot-Part-1`
3. Build and run  
   `dotnet run`

Make sure the audio file exists at `Resources/Greeting AI.wav`.

## CI/CD
This project uses GitHub Actions to automatically build on every push.  
✅ **Latest build status:**  
![.NET Build & Test](https://github.com/botletebele66/Cybersecurity-Bot-Part-1/actions/workflows/dotnet-build.yml/badge.svg)

## Screenshot of Successful CI Run
![CI success](screenshot-ci.png)

## Folder Structure

├── .github/workflows/dotnet-build.yml
├── Resources/
│ └── Greeting AI.wav
├── AsciiArt.cs
├── Bot.cs
├── Conversation.cs
├── SoundOutput.cs
├── Program.cs
├── Cybersecurity Bot Part 1.csproj
└── README.md
