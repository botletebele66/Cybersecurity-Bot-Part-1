using System;
using System.IO;
using System.Media;
using System.Threading.Tasks;
// NAudio provides better playback control (volume, buffering) than SoundPlayer
using NAudio.Wave;

namespace CybersecurityBot
{
    public static class SoundOutput
    {
        // Blocking playback (existing behavior)
        public static void PlayGreeting()
        {
            // Find a WAV file to play (tolerant to filename variations)
            string? wavPath = FindGreetingWav();

            if (!string.IsNullOrEmpty(wavPath) && File.Exists(wavPath))
            {
                try
                {
                    using (var player = new SoundPlayer(wavPath))
                    {
                        player.PlaySync();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: failed to play greeting audio: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Warning: greeting audio not found. Place a WAV file named like 'Greeting AI.wav' or 'Greeting AI .wav' under the Resources folder in the project.");
            }
        }

        // Non-blocking playback: start the audio on a background thread so the UI (ASCII art) can display concurrently.
        public static void PlayGreetingAsync()
        {
            // Find a WAV file to play (tolerant to filename variations)
            string? wavPath = FindGreetingWav();
            if (string.IsNullOrEmpty(wavPath) || !File.Exists(wavPath))
            {
                Console.WriteLine("Warning: greeting audio not found. Place a WAV file named like 'Greeting AI.wav' or 'Greeting AI .wav' under the Resources folder in the project.");
                return;
            }

            // Try to use NAudio for higher-quality playback and volume control. Fall back to SoundPlayer.
            try
            {
                var waveOut = new WaveOutEvent();
                var audioFile = new AudioFileReader(wavPath);
                // Apply a moderate default volume (0.0 - 1.0). Adjust as needed.
                audioFile.Volume = 0.85f;
                waveOut.Init(audioFile);
                waveOut.PlaybackStopped += (s, e) =>
                {
                    waveOut.Dispose();
                    audioFile.Dispose();
                };
                waveOut.Play();
            }
            catch (Exception)
            {
                // If NAudio cannot be used for any reason, fall back to SoundPlayer non-blocking play.
                try
                {
                    var player = new SoundPlayer(wavPath);
                    player.LoadAsync();
                    player.Play();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Warning: failed to play greeting audio asynchronously: {ex.Message}");
                }
            }
        }

        // Search for a candidate greeting WAV in Resources or the absolute folder.
        private static string? FindGreetingWav()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string resourcesDir = Path.Combine(baseDir, "Resources");

            // Candidate filenames to check first (exact and with stray space)
            var candidates = new[]
            {
                Path.Combine(resourcesDir, "Greeting AI.wav"),
                Path.Combine(resourcesDir, "Greeting AI .wav"),
                Path.Combine(resourcesDir, "GreetingAI.wav"),
                Path.Combine(resourcesDir, "greeting ai.wav")
            };

            foreach (var c in candidates)
                if (File.Exists(c)) return c;

            // Fallback: search for any wav in Resources whose name contains 'greeting' and 'ai'
            try
            {
                if (Directory.Exists(resourcesDir))
                {
                    var files = Directory.EnumerateFiles(resourcesDir, "*.wav", SearchOption.TopDirectoryOnly);
                    foreach (var f in files)
                    {
                        string name = Path.GetFileName(f).ToLowerInvariant();
                        if (name.Contains("greeting") && name.Contains("ai")) return f;
                    }

                    // If none found, return first WAV present
                    var first = files.FirstOrDefault();
                    if (!string.IsNullOrEmpty(first)) return first;
                }
            }
            catch { }

            // Last resort: check original absolute path variations in user's repo location
            var absoluteCandidates = new[]
            {
                @"C:\Users\botsh\Documents\GitHub\botletebele66\Cybersecurity Bot Part 1\Resources\Greeting AI.wav",
                @"C:\Users\botsh\Documents\GitHub\botletebele66\Cybersecurity Bot Part 1\Resources\Greeting AI .wav"
            };
            foreach (var a in absoluteCandidates)
                if (File.Exists(a)) return a;

            return null;
        }
    }
}
