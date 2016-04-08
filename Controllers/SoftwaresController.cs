using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeviceMS.Models;

namespace DeviceMS.Controllers
{
    public class SoftwaresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Softwares
        public ActionResult Index()
        {
            return View(db.Software.ToList());
        }

        // GET: Softwares/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Software software = db.Software.Find(id);
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
        public ActionResult Create([Bind(Include = "Id,Name,DateCreated,CreatedBy,DateModified,ModifiedBy")] Software software)
        {
            if (ModelState.IsValid)
            {
                software.DateCreated = DateTime.Now;
                software.CreatedBy = User.Identity.Name;
                software.DateModified = DateTime.Now;
                software.ModifiedBy = User.Identity.Name;

                db.Software.Add(software);
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
            Software software = db.Software.Find(id);
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
        public ActionResult Edit([Bind(Include = "Id,Name,DateCreated,CreatedBy,DateModified,ModifiedBy")] Software software)
        {
            if (ModelState.IsValid)
            {
                var a = db.Software.Where(x => x.Id == software.Id).FirstOrDefault();

                if (a != null)
                {
                    a.Id = a.Id;
                    a.Name = software.Name;
                    a.DateCreated = a.DateCreated;
                    a.CreatedBy = a.CreatedBy;
                    a.DateModified = DateTime.Now;
                    a.ModifiedBy = User.Identity.Name;
                }
                db.Entry(a).State = EntityState.Modified;
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
            Software software = db.Software.Find(id);
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
            Software software = db.Software.Find(id);
            db.Software.Remove(software);
            db.SaveChanges();
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
