using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reactive;
using System.Web;
using System.Web.Mvc;
using Team8_MVCApplication.DAL;
using Team8_MVCApplication.Models;

namespace Team8_MVCApplication.Controllers
{
    public class CoreValueRecognitionsController : Controller
    {
        private Team8_MVCApplication_Context db = new Team8_MVCApplication_Context();

        // GET: CoreValueRecognitions
        public ActionResult Index()
        {
            var coreValueRecognitions = db.CoreValueRecognitions.Include(c => c.personRecognized).Include(c => c.personRecognizor);
            return View(coreValueRecognitions.ToList());
        }

        // GET: CoreValueRecognitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoreValueRecognitions coreValueRecognitions = db.CoreValueRecognitions.Find(id);
            if (coreValueRecognitions == null)
            {
                return HttpNotFound();
            }
            return View(coreValueRecognitions);
        }

        // GET: CoreValueRecognitions/Create
        public ActionResult Create()
        {
            ViewBag.recognized = new SelectList(db.Profiles, "ProfileId", "profileFirstName");
            ViewBag.recognizor = new SelectList(db.Profiles, "ProfileId", "profileFirstName");
            return View();
        }

        // POST: CoreValueRecognitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,recognized,recognizor,award,recognizationDate,Comments")] CoreValueRecognitions coreValueRecognitions)
        {
            if (ModelState.IsValid)
            {
                db.CoreValueRecognitions.Add(coreValueRecognitions);
                db.SaveChanges();
                var recognizor = db.Profiles.Find(coreValueRecognitions.recognizor);
                var profile = db.Profiles.Find(coreValueRecognitions.recognized);
                var firstName = profile.profileFirstName;
                var lastName = profile.profileLastName;
                var fullNameRecognizor = recognizor.fullName;
                var email = profile.email;
                var msg = "Hi " + firstName + " " + lastName + ",\n\nWe wanted to notify you that your colleague, " + fullNameRecognizor + ", has recently given you a recognition.";
                msg += "\n\nTo access the recognition, please log into our system!";
                msg += "\n\nSincerely\nTeam 8";
                //notification += "<br/>" + firstName + " " + lastName + " at " + email;
                MailMessage myMessage = new MailMessage();
                MailAddress from = new MailAddress("nnjstull@gmail.com", "SysAdmin");
                myMessage.From = from;
                myMessage.To.Add(email);
                myMessage.Subject = "Recent recognition";
                myMessage.Body = msg;
                try
                {
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new System.Net.NetworkCredential("nnjstull@gmail.com", "An7igua@2095");
                    smtp.EnableSsl = true;
                    //smtp.Send(myMessage);
                    TempData["mailError"] = msg;
                    return View("mailError");
                }
                catch (Exception ex)
                {

                    TempData["mailError"] = ex.Message;
                    return View("mailError");
                }

                return RedirectToAction("Index");
            }

            ViewBag.recognized = new SelectList(db.Profiles, "ProfileId", "fullName", coreValueRecognitions.recognized);
            ViewBag.recognizor = new SelectList(db.Profiles, "ProfileId", "fullName", coreValueRecognitions.recognizor);
            return View(coreValueRecognitions);
        }

        // GET: CoreValueRecognitions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoreValueRecognitions coreValueRecognitions = db.CoreValueRecognitions.Find(id);
            if (coreValueRecognitions == null)
            {
                return HttpNotFound();
            }
            ViewBag.recognized = new SelectList(db.Profiles, "ProfileId", "profileFirstName", coreValueRecognitions.recognized);
            ViewBag.recognizor = new SelectList(db.Profiles, "ProfileId", "profileFirstName", coreValueRecognitions.recognizor);
            return View(coreValueRecognitions);
        }

        // POST: CoreValueRecognitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,recognized,recognizor,award,recognizationDate,Comments")] CoreValueRecognitions coreValueRecognitions)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coreValueRecognitions).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.recognized = new SelectList(db.Profiles, "ProfileId", "profileFirstName", coreValueRecognitions.recognized);
            ViewBag.recognizor = new SelectList(db.Profiles, "ProfileId", "profileFirstName", coreValueRecognitions.recognizor);
            return View(coreValueRecognitions);
        }

        // GET: CoreValueRecognitions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CoreValueRecognitions coreValueRecognitions = db.CoreValueRecognitions.Find(id);
            if (coreValueRecognitions == null)
            {
                return HttpNotFound();
            }
            return View(coreValueRecognitions);
        }

        // POST: CoreValueRecognitions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CoreValueRecognitions coreValueRecognitions = db.CoreValueRecognitions.Find(id);
            db.CoreValueRecognitions.Remove(coreValueRecognitions);
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
