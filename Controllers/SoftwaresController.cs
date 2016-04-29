using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeviceMS.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace DeviceMS.Controllers
{
    [Authorize]
    public class SoftwaresController : Controller
    {
        ApplicationDbContext context;
        private ApplicationDbContext db = new ApplicationDbContext();

        public Boolean isAdminUser()
        {
            context = new ApplicationDbContext();
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        // GET: Softwares
        public ActionResult Index()
        {
            return View(db.Softwares.ToList());
        }

        // GET: Softwares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Software software = db.Softwares.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }
            return View(software);
        }

        // GET: Softwares/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Softwares/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoftwareId,Name,DateCreated,CreatedBy,DateModified,ModifiedBy")] Software software)
        {
            if (ModelState.IsValid)
            {
                software.DateCreated = DateTime.Now;
                software.CreatedBy = (User.Identity.Name == "") ? "system" : User.Identity.Name;
                software.DateModified = DateTime.Now;
                software.ModifiedBy = (User.Identity.Name == "") ? "system" : User.Identity.Name;
                db.Softwares.Add(software);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(software);
        }

        // GET: Softwares/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Software software = db.Softwares.Find(id);
            
            if (software == null)
            {
                return HttpNotFound();
            }
            return View(software);
        }

        // POST: Softwares/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "SoftwareId,Name,DateCreated,CreatedBy,DateModified,ModifiedBy")] Software software)
        public ActionResult Edit(SoftwareViewModel software)
        {
            if (ModelState.IsValid)
            {
                var MySoftware = db.Softwares.Find(software.SoftwareId);

                MySoftware.Name = software.Name;
                MySoftware.DateModified = DateTime.Now;
                MySoftware.ModifiedBy = (User.Identity.Name == "") ? "system" : User.Identity.Name;

                db.Entry(MySoftware).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(software);
        }

        // GET: Softwares/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Software software = db.Softwares.Find(id);
            if (software == null)
            {
                return HttpNotFound();
            }
            return View(software);
        }

        // POST: Softwares/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Software software = db.Softwares.Find(id);

            var softwareUsed = db.SoftwaresToDevices.Where(s => s.SoftwareId == id).ToArray();
            int countSoftware = softwareUsed.Count();
            if ( countSoftware > 0)
            {
                ViewBag.ErrorDeleteMsg = "<div class='alert alert-danger'><strong>Danger!</strong> This software is being used by other devices, delete failed.</div>";
                return View(software);
            }
            else
            {
                ViewBag.ErrorDeleteMsg = "";
                db.Softwares.Remove(software);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
