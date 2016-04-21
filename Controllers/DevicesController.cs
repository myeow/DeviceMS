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
            return View(db.Devices.ToList());
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            var Results = from s in db.Softwares
                          select new
                          {
                              s.SoftwareId,
                              s.Name,
                              Checked = ((from sd in db.SoftwaresToDevices
                                              where (sd.DeviceId) == id & (sd.SoftwareId == s.SoftwareId)
                                              select sd).Count()>0)
                          };

            var DVM = new DeviceViewModel();
            DVM.DeviceId = id.Value;
            DVM.Name = device.Name;
            DVM.ProductId = device.ProductId;
            DVM.Processor = device.Processor;
            DVM.Ram = device.Ram;
            DVM.HardDrive = device.HardDrive;
            DVM.DateCreated = device.DateCreated;
            DVM.CreatedBy = device.CreatedBy;
            DVM.DateModified = device.DateModified;
            DVM.ModifiedBy = device.ModifiedBy;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.SoftwareId, Name = item.Name, Checked = item.Checked });
            }

            DVM.Softwares = MyCheckBoxList;
            return View(DVM);        
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DeviceId,Name,ProductId,Processor,Ram,HardDrive,DateCreated,CreatedBy,DateModified,ModifiedBy")] Device device)
        {
            if (ModelState.IsValid)
            {
                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }

            var Results = from s in db.Softwares
                          select new
                          {
                              s.SoftwareId,
                              s.Name,
                              Checked = ((from sd in db.SoftwaresToDevices
                                              where (sd.DeviceId) == id & (sd.SoftwareId == s.SoftwareId)
                                              select sd).Count()>0)
                          };

            var DVM = new DeviceViewModel();
            DVM.DeviceId = id.Value;
            DVM.Name = device.Name;
            DVM.ProductId = device.ProductId;
            DVM.Processor = device.Processor;
            DVM.Ram = device.Ram;
            DVM.HardDrive = device.HardDrive;
            DVM.DateCreated = device.DateCreated;
            DVM.CreatedBy = device.CreatedBy;
            DVM.DateModified = device.DateModified;
            DVM.ModifiedBy = device.ModifiedBy;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.SoftwareId, Name = item.Name, Checked = item.Checked });
            }

            DVM.Softwares = MyCheckBoxList;
            return View(DVM);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "DeviceId,Name,ProductId,Processor,Ram,HardDrive,DateCreated,CreatedBy,DateModified,ModifiedBy")] Device device)
        public ActionResult Edit(DeviceViewModel device)
        {
            if (ModelState.IsValid)
            {
                var MyDevice = db.Devices.Find(device.DeviceId);

                MyDevice.Name = device.Name;
                MyDevice.ProductId = device.ProductId;
                MyDevice.Processor = device.Processor;
                MyDevice.Ram = device.Ram;
                MyDevice.HardDrive = device.HardDrive;
                MyDevice.DateCreated = device.DateCreated;
                MyDevice.CreatedBy = device.CreatedBy;
                MyDevice.DateModified = device.DateModified;
                MyDevice.ModifiedBy = device.ModifiedBy;

                foreach (var item in db.SoftwaresToDevices)
                {
                    if (item.DeviceId == device.DeviceId)
                    {
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    }
                }

                foreach (var item in device.Softwares)
                {
                    if (item.Checked)
                    {
                        db.SoftwaresToDevices.Add(new SoftwareToDevice() { DeviceId = device.DeviceId, SoftwareId = item.Id });
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(device);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
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
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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
