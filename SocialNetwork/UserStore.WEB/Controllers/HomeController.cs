using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using UserStore.BLL.Interfaces;
using UserStore.BLL.Services;

namespace UserStore.Controllers
{
    public class HomeController : Controller
    {
        private IPageService pageService;
        public HomeController()
        {
            pageService=new PageService();
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
            return Redirect("/" + pageService.GetUserByID(id).Login);
        }
        [Authorize]
        public ActionResult UserPage(string id)
        {
            ViewBag.Text = id;
            return View();
        }
        [Authorize(Roles="admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }
    }
}