using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using UserStore.BLL.DTO;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;
using UserStore.Models;

namespace UserStore.Controllers
{
    public class HomeController : Controller
    {
        private IPageService pageService;
        public HomeController()
        {
            pageService = new PageService();
        }
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        public ActionResult Error()
        {
            return View("Error");
        }
        [Authorize]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            var t = pageService.GetUserByID(id).Login;
            if (t == null)
                return View("Error");
            return Redirect("/" + t);
        }
        [Authorize]
        public ActionResult UserPage(string id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            var t = pageService.GetUserByLogin(id);
            if (t == null)
                return View("Error");
            return View(mapper.Map<UserModel>(t));
        }
        [Authorize(Roles = "admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        [Authorize]
        public ActionResult ViewImage(string id)
        {
            var item = pageService.GetAvatar(id);
            if (item == null)
                return new EmptyResult();
            byte[] buffer = item.Avatar;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }
        [HttpPost]
        public ActionResult UploadImages(HttpPostedFileBase uploadImage,UserModel model)
        {
         
                if (uploadImage!=null)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                    }
                    var headerImage = new AvatarDTO()
                    {
                        Avatar = imageData,
                        Login = model.Login   
                    };
                    pageService.SetAvatar(headerImage);
                }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, UserDTO>());
            var mapper = config.CreateMapper();
            var t=mapper.Map<UserDTO>(model);
            t.Id = User.Identity.GetUserId();
            pageService.ChangeUserInfo(t);
            // return RedirectToAction("Error");
            return Redirect("/" + model.Login);
        }
 
        public ActionResult EditModel()
        {
            string id = User.Identity.GetUserId();
            var t = pageService.GetUserByID(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView(mapper.Map<UserModel>(t));
        }
    }
}