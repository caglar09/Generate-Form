using AutoGenerateForm.Core.Repository;
using AutoGenerateForm.DATA.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace AutoGenerateForm.Core.UserOperation
{
   public class UserService
    {
        private string _userName;
        private string _password;

        private IRepository<User> _repo;
        public UserService()
        {
            AutoGenerateFormEntitiy context = new AutoGenerateFormEntitiy();
            _repo = new Repository<User>(context);
        }

        public void Login(string userName, string password)
        {
            var user = _repo.GetAll().FirstOrDefault(x => x.username == userName && x.password == password);
            if (user==null)
                throw new Exception("Kullanıcı Bulunamadı!");
            Authorize(user.username,user.id);
        }

        public void LogOut()
        {
            FormsAuthentication.SignOut();
            HttpContext.Current.Session["user"] = null;
        }
        public bool IsLogin()
        {
            return HttpContext.Current.User.Identity.IsAuthenticated;
        }
        public bool checkUserName(string username)
        {
            var user=_repo.GetAll().FirstOrDefault(x => x.username == username);
            if (user!=null)
            {
                return false;
            }
            return true;
        }
        private static void Authorize(string userName,int id)
        {
            string a = "Admin,"+id.ToString();
            var ticket = new FormsAuthenticationTicket(
                1,
                userName,
                DateTime.Now,
                DateTime.Now.AddDays(1),
                false,
                a,
                FormsAuthentication.FormsCookiePath);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);

            if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;

            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }
}
