using AutoGenerateForm.Core.Repository;
using AutoGenerateForm.DATA.Data;
using AutoGenerateForm.ViewModel;
using AutoGenerateForm.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Security;

namespace AutoGenerateForm.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        public IRepository<Form> _repo;
        public HomeController()
        {
            AutoGenerateFormEntitiy context = new AutoGenerateFormEntitiy();
            _repo = new Repository<Form>(context);
        }
        [Route("")]
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public JsonResult GetAllForms(string keyword)
        {
            FormsIdentity identity = (FormsIdentity)HttpContext.User.Identity;
            int id = Convert.ToInt32(identity.Ticket.UserData.Split(',')[1]);
            List<Form> allForms = _repo.GetAll().Where(x=>x.createdBy==id).ToList();
            if (!string.IsNullOrEmpty(keyword))
            {
                allForms = allForms.Where(x => x.name.Contains(keyword)).ToList();
            }

            return Json(allForms.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("forms/{id}")]
        public ActionResult Forms(int id)
        {
            if (ModelState.IsValid)
            {
                Form form = _repo.GetAll().FirstOrDefault(x=>x.id==id);
                FormModel model = new FormModel();
                model.name = form.name;
                model.description = form.description;
                var flds = JsonConvert.DeserializeObject<List<Fileds>>(form.fields);
                model.fields = flds;
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }

       
        [HttpPost]
        public JsonResult AddForm(FormModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int i = 1;
                    FormsIdentity identity = (FormsIdentity)HttpContext.User.Identity;

                    Form form = new Form
                    {
                        createdAt = DateTime.Now,
                        name = model.name,
                        description = model.description,
                        fields = "["
                    };
                    foreach (Fileds item in model.fields)
                    {
                        if (i == model.fields.Count)
                        {
                            form.fields += JsonConvert.SerializeObject(item);
                        }
                        else
                        {
                            form.fields += JsonConvert.SerializeObject(item) + ",";
                        }

                        i++;
                    }
                    form.fields += "]";
                    form.createdBy = Convert.ToInt32(identity.Ticket.UserData.Split(',')[1]);

                    _repo.Add(form);
                    _repo.SaveChanges();

                    Models.Response response = new Models.Response();
                    response.Message = "Ekleme İşlemi Başarılı";
                    response.Result = "";
                    response.status = true;
                    return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);

                }
                catch (Exception ex)
                {
                    Models.Response response = new Models.Response();
                    response.Message = ex.Message;
                    response.Result = "Ekleme İşlemi Başarısız";
                    response.status = true;
                    return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new Exception("Form Kayıt Edilemedi !!"), JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DeleteForm(int id)
        {
            try
            {
                FormsIdentity identity = (FormsIdentity)HttpContext.User.Identity;
                int userId = Convert.ToInt32(identity.Ticket.UserData.Split(',')[1]);

                Form form = _repo.GetAll().FirstOrDefault(x=>x.id==id);
                if (form != null && form.createdBy==userId)
                {
                    _repo.Delete(form);
                    _repo.SaveChanges();
                    Models.Response response = new Models.Response();
                    response.Message = "Silme İşlemi Başarılı";
                    response.Result = "";
                    response.status = true;
                    return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Models.Response response = new Models.Response();
                    response.Message = "Silme İşlemi Başarısız";
                    response.Result = "Böyle Bir Form Bulunamadı !!";
                    response.status = false;
                    return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception)
            {
                return Json(new Exception("Form Silinemedi !!"), JsonRequestBehavior.AllowGet);
            }
        }

    }
}