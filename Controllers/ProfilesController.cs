using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Team8_MVCApplication.DAL;
using Team8_MVCApplication.Models;

namespace Team8_MVCApplication.Controllers
{
    public class ProfilesController : Controller
    {
        private Team8_MVCApplication_Context db = new Team8_MVCApplication_Context();

        // GET: Profiles
        public ActionResult Index(string searchString)
        {
            var testProfile = from u in db.Profiles select u;
            if (!String.IsNullOrEmpty(searchString))
            {
                testProfile = testProfile.Where(u =>
                u.profileLastName.Contains(searchString)
                   || u.profileFirstName.Contains(searchString));
                // if here, users were found so view them
                return View(testProfile.ToList());
            }
            return View(db.Profiles.ToList());
        }

        // GET: Profiles/Details/5
        [Authorize]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var profile = db.Profiles.Where(p => p.ProfileId == id).Include(p => p.BeingRecognized);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile.ToList());
        }

        // GET: Profiles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProfileId,profileFirstName,profileLastName,email,businessUnit,phoneNumber,title,hireDate")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                profile.ProfileId = Guid.NewGuid();
                db.Profiles.Add(profile);
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {

                    return View("DuplicateUser");
                }

            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProfileId,profileFirstName,profileLastName,email,businessUnit,phoneNumber,title,hireDate")] Profile profile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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
