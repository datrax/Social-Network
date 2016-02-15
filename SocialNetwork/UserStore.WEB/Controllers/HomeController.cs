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
            if (pageService.GetUserByLogin(id) == null)
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
        public ActionResult UploadImages(HttpPostedFileBase uploadImage, UserModel model)
        {
           
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, UserDTO>());
            var mapper = config.CreateMapper();
            var user = mapper.Map<UserDTO>(model);
            user.Id = User.Identity.GetUserId();
            var result=pageService.ChangeUserInfo(user);
            if (result != null)
            {
                return Json(new { result = false, responseText = result });
            }
            if (uploadImage != null)
            {
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
                var res=pageService.SetAvatar(headerImage);
                if (!res)
                {
                    return Json(new { result = false, responseText = "Internal server error. Cannot set a photo." });
                }
            }
            return Json(new { result = "Redirect", url = "/" + model.Login});
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
            return PartialView("Wall",mapper.Map<IEnumerable<PostModel>>(pageService.GetPosts(User.Identity.GetUserId(), id)));
        }
        [Authorize]
        [HttpPost]
        public ActionResult DeletePost(string id)
        {
            var postWallOwnerId = pageService.GetPostWallOwnerById(Int32.Parse(id));
            pageService.DeletePost(Int32.Parse(id), User.Identity.GetUserId());
            return Wall(postWallOwnerId);
        }
        [Authorize]
        [HttpPost]
        public ActionResult AddPost(string id,string postField, HttpPostedFileBase uploadImage)
        {
            byte[] imageData = null;
            if (uploadImage != null)
            {

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
            pageService.LikePost(User.Identity.GetUserId(), Int32.Parse(id));            
            return Wall(pageService.GetPostWallOwnerById(Int32.Parse(id)));
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetLikeUsers(string id)
        {           
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView("SearchUsers", mapper.Map<IEnumerable<UserModel>>(pageService.GetLikeUserList(Int32.Parse(id))));
        }

    }
}