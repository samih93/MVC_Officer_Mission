using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using MVC_Officer_Mission.Models;
using MVC_Officer_Mission.Models.ViewModels;
using System.IO;

namespace MVC_Officer_Mission.Controllers
{
    [Authorize]
    public class OfficersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Officers
        public ActionResult Index()
        {
            var officers = db.Officers.Include(o => o.Rank).OrderByDescending(s => s.Rank.Id).ThenBy(o => o.MilitaryNumber);
            return View(officers.ToList());
        }

        // GET: Officers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Officer officer = db.Officers.Find(id);
            

            if (officer == null)
            {
                return HttpNotFound();
            }
            return View(officer);
        }

        // GET: Officers/Create
        public ActionResult Create()
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name");
            ViewBag.RankId = new SelectList(db.Ranks, "Id", "Name");
            ViewBag.SpecificationId = new SelectList(db.Specifications, "Id", "Name");
            return View();
        }

        // POST: Officers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            OfficerViewModel officerVM,
             List<int> specificationlist,
            HttpPostedFileBase ProfileImage
            )
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", officerVM.Officer.DepartmentId);
            ViewBag.RankId = new SelectList(db.Ranks, "Id", "Name", officerVM.Officer.RankId);
            ViewBag.SpecificationId = new SelectList(db.Specifications, "Id", "Name");


            //profile image file extension validation
            if (ProfileImage != null && !GlobalController.isImage(ProfileImage))
            {
                TempData["message"] = MessagingSystem.AddMessage("حقل الصورة الشخصية يجب أن يكون صورة", "danger");
                return View(officerVM);
            }
            //check if specification is not empty
            //if (specificationlist == null || specificationlist.Count == 0)
            //{
            //    TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage("التخصص"), "danger");
            //    return View(officerVM);
            //}

            if (ModelState.IsValid)
            {
                var dbCtxTransaction = db.Database.BeginTransaction();

                //Document Order File Handle
                if (ProfileImage != null && ProfileImage.ContentLength > 0)
                {
                    //file name as unix timestamp
                    string fileExtention = Path.GetExtension(ProfileImage.FileName);
                    //filename is for example 2018345434343.pdf
                    string fileName = String.Concat(HomeController.DateTimeToTimestamp(DateTime.Now).ToString(), fileExtention);
                    var path = Path.Combine(Server.MapPath("~/Media/" + WebConfigurationManager.AppSettings["OfficerProfileImageFolderName"]), fileName);
                    ProfileImage.SaveAs(path);
                    officerVM.Officer.ProfileImage = fileName;
                }

                //adding selected specification to officerVM
            
                if (specificationlist != null)
                {
                    specificationlist.ForEach(delegate (int specificationId)
                    {
                        officerVM.Officer.Specifications.Add(db.Specifications.Find(specificationId));
                    });
                }

                officerVM.Officer = db.Officers.Add(officerVM.Officer);

                db.SaveChanges();

                foreach (OfficerExternalTournament item in officerVM.OfficerExternalTournaments)
                {
                    //adding only non empty external tournaments
                    if (item.Name != null && !string.IsNullOrEmpty(item.Name.Trim()))
                    {
                        ExternalTournament extTournament = new ExternalTournament();
                        extTournament.Name = item.Name;
                        extTournament.OfficerId = officerVM.Officer.Id;
                        officerVM.Officer.ExternalTournaments.Add(extTournament);
                    }
                }
                db.SaveChanges();

                dbCtxTransaction.Commit();

                TempData["message"] = MessagingSystem.AddMessage("تم إضافة الضابط ' " + officerVM.Officer.FirstName + " " + officerVM.Officer.LastName + "' بنجاح", "success");

                return RedirectToAction("Index");
            }
            return View(officerVM);
        }

        // GET: Officers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //  Officer officer = db.Officers.Find(id);
            OfficerViewModel officerVM = new OfficerViewModel();
            officerVM.Officer = db.Officers.Find(id);

            if (officerVM.Officer == null)
            {
                return HttpNotFound();
            }
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", officerVM.Officer.DepartmentId);
            ViewBag.RankId = new SelectList(db.Ranks, "Id", "Name", officerVM.Officer.RankId);
            ViewBag.SpecificationId = new MultiSelectList(db.Specifications, "Id", "Name", GetspecificationOfficerIds(officerVM.Officer));

            if (officerVM.Officer.ExternalTournaments == null || officerVM.Officer.ExternalTournaments.Count == 0)
            {
                officerVM.OfficerExternalTournaments = new List<OfficerExternalTournament>();
                officerVM.OfficerExternalTournaments.Add(new OfficerExternalTournament());
            }


            foreach (var item in officerVM.Officer.ExternalTournaments)
            {
                OfficerExternalTournament OET = new OfficerExternalTournament();
                OET.Name = item.Name;
                officerVM.OfficerExternalTournaments.Add(OET);
            }

            return View(officerVM);
        }

        // POST: Officers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            OfficerViewModel officerVM,
             List<int> specificationlist,
             HttpPostedFileBase ProfileImage,
            string DeleteProfileImage,
            string OldProfileImage
            )
        {
            ViewBag.DepartmentId = new SelectList(db.Departments, "Id", "Name", officerVM.Officer.DepartmentId);
            ViewBag.RankId = new SelectList(db.Ranks, "Id", "Name", officerVM.Officer.RankId);
            ViewBag.specificationlist = new MultiSelectList(db.Officers, "Id", "FullName", GetspecificationOfficerIds(officerVM.Officer));

            //profile image file extension validation
            if (ProfileImage != null && !GlobalController.isImage(ProfileImage))
            {
                TempData["message"] = MessagingSystem.AddMessage("حقل الصورة الشخصية يجب أن يكون صورة", "danger");
                return View(officerVM);
            }
            //check if specification is not empty
            //if (specificationlist == null || specificationlist.Count == 0)
            //{
            //    TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage("التخصص"), "danger");
            //    return View(officerVM);
            //}

            if (ModelState.IsValid)
            {
                db.Entry(officerVM.Officer).State = EntityState.Modified;

                //clearing old specification officerVM relation
                ClearSpecificationOfficer(officerVM.Officer);
                //adding selected specification to officerVM
                if (specificationlist != null)
                {
                    specificationlist.ForEach(delegate (int specificationId)
                    {
                        officerVM.Officer.Specifications.Add(db.Specifications.Find(specificationId));
                    });
                }
                
                clearOfficerExternalTournaments(officerVM.Officer);

                foreach (OfficerExternalTournament item in officerVM.OfficerExternalTournaments)
                {
                    //adding only non empty external tournaments
                    if (item.Name != null && !string.IsNullOrEmpty(item.Name.Trim()))
                    {
                        ExternalTournament extTournament = new ExternalTournament();
                        extTournament.Name = item.Name;
                        extTournament.OfficerId = officerVM.Officer.Id;
                        officerVM.Officer.ExternalTournaments.Add(extTournament);
                    }
                }

                ////if there is an old file and if delete document order file checkbox checked , delete it 
                if (!String.IsNullOrEmpty(OldProfileImage) && Boolean.Parse(DeleteProfileImage) == true)
                {
                    //delete physically
                    this.DeleteOfficerProfileImage(OldProfileImage);
                    //unset from database
                    officerVM.Officer.ProfileImage = null;
                }
                else
                {
                    //if we are uploading a new file
                    if (ProfileImage != null && ProfileImage.ContentLength > 0)
                    {
                        //if there is an old file
                        if (!String.IsNullOrEmpty(OldProfileImage))
                        {
                            this.DeleteOfficerProfileImage(OldProfileImage);
                        }
                        //file name as unix timestamp
                        string fileExtention = Path.GetExtension(ProfileImage.FileName);
                        //filename is for example 2018345434343.pdf
                        string fileName = String.Concat(HomeController.DateTimeToTimestamp(DateTime.Now).ToString(), fileExtention);
                        var path = Path.Combine(Server.MapPath("~/Media/" + WebConfigurationManager.AppSettings["OfficerProfileImageFolderName"]), fileName);
                        ProfileImage.SaveAs(path);
                        officerVM.Officer.ProfileImage = fileName;
                    }
                    else
                    {
                        // if we did not upload a new file, and we did not check to delete, then we need to keep the old file
                        officerVM.Officer.ProfileImage = OldProfileImage;
                    }
                }


                db.SaveChanges();
                TempData["message"] = MessagingSystem.AddMessage("تم تعديل الضابط ' " + officerVM.Officer.FirstName + " " + officerVM.Officer.LastName + "' بنجاح", "success");
                return RedirectToAction("Index");
            }

            return View(officerVM);
        }


        // GET: Officers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Officer officer = db.Officers.Find(id);
            if (officer == null)
            {
                return HttpNotFound();
            }
            return View(officer);
        }

        // POST: Officers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Officer officer = db.Officers.Find(id);
            db.Officers.Remove(officer);
            DeleteOfficerProfileImage(officer.ProfileImage);
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

        public void ClearSpecificationOfficer(Officer o)
        {
            db.Database.ExecuteSqlCommand($"DELETE FROM SpecificationOfficers WHERE Officer_Id = {o.Id}");
        }

        public List<int> GetspecificationOfficerIds(Officer o)
        {
            List<int> ids = new List<int>();
            List<int> data = db.Database.SqlQuery<int>($"SELECT  Specification_Id FROM SpecificationOfficers WHERE Officer_Id = {o.Id}").ToList();
            return data;
        }

        public static string GetOfficerSpecificationsString(Officer o)
        {
            string data = "";
            if (o.Specifications != null && o.Specifications.Count() > 0)
            {
                Specification lastSpec = o.Specifications.Last();
                foreach (var item in o.Specifications)
                {
                    data += item.Name;

                    if (!item.Equals(lastSpec))
                    {
                        data += " / ";
                    }
                }
            }
            return data;
        }

        public void clearOfficerExternalTournaments(Officer m)
        {
            db.Database.ExecuteSqlCommand($"DELETE FROM ExternalTournaments WHERE OfficerId = {m.Id}");
        }

        public static string getOfficerProfileImage(Officer o)
        {
            return !String.IsNullOrEmpty(o.ProfileImage) ? o.ProfileImage : WebConfigurationManager.AppSettings["DefaultOfficerProfileImage"];
        }


        public void DeleteOfficerProfileImage(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Media/" + WebConfigurationManager.AppSettings["OfficerProfileImageFolderName"]), filename));
            }
            return;
        }
    }
}
