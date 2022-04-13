using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WPF_Chat.Classes
{
    public class ConversationsClass
    {
        public enum CREATE_RES
        {
            SUCCESS = 0,
            YOUR_USER = 1,
            ALREADY_EXISTS = 2,
            USER_NOT_EXISTS = 3,
            MESSAGE_FAIL = 4,
            //.. maybe in future
        };

        WPFChatEntities database;

        private users me;
        public List<ConversationClass> allConversations;
        public ConversationClass selectedConversation;

        public ConversationsClass(users me) {
            this.me = me;
            database = new WPFChatEntities();
            allConversations = new List<ConversationClass>();
            updateAllConversations();
        }

        public CREATE_RES createNewConversation(string email_username, string message)
        {
            if (message == null || message.Length > 200)
                return CREATE_RES.MESSAGE_FAIL;

            var foundUser = database.users.Where(u => u.email == email_username || u.username == email_username).FirstOrDefault();

            if (foundUser == null)
                return CREATE_RES.USER_NOT_EXISTS;
            
            if (foundUser.id == this.me.id)
                return CREATE_RES.YOUR_USER;

            if (database.conversations.Any(c =>
                 (c.first_participant == foundUser.id && c.second_participant == this.me.id) ||
                 (c.first_participant == this.me.id && c.second_participant == foundUser.id)))
            {
                return CREATE_RES.ALREADY_EXISTS;
            }

            var new_conversation = new conversations();
            new_conversation.first_participant = this.me.id;
            new_conversation.second_participant = foundUser.id;
            new_conversation.last_message_id = null;
            database.conversations.Add(new_conversation);
            database.SaveChanges();

            var first_message = new messages();
            first_message.conversation_id = new_conversation.id;
            first_message.content = message;
            first_message.user_id = this.me.id;
            first_message.created_at = DateTime.Now;
            database.messages.Add(first_message);
            database.SaveChanges();

            new_conversation.last_message_id = first_message.id;
            database.SaveChanges();

            return CREATE_RES.SUCCESS;

        }

        

        public void updateAllConversations()
        {
            allConversations.Clear();
            foreach (var conv in database.conversations.AsNoTracking().Where(c=> c.first_participant == this.me.id || c.second_participant == this.me.id).ToList())
            {
                allConversations.Add(new ConversationClass(ref database, conv, me.id));
            }
            //MessageBox.Show("Total conversations: " + allConversations.Count);
        }
    }
}
