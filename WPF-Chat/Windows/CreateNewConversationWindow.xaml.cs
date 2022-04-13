using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WPF_Chat.Classes;

namespace WPF_Chat.Windows
{
    /// <summary>
    /// Interaction logic for CreateNewConversation.xaml
    /// </summary>
    public partial class CreateNewConversationWindow : Window
    {
        ConversationsClass cc;
        public CreateNewConversationWindow(ConversationsClass cc)
        {
            InitializeComponent();
            this.cc = cc;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string errStr = "";

            string username = txtUsername.Text,
                   message = txtMessage.Text;

            if (username == null)
                errStr += "Username is empty.\n";

            if (message == null || message.Length > 200)
                errStr += "Message is empty or too big(200 chars max).\n";

            if(errStr != "")
            {
                MessageBox.Show(errStr);
                return;
            }

            switch(this.cc.createNewConversation(username, message))
            {
                case ConversationsClass.CREATE_RES.SUCCESS:
                    MessageBox.Show("Conversation started.");
                    this.Close();
                    // UPDATE ALL CONVERSATIONS
                    break;
                case ConversationsClass.CREATE_RES.YOUR_USER:
                    MessageBox.Show("You cannot start conversation with yourself.");
                    break;
                case ConversationsClass.CREATE_RES.ALREADY_EXISTS:
                    MessageBox.Show("You already started conversation with @" + username);
                    break;
                case ConversationsClass.CREATE_RES.USER_NOT_EXISTS:
                    MessageBox.Show("User @" + username + " does not exists.");
                    break;
                case ConversationsClass.CREATE_RES.MESSAGE_FAIL:
                    MessageBox.Show("Message you entered is not valid (1-200 chars).");
                    break;
                default:
                    break;
            }
        }
    }
}
