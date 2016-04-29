using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using DeviceMS.Models;
using System.Net;

namespace DeviceMS.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        ApplicationDbContext context;

        public RoleController()
        {
            context = new ApplicationDbContext();
        }
        //Check for user role level
        public int checkLevel()
        {
            context = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());

                switch (s[0].ToString())
                {
                    case "Admin":
                    case "Manager":
                        return 1;
                    case "Staff":
                        return 2;
                    default:
                        return 0;
                }
            }
            return -1;
        }

        /// <summary>
        /// Get All Roles
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            //Check login status
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            var Roles = context.Roles.ToList();
            return View(Roles);

        }
        
        /// <summary>
        /// Create  a New role
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var intLevel = checkLevel();
            switch (intLevel)
            {
                case 1:
                    break;
                case 2:
                    return RedirectToAction("Index");
                case -1:
                    return RedirectToAction("Account", "Login");
                default:
                    return RedirectToAction("Index");
            }

            var Role = new IdentityRole();
            return View(Role);
        }

        /// <summary>
        /// Create a New Role
        /// </summary>
        /// <param name="Role"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(IdentityRole Role)
        {
            if (Role == null)
            {
                return RedirectToAction("Index");
            }
            context.Roles.Add(Role);
            context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}