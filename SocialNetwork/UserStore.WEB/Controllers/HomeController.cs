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
        public ActionResult UploadImages(HttpPostedFileBase[] uploadImages, string id)
        {
            if (uploadImages.Count() != 1)
            {
                return RedirectToAction("Error");
            }

            foreach (var image in uploadImages)
            {
                if (image.ContentLength > 0)
                {
                    byte[] imageData = null;
                    using (var binaryReader = new BinaryReader(image.InputStream))
                    {
                        imageData = binaryReader.ReadBytes(image.ContentLength);
                    }
                    var headerImage = new AvatarDTO()
                    {
                        Avatar = imageData,
                        Login = id   
                    };
                    pageService.SetAvatar(headerImage);
                }
            }
            // return RedirectToAction("Error");
            return Redirect("/" + id);
        }
        public ActionResult EditModel(UserModel model)
        {
            return PartialView(model);
        }
    }
}