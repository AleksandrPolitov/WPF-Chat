using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Chat.Classes
{
    public class ConversationClass
    {
        public users recipient;
        public List<messages> messages;
        WPFChatEntities database;

        public ConversationClass(ref WPFChatEntities database, conversations conv, int user_id) {
            this.database = database;

            if (conv.first_participant != user_id)
                recipient = database.users.Where(u=>u.id == conv.first_participant).FirstOrDefault();
            else
                recipient = database.users.Where(u => u.id == conv.second_participant).FirstOrDefault();

            
            messages = database.messages.Where(m => m.conversation_id == conv.id).OrderByDescending(m=>m.created_at).ToList();
        }
    }
}
