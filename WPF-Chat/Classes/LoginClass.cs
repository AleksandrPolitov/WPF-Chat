using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Chat.Classes
{
    public class LoginClass
    {
        public enum REGISTER_RES
        {
            SUCCESS = 0,
            ALREADY_EXISTS = 1,
            //.. maybe in future
        };

        public enum LOGIN_RES
        {
            SUCCESS = 0,
            FAIL = 1,
            //.. maybe in future
        };

        WPFChatEntities database;

        public int user_id;

        public LoginClass() { database = new WPFChatEntities(); }

        public LOGIN_RES Login(string email_username, string password) {
            var user = database.users.Where(u=> u.email == email_username || u.username == email_username).FirstOrDefault();
            
            if (user == null)
                return LOGIN_RES.FAIL;


            if (PasswordHash.ValidatePassword(password, user.password))
            {
                user_id = user.id;

                return LOGIN_RES.SUCCESS;
            }

            return LOGIN_RES.FAIL;
        }

        public REGISTER_RES Register(string email, string username, string password)
        {
            if (!database.users.Any(u => u.username == username || u.email == email))
            {
                var new_user = new users();
                new_user.email = email;
                new_user.username = username;
                new_user.password = PasswordHash.HashPassword(password);
                new_user.created_at = DateTime.Now;
                database.users.Add(new_user);
                database.SaveChanges();

                return REGISTER_RES.SUCCESS;
            }
            else 
                return REGISTER_RES.ALREADY_EXISTS;
        }
    }
}
