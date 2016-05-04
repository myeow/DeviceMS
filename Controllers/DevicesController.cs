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
using System.Web.Security;

namespace DeviceMS.Controllers
{
    public class DevicesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Devices
        [Authorize]
        public ActionResult Index(int? id, int? sid, string sortOrder)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var uid = User.Identity.GetUserId();
            var devices = db.Devices.Include(x => x.DevicesToUsers).ToList();

            switch (sortOrder)
            {
                case "name_desc":
                    devices = db.Devices.Include(x => x.DevicesToUsers).OrderByDescending(d => d.Name).ToList();
                    break;
                default :
                    devices = db.Devices.Include(x => x.DevicesToUsers).OrderBy(d => d.Name).ToList();
                    break;
            }

            List<DeviceViewModel> dvm_list = new List<DeviceViewModel>();
            foreach (var item in devices ){

                DeviceViewModel DeviceVM = new DeviceViewModel();
                DeviceVM.DeviceId   = item.DeviceId;
                DeviceVM.Name       = item.Name;

                List<string> email_list = new List<string>();
                foreach (var did in item.DevicesToUsers.OrderByDescending(d=>d.DateCreated))
                {
                    var email = db.Users.Where(x=>x.Id == did.UserID).FirstOrDefault();
                    //email_list.Add(email.Email+" | "+did.DateCreated);
                    if (email != null)
                    {
                        email_list.Add(email.FirstName + " " + email.LastName);
                    }
                    
                }
                DeviceVM.Email = email_list;

                List<string> software_list = new List<string>();
                foreach (var softwares_id in item.SoftwaresToDevices)
                {
                    var software = db.Softwares.Where(s => s.SoftwareId == softwares_id.SoftwareId).FirstOrDefault();
                    software_list.Add(software.Name);
                }
                DeviceVM.SoftwaresList = software_list;
                DeviceVM.ProductId = item.ProductId;
                DeviceVM.Processor = item.Processor;
                DeviceVM.Ram       = item.Ram;
                DeviceVM.HardDrive = item.HardDrive;

                dvm_list.Add(DeviceVM);
            }

            return View(dvm_list);
        }

        [Authorize]
        public ActionResult List()
        {
            var uid = User.Identity.GetUserId();
            var devices = db.Devices.Where(d => d.DevicesToUsers.Any(du => du.UserID == uid)).ToList();
            List<DeviceViewModel> dvm_list = new List<DeviceViewModel>();
            foreach (var item in devices)
            {

                DeviceViewModel DeviceVM = new DeviceViewModel();
                DeviceVM.DeviceId = item.DeviceId;
                DeviceVM.Name = item.Name;
                List<string> software_list = new List<string>();
                foreach (var softwares_id in item.SoftwaresToDevices)
                {
                    var software = db.Softwares.Where(s => s.SoftwareId == softwares_id.SoftwareId).FirstOrDefault();
                    software_list.Add(software.Name);
                }
                DeviceVM.SoftwaresList = software_list;
                DeviceVM.ProductId = item.ProductId;
                DeviceVM.Processor = item.Processor;
                DeviceVM.Ram = item.Ram;
                DeviceVM.HardDrive = item.HardDrive;

                dvm_list.Add(DeviceVM);
            }

            return View(dvm_list);
        }
        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Check login status
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            Device device   = db.Devices.Find(id);
            var DeviceVM    = new DeviceViewModel();
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
                list_email.Add(email.FirstName +" "+ email.LastName +" | "+ u_id.DateCreated );
            }

            //Get software list
            var sRes = db.SoftwaresToDevices.Where(sd => sd.DeviceId == id).ToArray();
            List<string> list_software = new List<string>();
            foreach (var sid in sRes)
            {
                var software = db.Softwares.Where(s => s.SoftwareId == sid.SoftwareId).FirstOrDefault();
                list_software.Add(software.Name);
            }

            DeviceVM.DeviceId   = id.Value;
            DeviceVM.Name       = device.Name;
            ViewBag.Email       = list_email;
            ViewBag.Software    = list_software;
            DeviceVM.ProductId  = device.ProductId;
            DeviceVM.Processor  = device.Processor;
            DeviceVM.Ram        = device.Ram;
            DeviceVM.HardDrive  = device.HardDrive;
            DeviceVM.DateCreated = device.DateCreated;
            DeviceVM.CreatedBy  = device.CreatedBy;
            DeviceVM.DateModified = device.DateModified;
            DeviceVM.ModifiedBy = device.ModifiedBy;

            return View(DeviceVM);
        }

        // GET: Devices/Create
        [Authorize(Roles="Admin")]
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
            var DeviceVM = new DeviceViewModel();
            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.SoftwareId, Name = item.Name });
            }

            DeviceVM.Softwares = MyCheckBoxList;
            return View(DeviceVM);
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
                if (Users != "" && Users != null)
                {
                    var DvU = new DeviceToUser();
                    DvU.UserID = Users;
                    DvU.DeviceID = device.DeviceId;
                    DvU.DateCreated = DateTime.Now;
                    db.DevicesToUsers.Add(DvU);
                }
                

                device.Name         = device.Name;
                device.ProductId    = device.ProductId;
                device.Processor    = device.Processor;
                device.Ram          = device.Ram;
                device.HardDrive    = device.HardDrive;

                device.DateCreated  = DateTime.Now;
                device.DateModified = DateTime.Now;
                
                device.ModifiedBy   = (User.Identity.Name == "") ? "system" : User.Identity.Name;
                device.CreatedBy    = (User.Identity.Name == "") ? "system" : User.Identity.Name;
                
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

        
        [Authorize(Roles="Admin,Manager")]
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

            }
            PopulateUserList(ViewBag.uid);
            var Results = from s in db.Softwares
                          select new
                          {
                              s.SoftwareId,
                              s.Name,
                              Checked = ((from sd in db.SoftwaresToDevices
                                              where (sd.DeviceId) == id & (sd.SoftwareId == s.SoftwareId)
                                              select sd).Count()>0)
                          };

            var DeviceVM            = new DeviceViewModel();
            DeviceVM.DeviceId       = id.Value;
            DeviceVM.Name           = device.Name;
            DeviceVM.ProductId      = device.ProductId;
            DeviceVM.Processor      = device.Processor;
            DeviceVM.Ram            = device.Ram;
            DeviceVM.HardDrive      = device.HardDrive;
            DeviceVM.DateCreated    = device.DateCreated;
            DeviceVM.CreatedBy      = device.CreatedBy;
            DeviceVM.DateModified   = device.DateModified;
            DeviceVM.ModifiedBy     = device.ModifiedBy;

            var MyCheckBoxList = new List<CheckBoxViewModel>();
            foreach (var item in Results)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.SoftwareId, Name = item.Name, Checked = item.Checked });
            }
            DeviceVM.Softwares = MyCheckBoxList;

            return View(DeviceVM);
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
                //var uid = db.DevicesToUsers.OrderByDescending(du => du.DateCreated).Where(du => du.DeviceID == device.DeviceId).FirstOrDefault().UserID;
                //check if user has changed
                if (Users != uid && Users !="")
                {
                    var DvU         = new DeviceToUser();
                    DvU.DeviceID    = device.DeviceId;
                    DvU.UserID      = Users;
                    DvU.DateCreated = DateTime.Now;
                    db.DevicesToUsers.Add(DvU);
                }

                var MyDevice            = db.Devices.Find(device.DeviceId);

                MyDevice.Name           = device.Name;
                MyDevice.ProductId      = device.ProductId;
                MyDevice.Processor      = device.Processor;
                MyDevice.Ram            = device.Ram;
                MyDevice.HardDrive      = device.HardDrive;
                MyDevice.DateCreated    = device.DateCreated;
                MyDevice.CreatedBy      = device.CreatedBy;
                MyDevice.DateModified   = DateTime.Now;
                MyDevice.ModifiedBy     = (User.Identity.Name == "") ? "system" : User.Identity.Name;

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

        [Authorize(Roles="Admin")]
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

        private void PopulateUserList(object selectedUser = null)
        {
            //var userQuery = db.Users.Where(u => u.Id == selectedUser);
            var userQuery = from u in db.Users
                            orderby u.Email
                            select u;
            ViewBag.Users2 = new SelectList(userQuery, "Id", "Email", selectedUser);
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
