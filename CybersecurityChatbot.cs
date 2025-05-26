using System;
using System.Collections.Generic;
using System.Threading;

namespace CybersecurityChatbot
{
    public class CybersecurityChatbot : ChatbotBase
    {
        private readonly Random rand = new(); // ✅ Shared Random instance added here

        private readonly Dictionary<string, string> basicResponses = new()
        {
            { "how are you", "I'm great! I'm here to help you stay safe online!" },
            { "what’s your purpose", "I educate users about cybersecurity best practices." },
            { "password safety", "Use strong, unique passwords and enable two-factor authentication." }
        };

        private readonly Dictionary<string, List<string>> keywordResponses = new()
        {
            { "password", new List<string> {
                "Use strong, unique passwords and avoid using personal details.",
                "Enable two-factor authentication (2FA) for important accounts.",
                "Consider using a password manager to store passwords securely." }
            },
            { "phishing", new List<string> {
                "Phishing is a cybercrime where attackers impersonate legitimate institutions to steal sensitive information like passwords or credit card numbers.",
                "Phishing emails often create a sense of urgency — always pause and verify before clicking on links or downloading attachments.",
                "Never enter credentials on suspicious websites — look for HTTPS and double-check URLs." }
            },
            { "privacy", new List<string> {
                "Check app permissions regularly and limit unnecessary access.",
                "Use secure messaging apps with end-to-end encryption.",
                "Avoid oversharing personal info on social media." }
            },
            { "scam", new List<string> {
                "Ignore messages asking for money or personal information.",
                "Report scam messages to relevant authorities or platforms.",
                "Be cautious of online offers that seem too good to be true." }
            },
            { "malware", new List<string> {
                "Malware is malicious software designed to harm or exploit any programmable device.",
                "Keep your software updated and avoid clicking suspicious links to prevent malware infections.",
                "Use antivirus software and scan regularly to detect malware." }
            },
            { "social engineering", new List<string> {
                "Social engineering attacks manipulate people into divulging confidential information.",
                "Be cautious of unsolicited requests for personal information via phone, email, or social media.",
                "Verify the identity of anyone asking for sensitive data." }
            },
            { "safe browsing", new List<string> {
                "Use HTTPS websites whenever possible to secure your data.",
                "Avoid clicking on unknown or suspicious links.",
                "Use browser extensions that block trackers and malicious ads." }
            }
        };

        private string? rememberedTopic = null;
        private int userUnderstandingScore = 0;
        private readonly List<string> conversationHistory = new();

        public override void StartChat()
        {
            ConsoleUI.ShowTypingEffect($"\nHi {UserName}, how can I help you with cybersecurity today? Type 'exit' to quit. Type 'help' for commands.");

            while (true)
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("\n> ");
                    string? inputRaw = Console.ReadLine();
                    string input = inputRaw?.Trim().ToLower() ?? "";

                    Console.ResetColor();

                    if (string.IsNullOrEmpty(input))
                    {
                        Console.WriteLine("Please type something so I can assist you.");
                        continue;
                    }

                    AddToConversationHistory(inputRaw!);

                    if (input == "exit")
                    {
                        Console.WriteLine($"\nStay safe online, {UserName}. Goodbye!");
                        break;
                    }

                    if (input == "help")
                    {
                        DisplayHelp();
                        continue;
                    }

                    if (input == "score" || input.Contains("my score"))
                    {
                        Console.WriteLine($"Your current understanding score is: {userUnderstandingScore}");
                        continue;
                    }

                    if (input == "history")
                    {
                        DisplayConversationHistory();
                        continue;
                    }

                    if (input.StartsWith("remind me to "))
                    {
                        string reminder = inputRaw!.Substring("remind me to ".Length).Trim();
                        if (string.IsNullOrEmpty(reminder))
                        {
                            Console.WriteLine("Please tell me what to remind you about.");
                        }
                        else
                        {
                            AddToConversationHistory($"Reminder set: {reminder}");
                            Console.WriteLine($"Got it! I will remind you to: {reminder}");
                        }
                        continue;
                    }

                    if (DetectSentiment(input, out string sentimentResponse))
                    {
                        Console.WriteLine(sentimentResponse);
                        IncrementScore(1);
                        continue;
                    }

                    if (basicResponses.ContainsKey(input))
                    {
                        Console.WriteLine(basicResponses[input]);
                        IncrementScore(2);
                        rememberedTopic = null;
                        continue;
                    }

                    bool matched = false;
                    foreach (var topic in keywordResponses.Keys)
                    {
                        if (input.Contains(topic) || input.Contains($"tell me about {topic}"))
                        {
                            matched = true;
                            rememberedTopic = topic;
                            Console.WriteLine(GetRandomResponse(keywordResponses[topic]));
                            IncrementScore(2);
                            break;
                        }
                    }

                    if (!matched && input.Contains("more") && rememberedTopic != null)
                    {
                        Console.WriteLine($"Here’s more about {rememberedTopic}:");
                        Console.WriteLine(GetRandomResponse(keywordResponses[rememberedTopic]));
                        IncrementScore(1);
                        continue;
                    }

                    if (!matched)
                    {
                        Console.WriteLine("I didn’t quite understand that. Try asking about password safety, phishing, scams, privacy, malware, social engineering, or safe browsing.");
                        DecrementScore(1);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] Something went wrong. Please try again. Details: {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

        private void AddToConversationHistory(string input)
        {
            conversationHistory.Add(input);
            if (conversationHistory.Count > 10)
            {
                conversationHistory.RemoveAt(0);
            }
        }

        private void DisplayConversationHistory()
        {
            Console.WriteLine("\nConversation history:");
            if (conversationHistory.Count == 0)
            {
                Console.WriteLine("No conversation history available.");
            }
            else
            {
                int count = 1;
                foreach (var line in conversationHistory)
                {
                    Console.WriteLine($"{count++}. {line}");
                }
            }
        }

        private void DisplayHelp()
        {
            Console.WriteLine("\nYou can ask me about cybersecurity topics like password safety, phishing, scams, privacy, malware, social engineering, or safe browsing.");
            Console.WriteLine("Commands you can try:");
            Console.WriteLine("- Type 'score' or 'what's my score' to see your understanding score.");
            Console.WriteLine("- Type 'history' to see recent conversation history.");
            Console.WriteLine("- Type 'remind me to [something]' to set a reminder.");
            Console.WriteLine("- Type 'exit' to quit the chatbot.");
            Console.WriteLine("- Simply type any question or phrase related to cybersecurity.");
        }

        private void IncrementScore(int points)
        {
            userUnderstandingScore += points;
        }

        private void DecrementScore(int points)
        {
            userUnderstandingScore -= points;
            if (userUnderstandingScore < 0)
                userUnderstandingScore = 0;
        }

        private string GetRandomResponse(List<string> responses)
        {
            return responses[rand.Next(responses.Count)]; // ✅ Uses the shared Random instance
        }

        private bool DetectSentiment(string input, out string response)
        {
            if (input.Contains("worried"))
            {
                response = "It's okay to feel worried. Scams can be scary, but I'm here to help you stay safe.";
                return true;
            }
            if (input.Contains("frustrated") || input.Contains("i'm frustrated"))
            {
                response = "I understand how frustrating this can be. Cybersecurity can seem complex, but you're not alone — you're taking the right steps just by asking!";
                return true;
            }
            if (input.Contains("curious"))
            {
                response = "Curiosity is the first step toward safety. What would you like to know more about?";
                return true;
            }

            response = "";
            return false;
        }
    }
}
