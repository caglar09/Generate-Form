using AutoGenerateForm.Core.Repository;
using AutoGenerateForm.Core.UserOperation;
using AutoGenerateForm.DATA.Data;
using AutoGenerateForm.Models;
using System;
using System.Web.Mvc;

namespace AutoGenerateForm.Controllers
{
    public class AccountController : Controller
    {

        private IRepository<User> _repo;
        private UserService _userService;
        public AccountController()
        {
            AutoGenerateFormEntitiy context = new AutoGenerateFormEntitiy();
            _repo = new Repository<User>(context);

            _userService = new UserService();
        }

        public ActionResult Login()
        {
            if (_userService.IsLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.UserName = "";
            ViewBag.Password ="";

            return View();
        }

        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user.username!=null && user.password!=null)
            {
                try
                {
                    _userService.Login(user.username, user.password);
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                    ViewBag.UserName = user.username;
                    ViewBag.Password = user.password;
                    return View();
                }
            }
            else
            {
                if (user.username==null)
                {
                    ViewBag.nullUsername = "Kullanıcı Adı Boş Olamaz";
                }
                if(user.password==null)
                {
                    ViewBag.nullPassword = "Şifre Boş Olamaz";
                }
                return View();
            }
           

        }


        public ActionResult Logout()
        {
            _userService.LogOut();
            return RedirectToAction("Index","Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            if (_userService.IsLogin())
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (_userService.checkUserName(user.username))
                    {
                        _repo.Add(user);
                        _repo.SaveChanges();
                        _userService.Login(user.username, user.password);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["error"] = "Bu Kullanıcı Adı Zaten Kullanılıyor.. Başka bir kullanıcı adı giriniz..";
                        return View();
                    }
                   
                }
                catch (Exception ex)
                {
                    TempData["error"] = ex.Message;
                    return View();
                }
            }
            else
            {
                if (user.username == null)
                {
                    ViewBag.nullUsername = "Kullanıcı Adı Boş Olamaz";
                }
                if (user.password == null)
                {
                    ViewBag.nullPassword = "Şifre Boş Olamaz";
                }
                return View();
            }
           
        }

    }
}