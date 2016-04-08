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
    public class DevicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Devices
        public ActionResult Index()
        {
            var device = db.Device.Include(d => d.Software).Include(d => d.User);
            return View(device.ToList());
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Device.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            ViewBag.SoftwareId = new SelectList(db.Software, "Id", "Name");
            //ViewBag.UserId = new SelectList(db.ApplicationUsers, "Id", "UserName");
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,UserId,Brand,ProductId,Processor,Ram,HardDrive,SoftwareId,DateCreated,CreatedBy,DateModified,ModifiedBy")] Device device)
        {
            if (ModelState.IsValid)
            {
                device.DateCreated  = DateTime.Now;
                device.CreatedBy    = User.Identity.Name;
                device.DateModified = DateTime.Now;
                device.ModifiedBy   = User.Identity.Name;

                db.Device.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SoftwareId = new SelectList(db.Software, "Id", "Name", device.SoftwareId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", device.UserId);
            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Device.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            ViewBag.SoftwareId = new SelectList(db.Software, "Id", "Name", device.SoftwareId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", device.UserId);
            return View(device);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,UserId,Brand,ProductId,Processor,Ram,HardDrive,SoftwareId,DateCreated,CreatedBy,DateModified,ModifiedBy")] Device device)
        {
            if (ModelState.IsValid)
            {
                var a = db.Device.Where(x => x.Id == device.Id).FirstOrDefault();
                if (a != null)
                {
                    a.Id = a.Id;
                    if (a.UserId != device.UserId)
                    {
                        Log Log = new Log();

                        {
                            Log.DeviceId = device.Id;
                            Log.OldOwner = a.UserId;
                            Log.NewOwner = device.UserId;
                            Log.DateCreated = DateTime.Now;
                            Log.CreatedBy = User.Identity.Name;

                        }
                        db.Log.Add(Log);
                        db.SaveChanges();

                    }
                    a.UserId = device.UserId;
                    a.Brand = device.Brand;
                    a.ProductId = device.ProductId;
                    a.Processor = device.Processor;
                    a.Ram = device.Ram;
                    a.HardDrive = device.HardDrive;
                    a.DateCreated = a.DateCreated;
                    a.CreatedBy = a.CreatedBy;
                    a.DateModified = DateTime.Now;
                    a.ModifiedBy = User.Identity.Name;
                }

                
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SoftwareId = new SelectList(db.Software, "Id", "Name", device.SoftwareId);
            ViewBag.UserId = new SelectList(db.Users, "Id", "Email", device.UserId);
            return View(device);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Device.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = db.Device.Find(id);
            db.Device.Remove(device);
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
