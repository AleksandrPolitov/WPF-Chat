using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        WPFChatEntities database;

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            var loginWND = new LoginWindow();
            loginWND.Show();
            this.Close();
        }

        private void ShowErrors(string err) {
            MessageBox.Show(err);
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            string errStr = "";

            string email = txtEmail.Text,
                   username = txtUsername.Text,
                   password = txtPassword.Text,
                   re_password = txtPasswordConfirm.Text;

            if (!new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Match(email).Success)
                errStr += "Email is not valid\n";

            if (username.Length < 4 || username.Length > 20)
                errStr += "Username us not valid(4-20 chars)\n";

            if (password.Length <4 || password.Length > 20)
                errStr += "Password is not valid(4-20 chars)\n";

            if ((re_password.Length <4 || re_password.Length > 20) || password != re_password)
                errStr += "Passwords are not the same\n";

            if (errStr != "") {
                ShowErrors(errStr);
                return;
            }

            var lc = new LoginClass();

            //var res = lc.Register(email, username, password);

            switch (lc.Register(email, username, password))
            {
                case LoginClass.REGISTER_RES.SUCCESS:
                    MessageBox.Show("Account created! You can now login with your username.");
                    var loginWND = new LoginWindow();
                    loginWND.Show();
                    this.Close();
                    break;
                case LoginClass.REGISTER_RES.ALREADY_EXISTS:
                    MessageBox.Show("Account with this email or username already exists.");
                    break;
                default:
                    break;
            }
        }
    }
}
