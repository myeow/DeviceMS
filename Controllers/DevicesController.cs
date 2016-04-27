﻿using System;
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
        public ActionResult Index(int? id, int? sid)
        {
            var devices = db.Devices.Include(x => x.DevicesToUsers).ToList();

            List<DViewModel> dvu = new List<DViewModel>();                
            foreach (var item in devices ){
                DViewModel dvm_u = new DViewModel();
                dvm_u.DeviceId = item.DeviceId;
                dvm_u.Name = item.Name;
                List<string> email_list = new List<string>();
                foreach (var did in item.DevicesToUsers.OrderByDescending(d=>d.DateCreated))
                {
                    var email = db.Users.Where(x=>x.Id == did.UserID).FirstOrDefault();
                    email_list.Add(email.Email);
                }
                dvm_u.Email = email_list;

                List<string> software_list = new List<string>();
                foreach (var softwares_id in item.SoftwaresToDevices)
                {
                    var software = db.Softwares.Where(s => s.SoftwareId == softwares_id.SoftwareId).FirstOrDefault();
                    software_list.Add(software.Name);
                }
                dvm_u.Softwares = software_list;
                dvm_u.ProductId = item.ProductId;
                dvm_u.Processor = item.Processor;
                dvm_u.Ram = item.Ram;
                dvm_u.HardDrive = item.HardDrive;
                dvu.Add(dvm_u);
            }

            return View(dvu);
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Device device = db.Devices.Find(id);
            var DVM = new DeviceViewModel();
            if (device == null)
            {
                return HttpNotFound();
            }
            
            //Get email list
            var eRes = db.DevicesToUsers.Where(du => du.DeviceID == id).OrderByDescending( du => du.DateCreated).ToArray();
            List<string> list_email = new List<string>();
            foreach (var u_id in eRes)
            {
                var email = db.Users.Where(u => u.Id == u_id.UserID).FirstOrDefault();
                list_email.Add(email.Email);
            }

            //Get software list
            var sRes = db.SoftwaresToDevices.Where(sd => sd.DeviceId == id).ToArray();
            List<string> list_software = new List<string>();
            foreach (var sid in sRes)
            {
                var software = db.Softwares.Where(s => s.SoftwareId == sid.SoftwareId).FirstOrDefault();
                list_software.Add(software.Name);
            }
            
            DVM.DeviceId = id.Value;
            DVM.Name = device.Name;
            ViewBag.Email = list_email;
            ViewBag.Software = list_software;
            DVM.ProductId = device.ProductId;
            DVM.Processor = device.Processor;
            DVM.Ram = device.Ram;
            DVM.HardDrive = device.HardDrive;
            DVM.DateCreated = device.DateCreated;
            DVM.CreatedBy = device.CreatedBy;
            DVM.DateModified = device.DateModified;
            DVM.ModifiedBy = device.ModifiedBy;

            return View(DVM);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            var userData = from u in db.Users
                           select new
                           {
                               u.Id,
                               u.Email
                           };
            SelectList userList = new SelectList(userData, "Id", "Email");
            ViewBag.Users = userList;

            var Results = from s in db.Softwares
                          select new
                          {
                              s.SoftwareId,
                              s.Name
                          };
            var DVM = new DeviceViewModel();
            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.SoftwareId, Name = item.Name });
            }

            DVM.Softwares = MyCheckBoxList;
            return View(DVM);
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( [Bind(Include = "DeviceId,Name,ProductId,Processor,Ram,HardDrive,DateCreated,CreatedBy,DateModified,ModifiedBy")] Device device,List<CheckBoxViewModel> Softwares, string Users)
        //public ActionResult Create(DeviceViewModel device, string Users, List<CheckBoxViewModel> Softwares)
        {

            if (ModelState.IsValid)
            {
                var DvU = new DeviceToUser();
                DvU.UserID = Users;
                DvU.DeviceID = device.DeviceId;
                DvU.DateCreated = DateTime.Now;
                db.DevicesToUsers.Add(DvU);

                device.Name = device.Name;
                device.ProductId = device.ProductId;
                device.Processor = device.Processor;
                device.Ram = device.Ram;
                device.HardDrive = device.HardDrive;

                device.DateCreated = DateTime.Now;
                device.DateModified = DateTime.Now;
                device.ModifiedBy = (User.Identity.Name == "") ? "system" : User.Identity.Name;
                device.CreatedBy = (User.Identity.Name == "") ? "system" : User.Identity.Name;
                
                foreach (var item in Softwares)
                {
                    if (item.Checked)
                    {
                        db.SoftwaresToDevices.Add(new SoftwareToDevice() { DeviceId = device.DeviceId, SoftwareId = item.Id });
                    }
                }

                db.Devices.Add(device);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(device);
        }

        private void PopulateUserList(object selectedUser = null)
        {
            //var userQuery = db.Users.Where(u => u.Id == selectedUser);
            var userQuery = from u in db.Users
                            orderby u.Email
                            select u;
            ViewBag.Users = new SelectList(userQuery, "Id", "Email", selectedUser);
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

            var uid = db.DevicesToUsers.OrderByDescending(du => du.DateCreated).Where(du => du.DeviceID == id).FirstOrDefault();
            if (uid != null)
            {
                ViewBag.uid = uid.UserID;
                PopulateUserList(uid);
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
        public ActionResult Edit(DeviceViewModel device,string Users, string uid)
        {
            if (ModelState.IsValid)
            {
                //check if user has changed
                if (Users != uid)
                {
                    var DvU = new DeviceToUser();
                    DvU.DeviceID = device.DeviceId;
                    DvU.UserID = Users;
                    DvU.DateCreated = DateTime.Now;
                    db.DevicesToUsers.Add(DvU);
                }

                var MyDevice = db.Devices.Find(device.DeviceId);

                MyDevice.Name = device.Name;
                MyDevice.ProductId = device.ProductId;
                MyDevice.Processor = device.Processor;
                MyDevice.Ram = device.Ram;
                MyDevice.HardDrive = device.HardDrive;
                MyDevice.DateCreated = device.DateCreated;
                MyDevice.CreatedBy = device.CreatedBy;
                MyDevice.DateModified = DateTime.Now;
                MyDevice.ModifiedBy = User.Identity.Name;
                
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
