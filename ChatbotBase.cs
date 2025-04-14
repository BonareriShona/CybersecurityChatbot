using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public abstract class ChatbotBase : IChatbot
    {
         public string UserName { get;  set; }
        

        public void SetUserName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            UserName = name;
        }

        public abstract void StartChat();
    }
}
