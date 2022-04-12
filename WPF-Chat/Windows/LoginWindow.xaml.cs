using System;
using System.Collections.Generic;
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
using WPF_Chat.Classes;

namespace WPF_Chat.Windows
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            var registerWND = new RegisterWindow();
            registerWND.Show();
            this.Close();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string errStr = "";

            string username = txtEmailOrUser.Text,
                   password = txtEmailOrUser.Text;

            if (username.Length <= 0)
                errStr += "Username is not valid\n";

            if (password.Length < 4 || password.Length > 20)
                errStr += "Password is not valid(4-20 chars)\n";

            if(errStr != "")
            {
                MessageBox.Show(errStr);
                return;
            }

            var lc = new LoginClass();

            switch (lc.Login(username, password))
            {
                case LoginClass.LOGIN_RES.SUCCESS:
                    MessageBox.Show("Login successfull, your user_id is: " + lc.user_id);
                    break;
                case LoginClass.LOGIN_RES.FAIL:
                    MessageBox.Show("Login failed, most likely you entered username or password wrong.");
                    break;
                default:
                    break;
            }

        }
    }
}
