using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;
using WPF_Chat.Classes;

namespace WPF_Chat.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginClass lc;
        ConversationsClass cc;

        public MainWindow(LoginClass lc)
        {
            InitializeComponent();
            this.lc = lc;

            var currenctUser = this.lc.getCurrentUser();
            cc = new ConversationsClass(currenctUser);

            this.SizeChanged += Window_SizeChanged;

            MessageBox.Show(currenctUser.username + currenctUser.email);
            //updateConversations();
        }


        private void resetConversations() {
            spConversations.Children.Clear();

            var btnStart = new Button();
            btnStart.Content = "Start new conversation";
            btnStart.Height = 40;
            btnStart.Click += this.btnStart_Click;
            spConversations.Children.Add(btnStart);
        }

        private void updateConversations()
        {
            //this.Measure();

            this.Dispatcher.Invoke(new Action(() =>
            {
                cc.updateAllConversations();
                resetConversations();

                BrushConverter bc = new BrushConverter();

                foreach (var conv in cc.allConversations)
                {

                    var spConversation = new StackPanel();
                    spConversation.Height = 80;
                    spConversation.Name = conv.recipient.username;
                    spConversation.MouseEnter += this.spConversation_MouseEnter;
                    spConversation.MouseLeave += this.spConversation_MouseLeave;
                    spConversation.MouseLeftButtonDown += this.spConversation_MouseLeftButtonDown;
                    spConversation.Orientation = Orientation.Horizontal;
                    spConversations.Children.Add(spConversation);

                    var brdLogo = new Border();
                    brdLogo.Height = 64;
                    brdLogo.Width = 64;
                    brdLogo.CornerRadius = new CornerRadius(40);
                    brdLogo.BorderBrush = Brushes.Black;
                    brdLogo.BorderThickness = new Thickness(1);
                    brdLogo.Background = (Brush)bc.ConvertFrom(GetColor(conv.recipient.username));
                    brdLogo.HorizontalAlignment = HorizontalAlignment.Left;
                    brdLogo.VerticalAlignment = VerticalAlignment.Center;
                    brdLogo.Margin = new Thickness(8, 0, 0, 0);
                    spConversation.Children.Add(brdLogo);

                    var lblLogo = new Label();
                    lblLogo.Content = Char.ToUpper(conv.recipient.username[0]);
                    lblLogo.FontSize = 20;
                    lblLogo.VerticalAlignment = VerticalAlignment.Center;
                    lblLogo.HorizontalAlignment = HorizontalAlignment.Center;
                    brdLogo.Child = lblLogo;

                    var spConversationInfo = new StackPanel();
                    spConversationInfo.Height = 80;
                    spConversationInfo.Width = 220;
                    spConversationInfo.Margin = new Thickness(0, 0, 0, 0);
                    spConversationInfo.HorizontalAlignment = HorizontalAlignment.Stretch;
                    spConversationInfo.VerticalAlignment = VerticalAlignment.Stretch;
                    //spConversationInfo.Background = Brushes.Red;
                    spConversation.Children.Add(spConversationInfo);


                    var lblUsername = new Label();
                    lblUsername.Content = "@" + conv.recipient.username;
                    lblUsername.FontSize = 18;
                    lblUsername.Margin = new Thickness(0, 8, 0, 0);
                    lblUsername.HorizontalAlignment = HorizontalAlignment.Left;
                    lblUsername.VerticalAlignment = VerticalAlignment.Center;
                    spConversationInfo.Children.Add(lblUsername);


                    var lastMsg = conv.messages.FirstOrDefault();
                    string lblLastMessageText = (lastMsg.user_id == lc.user_id ? "You: " : conv.recipient.username + ": ") + lastMsg.content;
                    var lblLastMessage = new Label();
                    lblLastMessage.Content = lblLastMessageText.Length > 24 ? lblLastMessageText.Substring(0,24)+".." : lblLastMessageText;
                    lblLastMessage.FontSize = 14;
                    lblLastMessage.Margin = new Thickness(0, 0, 0, 0);
                    lblLastMessage.HorizontalAlignment = HorizontalAlignment.Left;
                    lblLastMessage.VerticalAlignment = VerticalAlignment.Center;
                    spConversationInfo.Children.Add(lblLastMessage);


                    var lblLastMsgDate = new Label();
                    if (lastMsg.created_at.Day != DateTime.Now.Day)
                        lblLastMsgDate.Content = lastMsg.created_at.Day + "." + lastMsg.created_at.Month;
                    else
                        lblLastMsgDate.Content = lastMsg.created_at.Hour + ":" + lastMsg.created_at.Minute;
                    lblLastMsgDate.Margin = new Thickness(150, -58, 0, 0);
                    spConversationInfo.Children.Add(lblLastMsgDate);

                }
            }), DispatcherPriority.Loaded);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var cnc = new CreateNewConversationWindow(this.cc);
            cnc.Show();
        }

        private void Canvas_MouseEnter(object sender, MouseEventArgs e)
        {
            var element = sender as Canvas;
            BrushConverter bc = new BrushConverter();
            element.Background = (Brush)bc.ConvertFrom("#FFE6E6E6"); 
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            var element = sender as Canvas;
            element.Background = null;
        }



        private string GetColor(string raw)
        {
            using (MD5 md5Hash = MD5.Create())
            {
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(raw));
                return "#" + BitConverter.ToString(data).Replace("-", string.Empty).Substring(0, 6);
            }
        }



        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateConversations();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            var cnc = new CreateNewConversationWindow(this.cc);
            cnc.Closing += CreateNewConversationWindow_Closing;
            cnc.Show();
        }

        private void spConversation_MouseEnter(object sender, MouseEventArgs e)
        {
            var element = sender as StackPanel;
            BrushConverter bc = new BrushConverter();
            element.Background = (Brush)bc.ConvertFrom("#FFE6E6E6");
        }

        private void spConversation_MouseLeave(object sender, MouseEventArgs e)
        {
            var element = sender as StackPanel;
            element.Background = null;
        }

        private void spConversation_MouseLeftButtonDown(object sender, MouseEventArgs e)
        {
            var element = sender as StackPanel;
            MessageBox.Show(element.Name);
        }



        private void CreateNewConversationWindow_Closing(object sender, CancelEventArgs e)
        {
            MessageBox.Show("12521");
            updateConversations();
        }
    }
}

