using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public class CybersecurityChatbot : ChatbotBase
    {
        private readonly Dictionary<string, string> responses = new()
        {
            { "how are you", "I'm great! I'm here to help you stay safe online!" },
            { "what’s your purpose", "I educate users about cybersecurity best practices." },
            { "password safety", "Use strong, unique passwords and enable two-factor authentication." },
            { "phishing", "Avoid clicking on suspicious links or opening unexpected attachments." },
            { "safe browsing", "Use HTTPS websites and avoid using public Wi-Fi for banking." }
        };

        public override void StartChat()
        {
            ConsoleUI.ShowTypingEffect($"\nHi {UserName}, how can I help you with cybersecurity today? Type 'exit' to quit.");

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\n> ");
                string? input = Console.ReadLine()?.ToLower();
                Console.ResetColor();

                if (input == "exit")
                {
                    Console.WriteLine("\nStay safe online. Goodbye!");
                    break;
                }

                try
                {
                    if (string.IsNullOrWhiteSpace(input))
                        throw new ArgumentException("Input cannot be empty.");

                    if (responses.ContainsKey(input))
                    {
                        Console.WriteLine(responses[input]);
                    }
                    else
                    {
                        Console.WriteLine("I didn’t quite understand that. Try asking about password safety, phishing, or safe browsing.");
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] {ex.Message}");
                    Console.ResetColor();
                }
            }
        }
    }
}

