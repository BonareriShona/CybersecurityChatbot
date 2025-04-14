using System;
using System.Media; // Required for playing .wav files

namespace CybersecurityChatbot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Set up console appearance
                Console.Title = "Cybersecurity Awareness Chatbot";
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("===============================================");
                Console.WriteLine("=== Welcome to the Cybersecurity Awareness Chatbot ===");
                Console.WriteLine("===============================================");
                Console.ResetColor();

                // Display ASCII Art (optional - can be updated later)
                DisplayAsciiArt();

                // Play voice greeting (optional - requires wav file in project folder)
                PlayVoiceGreeting();

                // Ask user for their name
                Console.Write("\nPlease enter your name: ");
                string userName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(userName))
                    userName = "User";

                // Instantiate and start chatbot
                IChatbot chatbot = new CybersecurityChatbot();
                chatbot.UserName = userName;
                chatbot.StartChat();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n[ERROR] An unexpected error occurred: {ex.Message}");
                Console.ResetColor();
            }
        }

        // Optional: ASCII Art Display
        private static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"
   ____            _                      _                 
  / ___|__ _ _ __ | |_ ___ _ __ ___  __ _| |_ ___  _ __ ___ 
 | |   / _` | '_ \| __/ _ \ '__/ _ \/ _` | __/ _ \| '__/ _ \
 | |__| (_| | | | | ||  __/ | |  __/ (_| | || (_) | | |  __/
  \____\__,_|_| |_|\__\___|_|  \___|\__,_|\__\___/|_|  \___|
                                                            
            ");
            Console.ResetColor();
        }

        // Optional: Play voice greeting (add a WAV file in your project root)
        private static void PlayVoiceGreeting()
        {
            try
            {
                string filePath = "C:\\Users\\lab_services_student\\source\\repos\\CybersecurityChatbot\\Audio\\greeting.wav"; // Make sure this file exists in your build directory
                if (System.IO.File.Exists(filePath))
                {
                    using SoundPlayer player = new SoundPlayer(filePath);
                    player.PlaySync(); // or player.Play() if you don't want to block
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("[Warning] Voice greeting file not found. Skipping...");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Voice Error] Could not play greeting: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}
