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


        public ActionResult ViewImage(string id)
        {
            var item = pageService.GetAvatar(id);
            if (item == null)
                return new EmptyResult();
            byte[] buffer = item.Avatar;
            return File(buffer, "image/jpg", string.Format("{0}.jpg", id));
        }

        public string GetImageName(string id)
        {
            var item = pageService.GetAvatar(id);
            if (item == null)
                return null;
            return "/Home/ViewImage/" + id;
        }
        [HttpPost]
        public ActionResult UploadImages(HttpPostedFileBase uploadImage, UserModel model)
        {
            if (string.IsNullOrEmpty(model.Name ) )
                return Json(new { result = false, responseText = "Please enter name" });

            if (string.IsNullOrEmpty(model.Surname))
                return Json(new { result = false, responseText = "Please enter surname" });
            if(pageService.GetUserByLogin(model.Login)!=null&& pageService.GetUserByLogin(model.Login).Id!=model.Id)
                return Json(new { result = false, responseText = "This URL is busy" });

            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserModel, UserDTO>());
            var mapper = config.CreateMapper();
            var t = mapper.Map<UserDTO>(model);
            t.Id = User.Identity.GetUserId();
            pageService.ChangeUserInfo(t);

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
                pageService.SetAvatar(headerImage);
            }

            // return RedirectToAction("Error");
            return Json(new { result = "Redirect", url = "/" + model.Login});
        }

        public ActionResult EditModel()
        {
            string id = User.Identity.GetUserId();
            var t = pageService.GetUserByID(id);
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView(mapper.Map<UserModel>(t));
        }

        public ActionResult Search(string searchField)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView("SearchUsers", mapper.Map<IEnumerable<UserModel>>(pageService.FindUsers(searchField)));
        }

        public ActionResult SearchUsers()
        {
            return new EmptyResult();
        }
        public ActionResult Wall(string id)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<PostDTO, PostModel>());
            var mapper = config.CreateMapper();            
            return PartialView("Wall",mapper.Map<IEnumerable<PostModel>>(pageService.GetPosts(User.Identity.GetUserId(), id)));
        }
        public ActionResult DeletePost(string id)
        {
            var postId = pageService.GetPostWallOwnerById(Int32.Parse(id));
            pageService.DeletePost(Int32.Parse(id));

            return Wall(postId);
        }
        public ActionResult AddPost(string id,string postField, HttpPostedFileBase uploadImage)
        {
            pageService.AddPost(User.Identity.GetUserId(), id, postField);
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
                    UserId = postField
                };
                pageService.SetAvatar(headerImage);
            }
            return Wall(id);
        }
        public ActionResult LikePost(string id)
        {
            pageService.LikePost(User.Identity.GetUserId(), Int32.Parse(id));

            
            return Wall(pageService.GetPostWallOwnerById(Int32.Parse(id)));
        }
        public ActionResult GetLikeUsers(string id)
        {           
            var config = new MapperConfiguration(cfg => cfg.CreateMap<UserDTO, UserModel>());
            var mapper = config.CreateMapper();
            return PartialView("SearchUsers", mapper.Map<IEnumerable<UserModel>>(pageService.GetLikeUserList(Int32.Parse(id))));
        }

    }
}