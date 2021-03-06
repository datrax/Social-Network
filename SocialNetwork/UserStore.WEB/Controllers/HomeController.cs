﻿using System;
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
        public HomeController(IPageService pageService)
        {
            this.pageService = pageService;
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
            //case when there's cookie but db is just created
            if (pageService.GetUserByID(id) == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            var login = pageService.GetUserByID(id).Login;
            return Redirect("/" + login);
        }
        [Authorize]
        public ActionResult UserPage(string id)
        {
            //case when there's cookie but db is just created
            string idid = User.Identity.GetUserId();
            if (pageService.GetUserByID(idid) == null)
            {
                return RedirectToAction("Logout", "Account");
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            var user = pageService.GetUserByLogin(id);
            if (user == null)
                return View("Error");
            return View(mapper.Map<UserModel>(user));
        }


        [Authorize]
        public ActionResult ViewImage(string id)
        {
            var item = pageService.GetAvatar(id);
            if (item == null)
                return new EmptyResult();
            byte[] buffer = item.Avatar;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadImages(HttpPostedFileBase uploadImage, HttpPostedFileBase themeImage, UserModel model)
        {

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, UserDTO>());
            var mapper = config.CreateMapper();
            var user = mapper.Map<UserDTO>(model);
            user.Id = User.Identity.GetUserId();
            var result = pageService.ChangeUserInfo(user);
            if (result != null)
            {
                return Json(new { result = false, responseText = result });
            }
            if (uploadImage != null)
            {
                if (!(uploadImage.ContentType == "image/jpeg" || uploadImage.ContentType == "image/png"))
                {
                    return Json(new { result = false, responseText = "Image can be only jpg/png" });
                }
                if (uploadImage.ContentLength > 40960*1024)
                {
                    return Json(new { result = false, responseText = "Image is bigger than 40Mb" });
                }
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                var headerImage = new AvatarDTO()
                {
                    Avatar = imageData,
                    UserId = model.Id
                };
                var res = pageService.SetAvatar(headerImage);
                if (!res)
                {
                    return Json(new { result = false, responseText = "Internal server error. Cannot set a photo." });
                }

            }
            if (themeImage != null)
            {
                if (!(themeImage.ContentType == "image/jpeg" || themeImage.ContentType == "image/png"))
                {
                    return Json(new { result = false, responseText = "Image can be only jpg/png" });
                }
                if (themeImage.ContentLength > 40960 * 1024)
                {
                    return Json(new { result = false, responseText = "Image is bigger than 40Mb" });
                }
                byte[] imageData2 = null;
                using (var binaryReader = new BinaryReader(themeImage.InputStream))
                {
                    imageData2 = binaryReader.ReadBytes(themeImage.ContentLength);
                }
                var headerImage2 = new AvatarDTO()
                {
                    Avatar = imageData2,
                    UserId = model.Id + "!Background"
                };
                var res2 = pageService.SetAvatar(headerImage2);
                if (!res2)
                {
                    return Json(new { result = false, responseText = "Internal server error. Cannot set a photo." });
                }
            }
            return Json(new { result = "Redirect", url = "/" + model.Login });
        }
        [Authorize]
        [HttpPost]
        public ActionResult EditModel()
        {
            string id = User.Identity.GetUserId();
            var user = pageService.GetUserByID(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView(mapper.Map<UserModel>(user));
        }
        [Authorize]
        [HttpPost]
        public ActionResult Search(string searchField)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView("SearchUsers", mapper.Map<IEnumerable<UserModel>>(pageService.FindUsers(searchField)));
        }
        [Authorize]
        [HttpPost]
        public ActionResult Wall(string id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PostDTO, PostModel>());
            var mapper = config.CreateMapper();
            return PartialView("Wall", mapper.Map<IEnumerable<PostModel>>(pageService.GetPosts(User.Identity.GetUserId(), id)));
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeletePost(string id)
        {
            int t;
            if (!int.TryParse(id, out t))
            {
                return Json(new { result = false, responseText = "In LikePost. Post not found" });
            }
            var postowner = pageService.GetPostWallOwnerById(t);
            if (postowner == null)
            {
                return Json(new { result = false, responseText = "In LikePost. Post not found" });
            }
            if (!pageService.DeletePost(Int32.Parse(id), User.Identity.GetUserId()))
            {
                return Json(new { result = false, responseText = "In DeletePost. Internal error" });
            }
            return Wall(postowner);
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddPost(string id, string postField, HttpPostedFileBase uploadImage)
        {
            byte[] imageData = null;
            if (uploadImage != null)
            {
                if (!(uploadImage.ContentType == "image/jpeg"|| uploadImage.ContentType == "image/png"))
                {
                    return Wall(id);
                }
                if (uploadImage.ContentLength > 40960 * 1024)
                {
                    return Json(new { result = false, responseText = "Image is bigger than 40Mb" });
                }
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
            }
    
            pageService.AddPost(User.Identity.GetUserId(), id, postField, imageData);               
            return Wall(id);
        }
        [Authorize]
        [HttpPost]
        public ActionResult LikePost(string id)
        {
            int t;
            if (!int.TryParse(id, out t))
            {
                return Json(new { result = false, responseText = "In LikePost. Post not found" });
            }
            var postowner = pageService.GetPostWallOwnerById(t);
            if (postowner == null)
            {
                return Json(new { result = false, responseText = "In LikePost. Post not found" });
            }
            pageService.LikePost(User.Identity.GetUserId(), t);
            
            return Wall(postowner);
            // 
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetLikeUsers(string id)
        {
            int t;
            if (!int.TryParse(id, out t))
            {
                return Json(new { result = false, responseText = "In LikePost. Post not found" });
            }
            var postowner = pageService.GetPostWallOwnerById(t);
            if (postowner == null)
            {
                return Json(new { result = false, responseText = "In LikePost. Post not found" });
            }
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView("SearchUsers", mapper.Map<IEnumerable<UserModel>>(pageService.GetLikeUserList(t)));
        }

    }
}