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
using MVC_Officer_Mission.Models.EntityModels;
using System.IO;
using System.Globalization;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Data.SqlClient;

namespace MVC_Officer_Mission.Controllers
{
    [Authorize]
    public class MissionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Missions
        // [Authorize(Roles = "admin")]
        public ActionResult Index()
        {

            //if (!User.Identity.IsAuthenticated)
            //{
            //    Response.Redirect("~/Account/Login");
            //}
            //else
            //{
            //    if (User.IsInRole("general"))
            //    {
            //        Calendar_M_AND_T();
            //    }
            //    else
            //    {
            //        return View(db.Missions.ToList());
            //    }
            //}
            // return View(db.Missions.ToList());

            ViewBag.listOfOfficers = new SelectList(db.Officers.Where(m => m.IsInInstitute == true), "Id", "FullName");
            return View("Calendar_M_AND_T", "~/Views/Shared/_Layout.cshtml", db.Officers.Where(m => m.IsInInstitute == true).ToList().OrderByDescending(m => m.RankId).ThenBy(m => m.MilitaryNumber));
        }

        //returns  data for a specific tournament (used in officer tournaments popup)
        public ActionResult AjaxOfficerTournamentDetails(int? id)
        {
            Mission m = db.Missions.Find(id);
            return View("AjaxOfficerTournamentDetails", "~/Views/Shared/_AjaxLAyout.cshtml", m);
        }

        // GET: Missions/Create
        public ActionResult Create()
        {
            ViewBag.listOfOfficers = new SelectList(db.Officers.Where(m => m.IsInInstitute), "Id", "FullName");
            ViewBag.listOfBenefitedSides = new SelectList(db.BenefitedSides, "Id", "Name");
            ViewBag.listOfTournamentOfficers = new SelectList(db.Officers.Where(m => m.IsInInstitute), "Id", "FullName");
            return View();
        }

        // POST: Missions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            MissionTournamentViewModel missionVM,
            List<int> listOfOfficersDDL,
            List<int> listOfBenefitedSides,
            HttpPostedFileBase DocumentOrderFile
            )
        {
            List<string> ListOfficerHasManyMission = new List<string>(100);
            bool hasmissions, hastournaments;

            var dbCtxTransaction = db.Database.BeginTransaction();
            string mission_name = "المهمة";
            //  ضباط الدورة الموجودين في المعهد
            ViewBag.listOfOfficers = new SelectList(db.Officers.Where(m => m.IsInInstitute), "Id", "FullName");
            //الجهة المستفيدة
            ViewBag.listOfBenefitedSides = new SelectList(db.BenefitedSides, "Id", "Name");
            //reservation for listOfTournamentOfficers
            ViewBag.listOfTournamentOfficers = new SelectList[100];
            ViewBag.listOfTournamentOfficers[0] = new SelectList(db.Officers, "Id", "FullName");

            //saving listOfTournamentOfficers ids in order to generate the list in form upon error
            for (int i = 0; i < missionVM.listOfTournamentOfficers.Count(); i++)
            {
                ViewBag.listOfTournamentOfficers[i] = new SelectList(db.Officers.Where(m => m.IsInInstitute), "Id", "FullName", missionVM.listOfTournamentOfficers[i].OfficerId);
            }

            //if from > to throw error
            if (DateTime.Compare(missionVM.Mission.From, missionVM.Mission.To) > 0)
            {
                TempData["message"] = MessagingSystem.AddMessage("يجب التأكد من تاريخ البدء و تاريخ الانتهاء", "danger");
                return View(missionVM);
            }

            if (ModelState.IsValid)
            {
                //IF TOURNAMENT, create tournament then link it to mission
                if (missionVM.Mission.Istournament)
                {
                    mission_name = "الدورة";


                    //if no officers with roles were selected throw error 
                    if (missionVM.listOfTournamentOfficers == null || missionVM.listOfTournamentOfficers.Count() == 0)
                    {
                        TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage("اسم الضابط"), "danger");
                        return View(missionVM);
                    }

                    //if tournament, then benefited side is required
                    if (listOfBenefitedSides == null || listOfBenefitedSides.Count == 0)
                    {
                        TempData["message"] = MessagingSystem.AddMessage("يجب إدخال جهة مستفيدة", "danger");
                        return View(missionVM);
                    }
                    //saving the benefitedSideId
                    missionVM.Tournament.BenefitedSideId = listOfBenefitedSides.First();

                    //Document Order File Handle
                    if (DocumentOrderFile != null && DocumentOrderFile.ContentLength > 0)
                    {
                        //file name as unix timestamp
                        string fileExtention = Path.GetExtension(DocumentOrderFile.FileName);
                        //filename is for example 2018345434343.pdf
                        string fileName = String.Concat(HomeController.DateTimeToTimestamp(DateTime.Now).ToString(), fileExtention);
                        var path = Path.Combine(Server.MapPath("~/Media/" + WebConfigurationManager.AppSettings["TournamentDocumentOrderFolderName"]), fileName);
                        DocumentOrderFile.SaveAs(path);
                        missionVM.Tournament.DocumentOrderFile = fileName;
                    }

                    //SAVING TOURNAMENT
                    missionVM.Mission.Tournament = db.Tournaments.Add(missionVM.Tournament);

                    //saving officerRoleInMission List for created tournament
                    foreach (var item in missionVM.listOfTournamentOfficers)
                    {
                        OfficerRoleInMission ORIM = new OfficerRoleInMission();
                        ORIM.OfficerId = item.OfficerId;
                        //  Officer officer = db.Officers.Find(item.OfficerId);
                        ORIM.OfficerRole = item.OfficerRole;
                        ORIM.TournamentId = missionVM.Mission.Tournament.Id;

                        // check if has mission or tournament then add to list 
                        missionVM.Tournament.OfficersRolesInMission.Add(ORIM);
                        hasmissions = HasMissionBetweenBothDate(item.OfficerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        hastournaments = HasTournamentBetweenBothDate(item.OfficerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        if (hasmissions || hastournaments)
                        {
                            Officer officer = db.Officers.Find(item.OfficerId);
                            ListOfficerHasManyMission.Add(officer.FullName);
                        }
                    }


                    db.SaveChanges();

                }
                else
                {
                    //IF NOT TOURNAMENT 

                    //if no officers were selected throw error
                    if (listOfOfficersDDL == null || listOfOfficersDDL.Count == 0)
                    {
                        TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage("اسم الضابط"), "danger");
                        return View(missionVM);
                    }
                    //adding selected officers objects to mission
                    listOfOfficersDDL.ForEach(delegate (int officerId)
                    {
                        missionVM.Mission.Officers.Add(db.Officers.Find(officerId));

                        // check if has mission or tournament then add to list 
                        hasmissions = HasMissionBetweenBothDate(officerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        hastournaments = HasTournamentBetweenBothDate(officerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        if (hasmissions || hastournaments)
                        {
                            Officer officer = db.Officers.Find(officerId);
                            ListOfficerHasManyMission.Add(officer.FullName);
                        }

                    });
                }



                db.Missions.Add(missionVM.Mission);
                db.SaveChanges();
                AddMissionLog(User.Identity.GetUserId(), missionVM.Mission.Id, missionVM.Mission.Name, 0);
                dbCtxTransaction.Commit();
                TempData["message"] = MessagingSystem.AddMessage("تم إنشاء '" + mission_name + "' بنجاح", "success");

                //check if officers have missions at same time
                string additionalMessage = "";
                int index = 1;
                if (ListOfficerHasManyMission.Count > 0)
                {
                    foreach (string item in ListOfficerHasManyMission)
                    {
                        additionalMessage +=  String.Format("{0} - {1} <br/>",index,item.ToString());
                        index++;
                    }

                    TempData["additionalMessage"] = MessagingSystem.AddMessage(additionalMessage, "warning");
                }

                return RedirectToAction("Index");
            }

            return View(missionVM);
        }

        // GET: Missions/Edit/5
        public ActionResult Edit(int? id, MissionTournamentViewModel MissionTournamentVM)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MissionTournamentViewModel missionVM = new MissionTournamentViewModel();
            missionVM.Mission = db.Missions.Find(id);
            string mission_name = "المهمة ";

            //if is tournment we fill the benefitedsides dropdown
            if (missionVM.Mission.Istournament)
            {
                mission_name = "الدورة ";


                missionVM.Tournament = db.Tournaments.Find(missionVM.Mission.TournamentID);

                ViewBag.listOfBenefitedSides = new SelectList(db.BenefitedSides, "Id", "Name", missionVM.Tournament.BenefitedSideId);
                //reservation for listOfTournamentOfficers
                ViewBag.listOfTournamentOfficers = new SelectList[missionVM.Tournament.OfficersRolesInMission.Count()];
                //reservation for view model listOfTournamentOfficers 
                //missionVM.listOfTournamentOfficers = new TournamentOfficerRole[missionVM.Tournament.OfficersRolesInMission.Count];
                //looping on officer roles in mission for tournament
                for (int i = 0; i < missionVM.Tournament.OfficersRolesInMission.Count(); i++)
                {
                    TournamentOfficerRole TOR = new TournamentOfficerRole();
                    TOR.OfficerId = (missionVM.Tournament.OfficersRolesInMission.ToArray())[i].OfficerId;
                    TOR.OfficerRole = (missionVM.Tournament.OfficersRolesInMission.ToArray())[i].OfficerRole;
                    missionVM.listOfTournamentOfficers.Add(TOR);

                    ViewBag.listOfTournamentOfficers[i] = new SelectList(db.Officers, "Id", "FullName", (missionVM.Tournament.OfficersRolesInMission.ToArray())[i].OfficerId);
                }
            }
            else //if not tournament we fill the officers list dropdown
            {
                ViewBag.listOfOfficers = new MultiSelectList(db.Officers, "Id", "FullName", GetMissionOfficerIds(missionVM.Mission));
            }

            if (missionVM.Mission == null)
            {
                return HttpNotFound();
            }

            ViewBag.MissionType = mission_name;
            return View(missionVM);
        }

        // POST: Missions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            MissionTournamentViewModel missionVM,
            List<int> listOfOfficersDDL,
            List<int> listOfBenefitedSidesDDL,
             HttpPostedFileBase DocumentOrderFile,
             string DeleteDocumentOrderFile,
             string OldDocumentOrderFile)
        {
            List<string> ListOfficerHasManyMission = new List<string>(100);
            bool hasmissions, hastournaments;

            string mission_name = "المهمة ";
            //ViewBag.listOfOfficers = new SelectList(db.Officers, "Id", "FullName");
            ViewBag.listOfOfficers = new MultiSelectList(db.Officers, "Id", "FullName", listOfOfficersDDL);
            ViewBag.listOfBenefitedSides = new SelectList(db.BenefitedSides, "Id", "Name");

            //reservation for listOfTournamentOfficers
            ViewBag.listOfTournamentOfficers = new SelectList[100];
            ViewBag.listOfTournamentOfficers[0] = new SelectList(db.Officers, "Id", "FullName");

            //saving listOfTournamentOfficers ids in order to generate the list in form upon error
            for (int i = 0; i < missionVM.listOfTournamentOfficers.Count(); i++)
            {
                ViewBag.listOfTournamentOfficers[i] = new SelectList(db.Officers, "Id", "FullName", missionVM.listOfTournamentOfficers[i].OfficerId);
            }

            //if from > to throw error
            if (DateTime.Compare(missionVM.Mission.From, missionVM.Mission.To) > 0)
            {
                TempData["message"] = MessagingSystem.AddMessage("يجب التأكد من تاريخ البدء و تاريخ الانتهاء", "danger");
                ViewBag.listOfOfficers = new MultiSelectList(db.Officers, "Id", "FullName", listOfOfficersDDL);
                return View(missionVM);
            }

            if (ModelState.IsValid)
            {
                var dbCtxTransaction = db.Database.BeginTransaction();
                db.Entry(missionVM.Mission).State = EntityState.Modified;

                //if tournament
                if (missionVM.Mission.Istournament)
                {
                    db.Entry(missionVM.Tournament).State = EntityState.Modified;
                    mission_name = "الدورة ";

                    //if no benefited sides were selected throw error
                    if (listOfBenefitedSidesDDL == null || listOfBenefitedSidesDDL.Count == 0)
                    {
                        TempData["message"] = MessagingSystem.AddMessage("يجب إدخال جهة مستفيدة", "danger");
                        return View(missionVM);
                    }

                    //if no officers with roles were selected throw error 
                    if (missionVM.listOfTournamentOfficers == null || missionVM.listOfTournamentOfficers.Count() == 0)
                    {
                        TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage("اسم الضابط"), "danger");
                        return View(missionVM);
                    }


                    //clearing old officer roles in mission for this current tournament
                    ClearTournamentOfficerRolesInMission(missionVM.Mission);

                    //saving officerRoleInMission List for created tournament
                    foreach (var item in missionVM.listOfTournamentOfficers)
                    {
                        OfficerRoleInMission ORIM = new OfficerRoleInMission();
                        ORIM.OfficerId = item.OfficerId;
                        ORIM.OfficerRole = item.OfficerRole;
                        ORIM.TournamentId = missionVM.Mission.Tournament.Id;
                        // check if has mission or tournament then add to list 
                        missionVM.Tournament.OfficersRolesInMission.Add(ORIM);

                        hasmissions = HasMissionBetweenBothDate(item.OfficerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        hastournaments = HasTournamentBetweenBothDate(item.OfficerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        if (hasmissions || hastournaments)
                        {
                            Officer officer = db.Officers.Find(item.OfficerId);
                            ListOfficerHasManyMission.Add(officer.FullName);
                        }
                    }
                    //saving the benefitedSideId
                    missionVM.Tournament.BenefitedSideId = listOfBenefitedSidesDDL.First();
                    //if delete document order file checkbox checked
                    if (Boolean.Parse(DeleteDocumentOrderFile) == true)
                    {
                        //if there is an old file, delete it
                        if (!String.IsNullOrEmpty(OldDocumentOrderFile))
                        {
                            //delete physically
                            this.DeleteDocumentOrderFile(OldDocumentOrderFile);
                            //unset from database
                            missionVM.Tournament.DocumentOrderFile = null;
                        }
                    }
                    else
                    {
                        //if we are uploading a new file
                        if (DocumentOrderFile != null && DocumentOrderFile.ContentLength > 0)
                        {
                            //if there is an old file
                            if (!String.IsNullOrEmpty(OldDocumentOrderFile))
                            {
                                this.DeleteDocumentOrderFile(OldDocumentOrderFile);
                            }
                            //file name as unix timestamp
                            string fileExtention = Path.GetExtension(DocumentOrderFile.FileName);
                            //filename is for example 2018345434343.pdf
                            string fileName = String.Concat(HomeController.DateTimeToTimestamp(DateTime.Now).ToString(), fileExtention);
                            var path = Path.Combine(Server.MapPath("~/Media/" + WebConfigurationManager.AppSettings["TournamentDocumentOrderFolderName"]), fileName);
                            DocumentOrderFile.SaveAs(path);
                            missionVM.Tournament.DocumentOrderFile = fileName;
                        }
                        else
                        {
                            // if we did not upload a new file, and we did not check to delete, then we need to keep the old file
                            missionVM.Tournament.DocumentOrderFile = OldDocumentOrderFile;
                        }
                    }
                }
                else //if not tournament, then officers is now required
                {
                    //if no officers were selected throw error
                    if (listOfOfficersDDL == null || listOfOfficersDDL.Count == 0)
                    {
                        TempData["message"] = MessagingSystem.AddMessage(GlobalController.overridenRequiredFieldMessage("اسم الضابط"), "danger");
                        return View(missionVM);
                    }

                    //clearing old officers relation
                    ClearMissionOfficers(missionVM.Mission);
                    //adding selected officers objects to mission
                   
                    listOfOfficersDDL.ForEach(delegate (int officerId)
                    {
                        missionVM.Mission.Officers.Add(db.Officers.Find(officerId));

                        // check if has mission or tournament then add to list 
                        hasmissions = HasMissionBetweenBothDate(officerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        hastournaments = HasTournamentBetweenBothDate(officerId, missionVM.Mission.From.ToString("yyyy-MM-dd"), missionVM.Mission.To.ToString("yyyy-MM-dd"));
                        if (hasmissions || hastournaments)
                        {
                            Officer officer = db.Officers.Find(officerId);
                            ListOfficerHasManyMission.Add(officer.FullName);
                        }

                    });
                }
                ViewBag.MissionType = mission_name;
                TempData["message"] = MessagingSystem.AddMessage("تم تعديل '" + mission_name + "' بنجاح", "success");

                db.SaveChanges();

                AddMissionLog(User.Identity.GetUserId(), missionVM.Mission.Id, missionVM.Mission.Name, 1);


                dbCtxTransaction.Commit();

                //check if officers have missions at same time
                string additionalMessage = "";
                int index = 1;
                if (ListOfficerHasManyMission.Count > 0)
                {
                    foreach (string item in ListOfficerHasManyMission)
                    {
                        additionalMessage += String.Format("{0} - {1} <br/>", index, item.ToString());
                        index++;
                    }

                    TempData["additionalMessage"] = MessagingSystem.AddMessage(additionalMessage, "warning");
                }

                return RedirectToAction("Index");
            }
            return View(missionVM);
        }

        // GET: Missions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mission mission = db.Missions.Find(id);
            if (mission == null)
            {
                return HttpNotFound();
            }
            return View(mission);
        }

        // POST: Missions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string mission_name = "المهمة";
            MissionTournamentViewModel missionVM = new MissionTournamentViewModel();
            missionVM.Mission = db.Missions.Find(id);
            //if is tournament,delete data from database
            if (missionVM.Mission.Istournament)
            {
                mission_name = "الدورة";
                //deleting documentorderfile (if there was any)
                DeleteDocumentOrderFile(missionVM.Mission.Tournament.DocumentOrderFile);
                missionVM.Tournament = db.Tournaments.Find(id);
                //deleting the tournament data from database
                db.Tournaments.Remove(missionVM.Mission.Tournament);
            }
            AddMissionLog(User.Identity.GetUserId(), missionVM.Mission.Id, missionVM.Mission.Name, 2);

            //deleting the mission itself
            db.Missions.Remove(missionVM.Mission);
            db.SaveChanges();
            TempData["message"] = MessagingSystem.AddMessage("تم حذف '" + mission_name + "' بنجاح", "success");

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
        public void DeleteDocumentOrderFile(string filename)
        {
            if (!string.IsNullOrEmpty(filename))
            {
                System.IO.File.Delete(Path.Combine(Server.MapPath("~/Media/" + WebConfigurationManager.AppSettings["TournamentDocumentOrderFolderName"]), filename));
            }
            return;
        }
        public List<int> GetMissionOfficerIds(Mission m)
        {
            List<int> ids = new List<int>();
            List<int> data = db.Database.SqlQuery<int>($"SELECT  Officer_Id FROM MissionOfficers WHERE Mission_Id = {m.Id}").ToList();
            return data;
        }


        public static bool OfficerHasMissionInDay(Officer o, DateTime date)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Missions.Where(m => m.Officers.Select(c => c.Id).Contains(o.Id) && date >= m.From && date <= m.To).Count() > 0;

        }
        //gets the missions of a specific officer in the month of the @date parameter
        public static List<Mission> GetOfficerMissionsInMonth(Officer o, DateTime date)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return db.Missions.Where(
                                        //if this officer is in the list of officers for a mission
                                        m => (
                                                    m.Officers.Select(c => c.Id).Contains(o.Id)
                                                    ||
                                                    // or if the missions is tournament and this officer has role in it
                                                    (m.Istournament && m.Tournament.OfficersRolesInMission.Select(c => c.OfficerId).Contains(o.Id))
                                             )
                                             &&
                                             (
                                                    (date.Month == m.From.Month && date.Year == m.From.Year) || 
                                                    (date.Month == m.To.Month && date.Year == m.To.Year) ||
                                                    (date >= m.From && date <= m.To)
                                             )
                                  ).ToList();
        }



        public void ClearMissionOfficers(Mission m)
        {
            db.Database.ExecuteSqlCommand($"DELETE FROM MissionOfficers WHERE Mission_Id = {m.Id}");
        }
        //samih
        public void ClearTournamentOfficerRolesInMission(Mission m)
        {
            db.Database.ExecuteSqlCommand($"DELETE FROM OfficerRoleInMissions WHERE TournamentId = {m.TournamentID}");
        }

        //gets the display name for a mission
        // if name word count <= days between from and to, return full name, else return only words equal to days between concatinated with ...
        public static string GetMissionDisplayName(Mission m, string extraNameToBeAddedFirst, int year, int month)
        {
            string fullName = (extraNameToBeAddedFirst + " " + m.Name).Trim();
            int wordCount = GlobalController.WordCount(fullName);
            DateTime currentMonthStart = new DateTime(year, month, 1);
            DateTime currentMonthEnd = new DateTime(year, month, DateTime.DaysInMonth(year, month));
            DateTime startDate = m.From <= currentMonthStart ? currentMonthStart : m.From;
            DateTime endDate = m.To >= currentMonthEnd ? currentMonthEnd : m.To;
            int missionDays = startDate < endDate ? GlobalController.DaysBetween(startDate, endDate) + 1 : 1;
            if (missionDays <= 2)
            {
                return String.Join(" ", fullName.Split(' ').Take(1)) + "...";
            }
            if (wordCount < missionDays - 2)
            {
                return fullName;
            }
            return String.Join(" ", fullName.Split(' ').Take(missionDays - 2)) + "...";
        }


        public ActionResult Calendar_M_AND_T()
        {
            ViewBag.listOfOfficers = new SelectList(db.Officers.Where(m => m.IsInInstitute == true), "Id", "FullName");
            return View("Calendar_M_AND_T", "~/Views/Shared/_Layout.cshtml", db.Officers.Where(m => m.IsInInstitute == true).ToList().OrderByDescending(m => m.RankId).ThenBy(m => m.MilitaryNumber));
        }

        public static string GetUserInsertedMission(Mission mission)
        {
            string UserId;
            ApplicationDbContext db = new ApplicationDbContext();
            UserId = db.MissionLogs.Where(m => m.MissionId == mission.Id).Select(x => x.UserId).FirstOrDefault()?.ToString();
            return db.Users.Where(m => m.Id == UserId).Select(n => n.UserName).FirstOrDefault()?.ToString();
        }

        // GET: Missions/GetAjaxData
        public ActionResult GetAjaxData()
        {
            return View("GetAjaxData", "~/Views/Shared/_AjaxLAyout.cshtml", db.Missions.Where(m => m.Istournament == true).ToList());
        }
        public static void AddMissionLog(string UserId, int MissionId, string MissionName, int ActionId)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            MissionLog ML = new MissionLog();
            ML.UserId = UserId;
            ML.MissionId = MissionId;
            ML.DateModified = DateTime.Now;
            ML.mission_name = MissionName;
            ML.Action_Id = ActionId;
            db.MissionLogs.Add(ML);
            db.SaveChanges();
        }

        //public  bool OfficerHasMissionInBetweenDate(string officer_Id, DateTime startdate , DateTime endate)
        //{
        //    db.Database.ExecuteSqlCommand("$select ");
        //   // return db.Missions.Where(m => m.Officers.Select(c => c.Id).Contains(o.Id) && startda endate <= m.To).Count() > 0;

        //}
        public bool HasMissionBetweenBothDate(int OfficerId, string StartDate, string EndDate)
        {
            return db.Database.SqlQuery<CountEntity>("GetOfficerMissionsBetweenBothDate @OfficerId,@StartDate,@EndDate",
                new SqlParameter("@OfficerId", OfficerId), new SqlParameter("@StartDate", StartDate), new SqlParameter("@EndDate", EndDate)).Count() > 0;

        }

        public bool HasTournamentBetweenBothDate(int OfficerId, string StartDate, string EndDate)
        {
            return db.Database.SqlQuery<CountEntity>("GetOfficerTournamentsBetweenBothDate @OfficerId,@StartDate,@EndDate",
                new SqlParameter("@OfficerId", OfficerId), new SqlParameter("@StartDate", StartDate), new SqlParameter("@EndDate", EndDate)).Count() > 0;

        }
    }
}
