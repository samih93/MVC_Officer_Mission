using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Officer_Mission.Models;

namespace MVC_Officer_Mission.Controllers
{
    [Authorize(Roles = "admin")]
    public class SpecificationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Specifications
        public async Task<ActionResult> Index()
        {
            return View(await db.Specifications.OrderBy(m => m.Name).ToListAsync());
        }

        // GET: Specifications/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Specifications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name")] Specification specification)
        {
            if (ModelState.IsValid)
            {
                db.Specifications.Add(specification);
                await db.SaveChangesAsync();
                TempData["message"] = MessagingSystem.AddMessage("تم اضافة التخصص ' " + specification.Name + "' بنجاح", "success");
                return RedirectToAction("Index");
            }

            return View(specification);
        }

        // GET: Specifications/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specification specification = await db.Specifications.FindAsync(id);
            if (specification == null)
            {
                return HttpNotFound();
            }
            return View(specification);
        }

        // POST: Specifications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name")] Specification specification)
        {
            if (ModelState.IsValid)
            {
                db.Entry(specification).State = EntityState.Modified;
                await db.SaveChangesAsync();
                TempData["message"] = MessagingSystem.AddMessage("تم تعديل التخصص ' "+specification.Name+"' بنجاح", "success");
                return RedirectToAction("Index");
            }
            return View(specification);
        }

        // GET: Specifications/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Specification specification = await db.Specifications.FindAsync(id);
            if (specification == null)
            {
                return HttpNotFound();
            }
            return View(specification);
        }

        // POST: Specifications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Specification specification = await db.Specifications.FindAsync(id);
            db.Specifications.Remove(specification);
            await db.SaveChangesAsync();
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
