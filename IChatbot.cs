using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CybersecurityChatbot
{
    public interface IChatbot
    {
        string UserName { get; set; }
        public void StartChat();
    }
}
